#region Header

// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/30/2016 1:07 PM
// Last Revised: 11/04/2016 12:47 AM
// Last Revised by: Alex Gravely

#endregion Header

namespace OrgBot.Modules.YGOWikia.Entities {

    #region Using

    using AngleSharp;
    using RestEase;
    using System;
    using System.Net;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Text.RegularExpressions;

    #endregion Using

    public static class YGOWikiaClient {

        #region Private Fields + Properties

        const string BASE_URI = "http://yugioh.wikia.com/api/v1";
        static readonly IYGOWikiaAPI API = RestClient.For<IYGOWikiaAPI>(BASE_URI);
        static readonly Regex URL_EXCLUSION_REGEX = new Regex(@"\((anime|BAM)\)", RegexOptions.IgnoreCase);

        #endregion Private Fields + Properties

        #region Public Methods

        public static async Task<YGOWikiaCard> GetCardAsync(string cardName) {
            try {
                var results = (await API.GetUrlsAsync(cardName))?.Items.Where(x => !URL_EXCLUSION_REGEX.IsMatch(x.URL));
                return await ParsePageHTMLAsync(results?.ElementAtOrDefault(0)?.URL);
            }
            catch (ApiException e) when (e.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }
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
            var ceffect = Regex.Replace(doc.QuerySelectorAll(".navbox-list").Select(x => x.InnerHtml).ElementAtOrDefault(0), "<br>", "\n");
            var cardeffect = Regex.Replace(Regex.Replace(ceffect, "(<(.*?)>)+?", ""), @"^ *(Monster Effect|Pendulum Effect)\s*\n\s*(?=\w)", "**$1:**\n", RegexOptions.Multiline);
            return YGOWikiaCard.Parse(cardAttrs, cardeffect, url);
        }

        #endregion Private Methods
    }
}