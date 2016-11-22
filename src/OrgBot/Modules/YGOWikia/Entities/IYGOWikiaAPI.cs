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

namespace OrgBot.Modules.YGOWikia.Entities {

    using RestEase;
    using System.Threading.Tasks;

    public interface IYGOWikiaAPI {

        #region Public Methods

        [Get("Search/List")]
        Task<YGOWikiaSearchListResponse> GetUrlsAsync([Query] string query);

        #endregion Public Methods
    }
}