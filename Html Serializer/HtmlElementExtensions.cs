using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Html_Serializer
{
    public static class HtmlElementExtensions
    {
        public static IEnumerable<HtmlElement> ExtensionQuery(this HtmlElement element,Selector s)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            RecorsiveSearch(result, element, s);
            return result;
        }

        private static void RecorsiveSearch(HashSet<HtmlElement>hash,HtmlElement element,Selector s)
        {
            
            var matches = element.Descendants().Skip(1).Where(e => e.IsMatch(s));
            if (s.Child == null)
            {
                hash.UnionWith(matches);
                return;
            }
            else
            {
                foreach(var child in matches)
                {
                    RecorsiveSearch(hash, child, s.Child);
                }
            }
        }
    }
}
