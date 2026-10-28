
using Html_Serializer;
using System.Text.RegularExpressions;

var html = await Load("");
var cleanHtml = new Regex("\\s+").Replace(html, " ");
var htmlLines = new Regex("<(.*?)>")
    .Split(cleanHtml)
    .Select(s => s.Trim())
    .Where(s => !string.IsNullOrWhiteSpace(s));
var htmlElement = "<div id=\"my-id\" class=\"my-class-1 my-class-2\" width=\"100%\">text</div>";
var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

static HtmlElement BuildTree(IEnumerable<string> htmlString)
{
    HtmlElement root = new HtmlElement { Name = "html" };
    HtmlElement current = root;
    foreach (var line in htmlString)
    {
        var parts = line.Split(' ', 2);
        var firstWord = parts[0].TrimEnd('/');
        if (firstWord == "/html")
            break;
        else if (firstWord.StartsWith('/'))
        {
            if (current.Parent != null)
                current = current.Parent;
        }
        else if (firstWord.StartsWith('!') || firstWord == "html")
            continue;
        else if (HtmlHelper.Instance.AllTags.Contains(firstWord))
        {
            HtmlElement newTag = new HtmlElement { Name = firstWord, Parent = current };
            current.Children.Add(newTag);
            if (parts.Length > 1)
            {
                var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(parts[1]);
                foreach (Match attribute in attributes)
                {
                    var name = attribute.Groups[1].Value;
                    var value = attribute.Groups[2].Value;
                    newTag.Attributes.Add(attribute.Value);
                    if (name == "id")
                        newTag.Id = value;
                    else if (name == "class")
                        newTag.Classes.AddRange(value.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                }
            }
            bool isSelfClosing = line.EndsWith("/") || HtmlHelper.Instance.SelfClosingTags.Contains(firstWord);
            if (!isSelfClosing)
                current = newTag;
        }
        else
        {
            current.InnerHtml += line;
        }
    }

    return root;
}
