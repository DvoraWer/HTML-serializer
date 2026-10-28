using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            q.Enqueue(this);
            while(q.Count > 0)
            {
                var obj= q.Dequeue();
                yield return obj;
                foreach( var child in obj.Children )
                {
                    q.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement root = this.Parent;
            while( root != null )
            {
                yield return root;
                root= root.Parent;
            }
        }

        public bool IsMatch(Selector selector)
        {
            if (!string.IsNullOrEmpty(selector.TagName) && selector.TagName != Name)
                return false;

            if (!string.IsNullOrEmpty(selector.Id) && selector.Id != Id)
                return false;

            foreach (var cls in selector.Classes)
            {
                if (!Classes.Contains(cls))
                    return false;
            }

            return true;
        }
    }
}
