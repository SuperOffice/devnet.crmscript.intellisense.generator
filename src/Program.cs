using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace devnet.crmscript.intellisense.generator
{
    class Program
    {
        const string BASE_URL = "https://docs.superoffice.com/api/reference/crmscript";
        const string GLOBAL = "CRMScript.Global.Void";
        const string NATIVE = "CRMScript.Native";
        const string NETSERVER = "CRMScript.NetServer";
        const bool   INCLUDE_DOCS_LINKS = true;

        static int Main(string[] args)
        {
            if( args.Length < 2 )
            {
                Console.WriteLine("GenerateTooltips  outputFilename  pathToCRMScriptYamlFolder");
                return 1;
            }
            Console.WriteLine("Generate Tooltips from GitHub files");
            var outputFilename = Path.GetFullPath( args[0] );
            var rootDir = Path.GetFullPath( args[1] );

            Console.WriteLine("Output: {0}", outputFilename);
            Console.WriteLine("Root: {0}", rootDir);

            if (!Directory.Exists(rootDir))
            {
                Console.WriteLine("Root dir does not exist");
                return 2;
            }

            var deserializer = new DeserializerBuilder().Build();

            // The root directory should point to the crmscript repo's api-reference folder.
            // crmscript\docs\api-reference

            // exclude Triggers events
            var crmscriptFiles = Directory.GetFiles(rootDir, "*.yml")
                .Where(f => !f.Contains("toc.yml") && !f.Contains("CRMScript.Event")).ToArray();
            
            var intellisense = new List<Intellisense>();


            foreach (var fileName in crmscriptFiles)
            {
                var preservedText = File.ReadAllText(fileName);
                var file = deserializer.Deserialize<YmlFile>(preservedText);
                foreach (var item in file.Items)
                {
                    if (item.Type.Equals("Namespace")
                     || item.Type.Equals("Enum")
                     || item.Type.Equals("Class")
                     || item.Type.Equals("Field"))
                        continue;

                    intellisense.Add(new Intellisense()
                    {
                        text = GetText(item),
                        help = GetHelp(item)
                    });
                }
            }



            intellisense.Sort(new IntellisenseComparer());
            intellisense.Add(new Intellisense { text = "Void.for", help = " Example code: \nInteger count = 10;\nfor(Integer i = 1; i < count; i++) {\n\tprintLine(i.toString());\n}\n" });
            intellisense.Add(new Intellisense { text = "Void.foreach", help = " Example code: \nforeach(Type in array) {\n\n}\n" });
            intellisense.Add(new Intellisense { text = "Void.if", help = " Example code: \nif (false) {\n\n}\n" });
            intellisense.Add(new Intellisense { text = "Void.if-else", help = " Example code: \nif (false) {\n\n}\nelse if (false) {\n\n}\n" });
            intellisense.Add(new Intellisense { text = "Void.struct", help = " Example code: \nstruct {\n\n};\n" });
            intellisense.Add(new Intellisense { text = "Void.try", help = " Example code: \ntry {\n\n}\n" });
            intellisense.Add(new Intellisense { text = "Void.try-catch", help = " Example code: \ntry {\n\n}\ncatch {\nprintLine(\"Exception caught: \" + error);\nprintLine(\"...at \" + errorLocation);\n}\n" });
            intellisense.Add(new Intellisense { text = "Void.while", help = " Example code: \nwhile (i < 10) {\n\n}\n" });
            intellisense.Add(new Intellisense { text = "Void.script-nuggets", help = " Example code: \n%EJSCRIPT_START%\n<%\n%>\n%EJSCRIPT_END%\n" });


            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string jsonString = JsonSerializer.Serialize(intellisense, options);

            var generatedDate = string.Concat("// generateTooltips ", DateTime.Now.ToString("o"), Environment.NewLine);

            File.WriteAllText(outputFilename, string.Concat(generatedDate, "var ejScriptIntellisense = ", jsonString));

            Console.WriteLine("Wrote: {0}", outputFilename);

            return 0;
        }

        private static string GetText(Item item)
        {
            return item.SoIntellisense;
        }

        private static string GetHelp(Item item)
        {
            var result = string.Empty;

            string protoAndTable = GetPrototypeWithParameters(item);
            string remarks = GetRemarks(item);
            string examples = GetExamples(item);

            result = string.Concat(protoAndTable, remarks, examples);

            return result;

        }

        private static string GetExamples(Item item)
        {
            string result = string.Empty;

            if (item.Example != null && item.Example.Length > 0)
            {
                List<string> examples = new List<string>(1);

                foreach (var example in item.Example)
                {
                    examples.Add(example);
                }

                result = string.Concat("Example code:", string.Join('\n', examples));
            }

            return result;
        }

        private static string GetRemarks(Item item)
        {
            if (!string.IsNullOrWhiteSpace(item.Remarks))
                return item.Remarks.ToItalicTag().ToParagraphTag();
            return string.Empty;
        }

        private static string GetPrototypeWithParameters(Item item)
        {
            var firstLine = item.Syntax.Content.ToHeaderTag(1);
            var docLink = INCLUDE_DOCS_LINKS ? GetDocLink(item) : string.Empty;
            var hasParameters = item.Syntax?.Parameters != null && item.Syntax?.Parameters.Length > 0;
            var parameters = string.Empty;
            var rows = new List<string>();

            if (hasParameters)
            {
                foreach (var param in item.Syntax?.Parameters)
                {
                    hasParameters = true;
                    rows.Add(GetRow(param));
                }
            }

            // Prototype
            // Summary + DocLink
            // Parameter Table


            var summary = string.Empty;
            if (!string.IsNullOrWhiteSpace(item.Summary))
                summary = string.Concat(item.Summary.Trim(new char[] { '\n' }), docLink);
            else
                summary = docLink;

            return hasParameters ? string.Concat(firstLine, summary.ToParagraphTag(), GetTable(string.Join(' ', rows))) : string.Concat(firstLine, summary.ToParagraphTag());
        }

        private static string GetDocLink(Item item)
        {
            // item.Parent is used to identity the root doc page that contains 
            // all methods of that type, i.e. CRMScript.Global.Bool.
            // NetServer and Native class do not have item.Parent defined,
            // so we use this to resolve the root page name, i.e. CRMScript.Native.AppointmentSlicer

            if (string.IsNullOrWhiteSpace(item.Parent))
            {

                if (item.Uid.StartsWith(Program.GLOBAL))
                {
                    item.Parent = Program.GLOBAL;
                }
                else if (item.Uid.StartsWith(Program.NATIVE))
                {
                    var name = item.NameWithType.Substring(0, item.NameWithType.IndexOf('.'));
                    item.Parent = string.Concat(Program.NATIVE, ".", name);

                }
                else if (item.Uid.StartsWith(Program.NETSERVER))
                {
                    var name = item.NameWithType.Substring(0, item.NameWithType.IndexOf('.'));
                    item.Parent = string.Concat(Program.NETSERVER, ".", name);
                }
                else
                {
                    // should never get here...
                    Console.WriteLine($"No DocLink UID Matches: {item.Uid}");
                }
            }


            var uid = Regex.Replace(item.Uid, "[()#,.]", "_");

            //var formatString = @" <a href=""{Program.BASE_URL}/{item.Parent}.html?{uid}\" target =\"_blank\">DocLink</a>"";
            var formatString = @" <a href=""{0}/{1}.html#{2}"" target=""_blank"" onclick=""event.stopPropagation();"">Docs</a>";
            string docLink = string.Format(formatString, Program.BASE_URL, item.Parent, uid);

            return docLink;
        }

        static string GetRow(DataType dataType)
        {
            // strip away namespace prefix, i.e. 'CrmScript.Global.'Bool
            var position = dataType?.Type.LastIndexOf('.') + 1;
            var type = dataType?.Type.Substring(position.Value);

            var row = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";
            return string.Format(row, type, dataType?.Id, dataType?.Description);
        }

        static string GetTable(string rows)
        {
            //var table = @"<table style=""text-align:left; border-collapse:collapse; border-color:#ccc;border-spacing:0"" with=""100%""><thead style=""border - color:black; background - color:#ddd;border-style:solid;border-width:1px;""><tr><th> Type </th><th> Name </th><th> Description </th></tr></thead><tbody>{0}</tbody></table>";
            var table = @"<table width=""100%""><thead><tr><th> Type </th><th> Name </th><th> Description </th></tr></thead><tbody>{0}</tbody></table>";
            return string.Format(table, rows);
        }

        static string GetRemarks(string remarks)
        {
            if (string.IsNullOrEmpty(remarks))
                return string.Empty;

            return $"<p><strong>Remarks:</strong></p>{remarks.ToParagraphTag()}";
        }
    }
}
