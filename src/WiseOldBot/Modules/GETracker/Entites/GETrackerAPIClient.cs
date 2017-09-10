
// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 10/16/2016 9:47 PM
// Last Revised: 10/19/2016 12:21 AM
// Last Revised by: Alex Gravely

namespace WiseOldBot.Modules.GETracker.Entities {

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RestEase;

    public static class GETrackerAPIClient {

        public static ItemMap Items { get; set; }
        static readonly IGETrackerAPI API = RestClient.For<IGETrackerAPI>(BASE_URI);

        internal const string BASE_URI = "https://ge-tracker.com/api";

        static GETrackerAPIClient() {
            var items = API.DownloadItemsAsync().GetAwaiter().GetResult()["data"].GroupBy(x => x.Name.ToLower()).
                            ToDictionary(g => g.Key, g => g.OrderBy(x => x.ItemID).ToList());
            Items = new ItemMap(items);
        }

        public static async Task<GETrackerItem.Wrapper> DownloadItemAsync(int id) => await API.DownloadItemAsync(id);

        public static async Task<Dictionary<string, List<GETrackerItem>>> DownloadItemsAsync() => await API.DownloadItemsAsync();

        public static IEnumerable<GETrackerItem> FindItems(string itemName) => Items.FindItems(itemName);

        public static IEnumerable<GETrackerItem> FindItemOrItems(string itemName) => Items.FindItemOrItems(itemName);

        public static GETrackerItem GetItemOrDefault(string itemName) {
            List<GETrackerItem> item;
            if (Items.TryGetValue(itemName.ToLower(), out item)) {
                return item.FirstOrDefault();
            }
            return Items.FindItems(itemName).FirstOrDefault();
        }

    }
}