using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace Html_Serializer
{
    public class HtmlHelper
    {
        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }
        private static readonly HtmlHelper Helper= new HtmlHelper();
        public static HtmlHelper Instance { get; }=new HtmlHelper();
        private HtmlHelper()
        {
            string allTagsJson = File.ReadAllText("HtmlTags.json");
            AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
            string selfClosingJson = File.ReadAllText("HtmlVoidTags.json");
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingJson);
        }

    }
}
