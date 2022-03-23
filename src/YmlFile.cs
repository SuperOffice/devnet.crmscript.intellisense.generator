using System;
using System.Collections.Generic;
using System.Globalization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace devnet.crmscript.intellisense.generator
{

    public partial class YmlFile
    {
        [YamlMember(Alias = "items")]
        public Item[] Items { get; set; }

        [YamlMember(Alias = "references")]
        public Reference[] References { get; set; }
    }

    public partial class Item
    {
        [YamlMember(Alias = "parent")]
        public string Parent { get; set; }

        [YamlMember(Alias = "uid")]
        public string Uid { get; set; }

        [YamlMember(Alias = "commentId")]
        public string CommentId { get; set; }

        [YamlMember(Alias = "so.envir")]
        public string SoEnvir { get; set; }

        [YamlMember(Alias = "id")]
        public string Id { get; set; }

        [YamlMember(Alias = "langs")]
        public string[] Langs { get; set; }

        [YamlMember(Alias = "children")]
        public string[] Children { get; set; }

        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "nameWithType")]
        public string NameWithType { get; set; }

        [YamlMember(Alias = "fullName")]
        public string FullName { get; set; }

        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        [YamlMember(Alias = "source")]
        public Source Source { get; set; }

        [YamlMember(Alias = "assemblies")]
        public string[] Assemblies { get; set; }

        [YamlMember(Alias = "namespace")]
        public string Namespace { get; set; }

        [YamlMember(Alias = "summary")]
        public string Summary { get; set; }

        [YamlMember(Alias = "example")]
        public string[] Example { get; set; }

        private string _intellisense = string.Empty;


        [YamlMember(Alias = "so.intellisense", ScalarStyle = YamlDotNet.Core.ScalarStyle.Literal)]
        public string SoIntellisense
        {
            get { return _intellisense; }
            set { _intellisense = value; }
        }
        [YamlMember(Alias = "intellisense", ScalarStyle = YamlDotNet.Core.ScalarStyle.Literal)]
        public string SoIntellisense2
        {
            get { return _intellisense; }
            set { _intellisense = value; }
        }


        [YamlMember(Alias = "so:intellisense", ScalarStyle = YamlDotNet.Core.ScalarStyle.Literal)]
        public string SoIntellisense3
        {
            get { return _intellisense; }
            set { _intellisense = value; }
        }

        [YamlMember(Alias = "se.intellisense", ScalarStyle = YamlDotNet.Core.ScalarStyle.Literal)]
        public string SoIntellisense4
        {
            get { return _intellisense; }
            set { _intellisense = value; }
        }



        [YamlMember(Alias = "remarks")]
        public string Remarks { get; set; }

        [YamlMember(Alias = "syntax")]
        public Syntax Syntax { get; set; }

        [YamlMember(Alias = "so.version")]
        public string SoVersion { get; set; }

        public override string ToString()
        {
            var result = string.Empty;

            switch (Type?.ToLower())
            {
                case "class":
                    return GetClassDefinition();

                // used by global data types
                case "constructor":
                    return GetConstructorDefinition();
                
                case "method":

                    // is this a class constructor
                    if (Name.Contains('#'))
                    {
                        return GetConstructorDefinition(true);
                    }

                    return GetMethodDefinition();

                default:
                    break;
            }


            return result;
        }

        private string GetMethodDefinition()
        {
            var result = string.Empty;

            return result;
        }

        private string GetConstructorDefinition(
            bool isMethod = false)
        {
            var result = string.Empty;

            if (isMethod)
            { 
                
            }
            else
            {

            }

            return result;
        }

        private string GetClassDefinition()
        {
            var result = string.Empty;

            return result;
        }
    }

    public partial class Source
    {
        [YamlMember(Alias = "path")]
        public string Path { get; set; }

        [YamlMember(Alias = "isExternal")]
        public bool IsExternal { get; set; }
    }

    public partial class Syntax
    {
        [YamlMember(Alias = "content")]
        public string Content { get; set; }

        [YamlMember(Alias = "parameters")]
        public DataType[] Parameters { get; set; }

        [YamlMember(Alias = "return")]
        public DataType Return { get; set; }

        [YamlMember(Alias = "type")]
        public string Type { get; set; }
    }

    public partial class DataType
    {
        [YamlMember(Alias = "id")]
        public string Id { get; set; }

        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "descending")]
        public string Descending { get; set; }
    }

    public partial class Reference
    {
        [YamlMember(Alias = "uid")]
        public string Uid { get; set; }

        [YamlMember(Alias = "commentId")]
        public string CommentId { get; set; }

        [YamlMember(Alias = "fullName")]
        public string FullName { get; set; }

        [YamlMember(Alias = "isExternal")]
        public bool IsExternal { get; set; }

        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "nameWithType")]
        public string NameWithType { get; set; }

        [YamlMember(Alias = "parent")]
        public string Parent { get; set; }

        [YamlMember(Alias = "spec.crmscript", ScalarStyle = YamlDotNet.Core.ScalarStyle.Literal)]
        public SpecCrmscript[] SpecCrmscript { get; set; }
    }

    public partial class SpecCrmscript
    {
        [YamlMember(Alias = "uid")]
        public string Uid { get; set; }

        [YamlMember(Alias = "commentId")]
        public string CommentId { get; set; }

        [YamlMember(Alias = "isExternal")]
        public bool? IsExternal { get; set; }

        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "nameWithType")]
        public string NameWithType { get; set; }

        [YamlMember(Alias = "fullName")]
        public string FullName { get; set; }
    }
}
