#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/27/2016 1:57 AM
// Last Revised: 10/18/2016 10:04 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.Modules.GETracker.Entities {
    #region Using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RestEase;

    #endregion

    public interface IGETrackerAPI {
        #region Public Methods

        [Get("items/{itemId}")]
        Task<GETrackerItem.Wrapper> DownloadItemAsync([Path("itemId")] int itemId);

        [Get("items")]
        Task<Dictionary<string, List<GETrackerItem>>> DownloadItemsAsync();

        #endregion Public Methods
    }
}