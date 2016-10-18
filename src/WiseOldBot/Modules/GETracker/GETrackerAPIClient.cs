#region Header
// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 10/16/2016 9:47 PM
// Last Revised: 10/16/2016 9:47 PM
// Last Revised by: Alex Gravely
#endregion
namespace WiseOldBot.GETracker {
    using RestEase;
    using System.Linq;

    public static class GETrackerAPIClient {
        const string BASE_URI = "https://ge-tracker.com/api";
        public static readonly IGETrackerAPI API = RestClient.For<IGETrackerAPI>(BASE_URI);
        public static ItemMap ItemMap { get; set; }

        static GETrackerAPIClient() {
            var items = API.GetItemsAsync()
                .Result["data"]
                .GroupBy(x => x.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.OrderBy(x => x.ItemID).ToList());
            ItemMap = new ItemMap(items);
        }
    }
}