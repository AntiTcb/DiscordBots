#region Header
// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/29/2016 1:24 PM
// Last Revised: 10/29/2016 1:24 PM
// Last Revised by: Alex Gravely
#endregion
namespace OrgBot.Modules.YGOCard.Entities {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RestEase;
    using YGOWikia.Entities;

    public interface IYGOWikiaAPI {
        [Get("Search/List")]
        Task<YGOWikiaSearchListResponse> GetUrlsAsync([Query] string query);
    }
}