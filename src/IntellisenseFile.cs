using System;
using System.Collections.Generic;
using System.Text;

namespace devnet.crmscript.intellisense.generator
{

    public class IntellisenseFile
    {
        public IntellisenseFile()
        {
            Root = new List<Intellisense>();
        }

        public List<Intellisense> Root { get; set; }
    }

    public class Intellisense
    {
        public string text { get; set; }
        public string help { get; set; }
    }

    class IntellisenseComparer : IComparer<Intellisense>
    {
        public int Compare(Intellisense x, Intellisense y)
        {
            if (x.text == y.text)
            {
                return 0;
            }

            // CompareTo() method
            return x.text.CompareTo(y.text);

        }
    }
}
