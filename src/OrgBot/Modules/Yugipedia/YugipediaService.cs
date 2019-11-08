using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WikiClientLibrary.Pages;
using WikiClientLibrary.Sites;

namespace OrgBot
{
    public class YugipediaService
    {
        public WikiSite Site { get;set; }

        private static readonly Regex _cardTableParser = new Regex(@"\|\s([\w|_]+)\s*=\s(.*)", RegexOptions.Compiled);

        public YugipediaService(WikiSite site)
        {
            Site = site;
        }
        
        public async Task<YugipediaCard> GetCardAsync(string cardName)
        {
            var page = new WikiPage(Site, cardName);
            await page.RefreshAsync(PageQueryOptions.FetchContent | PageQueryOptions.ResolveRedirects);

            if (string.IsNullOrEmpty(page.Content) || page.NamespaceId != 0 || !page.Content.Contains("{{CardTable2")) return null;

            var propDict = new Dictionary<string, string>
            {
                { "en_name", page.Title }
            };

            var props = _cardTableParser.Matches(page.Content)
                .OfType<Match>()
                .Select(m => (m.Groups[1].Value, m.Groups[2].Value));

            foreach (var (key, value) in props)
                propDict[key] = value;

            // TODO: This is so hackily bad.
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var card = JsonConvert.DeserializeObject<YugipediaCard>(JsonConvert.SerializeObject(propDict));

            return card;
        }
    }
}
