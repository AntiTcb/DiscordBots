#region Header

// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/29/2016 1:24 PM
// Last Revised: 10/29/2016 1:24 PM
// Last Revised by: Alex Gravely

#endregion Header

namespace OrgBot.Modules.YGOCard.Entities
{
    using AngleSharp;
    using AngleSharp.Parser.Html;
    using RestEase;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using YGOWikia.Entities;

    public static class YGOWikiaClient
    {
        #region Private Fields + Properties

        const string BASE_URI = "http://yugioh.wikia.com/api/v1";
        static readonly IYGOWikiaAPI API = RestClient.For<IYGOWikiaAPI>(BASE_URI);

        #endregion Private Fields + Properties

        #region Public Methods

        public static async Task<YGOWikiaCard> GetCardAsync(string cardName) {
            //var result = await API.GetUrlsAsync(cardName);
            var results = (await API.GetUrlsAsync(cardName)).Items.Where(x => !x.URL.Contains("(anime)") || !x.URL.Contains("(ANIME)"));
            return await ParsePageHTMLAsync(results.ElementAtOrDefault(0)?.URL);
        }

        #endregion Public Methods

        #region Private Methods

        static async Task<YGOWikiaCard> ParsePageHTMLAsync(string url) {
            if (url == null) {
                return null;
            }
            var parser = new HtmlParser();
            var doc = await BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(url);
            var cardAttrs = doc.QuerySelectorAll("tr.cardtablerow").ToLookup(x => x.TextContent.Split('\n')[0], x => x.TextContent.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(1));
            var cardEffects = doc.QuerySelectorAll(".navbox-list").Select(x => x.TextContent);
            return YGOWikiaCard.Parse(cardAttrs, cardEffects.ElementAtOrDefault(0));
        }

        #endregion Private Methods
    }
}