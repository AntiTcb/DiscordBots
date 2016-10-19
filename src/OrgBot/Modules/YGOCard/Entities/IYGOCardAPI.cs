#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/18/2016 10:01 PM
// Last Revised: 10/18/2016 10:04 PM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOCard.Entities {
    #region Using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RestEase;

    #endregion

    public interface IYGOCardAPI {
        #region Public Methods

        [Get("cards/{id}/?media=json")]
        Task<YGOCard> GetCardAsync([Path("id")] uint cardId);

        [Get("cards/?media=json&$size=10000")]
        Task<List<YGOCard>> GetCardsAsync();

        #endregion Public Methods
    }
}