#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/18/2016 10:57 PM
// Last Revised: 10/19/2016 12:00 AM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOCard.Entities {
    #region Using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RestEase;

    #endregion

    public static class YGOCardAPIClient {
        #region Public Fields + Properties

        public static CardMap Cards { get; set; }
        static readonly IYGOCardAPI API = RestClient.For<IYGOCardAPI>(BASE_URI);

        #endregion Public Fields + Properties

        #region Private Fields + Properties

        const string BASE_URI = "https://dncardsapi.apispark.net/v2";

        #endregion Private Fields + Properties

        #region Public Constructors

        static YGOCardAPIClient() {
            var items = API.GetCardsAsync().Result;
            Cards = new CardMap(items);
        }

        public static async Task<YGOCard> GetCardAsync(uint id) => await API.GetCardAsync(id);
        public static async Task<List<YGOCard>> GetCardsAsync() => await API.GetCardsAsync();

        #endregion Public Constructors
    }
}