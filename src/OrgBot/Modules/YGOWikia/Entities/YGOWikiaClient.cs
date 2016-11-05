#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/30/2016 1:07 PM
// Last Revised: 11/04/2016 12:47 AM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOCard.Entities {
    #region Using

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AngleSharp;
    using AngleSharp.Parser.Html;
    using RestEase;
    using YGOWikia.Entities;

    #endregion

    public static class YGOWikiaClient {
        #region Private Fields + Properties

        const string BASE_URI = "http://yugioh.wikia.com/api/v1";
        static readonly IYGOWikiaAPI API = RestClient.For<IYGOWikiaAPI>(BASE_URI);

        #endregion Private Fields + Properties

        #region Public Methods

        public static async Task<YGOWikiaCard> GetCardAsync(string cardName) {
            var results = (await API.GetUrlsAsync(cardName))?.Items.Where
                                                              (x =>
                                                                   !x.URL.Contains("(anime)") ||
                                                                   !x.URL.Contains("(ANIME)"));
            return await ParsePageHTMLAsync(results?.ElementAtOrDefault(0)?.URL);
        }

        #endregion Public Methods

        #region Private Methods

        static async Task<YGOWikiaCard> ParsePageHTMLAsync(string url) {
            if (url == null) {
                return null;
            }
            var doc = await BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(url);
            var cardAttrs = doc.QuerySelectorAll("tr.cardtablerow").
                                ToLookup
                                (x => x.TextContent.Split('\n')[0],
                                 x =>
                                     x.TextContent.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).
                                       ElementAtOrDefault(1));
            var cardEffects = doc.QuerySelectorAll(".navbox-list").Select(x => x.TextContent);
            return YGOWikiaCard.Parse(cardAttrs, cardEffects.ElementAtOrDefault(0), url);
        }

        #endregion Private Methods
    }
}