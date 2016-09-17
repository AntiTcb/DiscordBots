#region Header
// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/15/2016 2:09 PM
// Last Revised: 09/15/2016 2:09 PM
// Last Revised by: Alex Gravely
#endregion
namespace WiseOldBot.APIs {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;
    using RestEase;

    public interface IGETrackerAPI {
        [Get("items/{itemId}")]
        Task<GETrackerItem> GetItemAsync(int itemId);
        [Get("items")]
        Task<List<GETrackerItem>> GetItemsAsync();
    }
}