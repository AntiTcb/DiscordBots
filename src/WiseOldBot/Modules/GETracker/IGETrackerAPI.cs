#region Header
// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/27/2016 1:57 AM
// Last Revised: 09/27/2016 1:57 AM
// Last Revised by: Alex Gravely
#endregion
namespace WiseOldBot.GETracker {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RestEase;

    public interface IGETrackerAPI
    {
        #region Public Methods

        [Get("items/{itemId}")]
        Task<GETrackerItem.Wrapper> GetItemAsync([Path("itemId")] int itemId);

        [Get("items")]
        Task<Dictionary<string, List<GETrackerItem>>> GetItemsAsync();

        #endregion Public Methods
    }
}