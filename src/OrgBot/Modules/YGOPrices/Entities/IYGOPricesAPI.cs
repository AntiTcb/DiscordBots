#region Header
// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 11/04/2016 5:30 PM
// Last Revised: 11/04/2016 5:30 PM
// Last Revised by: Alex Gravely
#endregion
namespace OrgBot.Modules.YGOPrices.Entities {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RestEase;

    public interface IYGOPricesAPI {
        [Get("card_sets")]
        Task<List<string>> GetSetNamesAsync();

        [Get("get_card_prices/{cardName}")]
        Task<CardPriceResponse> GetCardPriceAsync([Path]string cardName);

        [Get("card_data/{cardName}")]
        Task<CardDataResponse> GetCardDataAsync([Path]string cardName);
    }
}