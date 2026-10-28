using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector Parse(string query)
        {
            var levels = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Selector root= new Selector();
            Selector current = root;
            foreach(var level in levels)
            {
                var parts = Regex.Split(level, "(?=[#.])");
                foreach (var part in parts)
                {
                    if (string.IsNullOrEmpty(part))
                        continue;
                    if (part.StartsWith("#"))
                        current.Id = part.Substring(1);
                    else if (part.StartsWith("."))
                        current.Classes.Add(part.Substring(1));
                    else if (HtmlHelper.Instance.AllTags.Contains(part))
                        current.TagName = part;
                }
                Selector newSel= new Selector();
                newSel.Parent= current;
                current.Child= newSel;
                current = newSel;
            }
            current.Parent.Child = null;
            return root;
        }
    }
}
