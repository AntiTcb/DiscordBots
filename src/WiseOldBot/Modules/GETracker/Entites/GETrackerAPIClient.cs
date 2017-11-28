namespace WiseOldBot.Modules.GETracker.Entities
{
    using RestEase;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BCL;

    public static class GETrackerAPIClient
    {
        public static ItemMap Items { get; set; }
        internal const string BASE_URI = "https://www.ge-tracker.com/api";
        static readonly IGETrackerAPI API = RestClient.For<IGETrackerAPI>(BASE_URI);

        static GETrackerAPIClient()
        {
            API.Token = $"Bearer {((WiseOldBotConfig)Globals.BotConfig).GETrackerToken}";
            var items = API.DownloadItemsAsync().GetAwaiter().GetResult()["data"].GroupBy(x => x.Name.ToLower()).
                            ToDictionary(g => g.Key, g => g.OrderBy(x => x.ItemId).ToList());
            Items = new ItemMap(items);
        }

        public static async Task<GETrackerItem.Wrapper> DownloadItemAsync(int id) => await API.DownloadItemAsync(id);

        public static async Task<Dictionary<string, List<GETrackerItem>>> DownloadItemsAsync() => await API.DownloadItemsAsync();

        public static IEnumerable<GETrackerItem> FindItemOrItems(string itemName) => Items.FindItemOrItems(itemName);

        public static IEnumerable<GETrackerItem> FindItems(string itemName) => Items.FindItems(itemName);

        public static GETrackerItem GetItemOrDefault(string itemName)
        {
            if (Items.TryGetValue(itemName.ToLower(), out var item))
                return item.FirstOrDefault();

            return Items.FindItems(itemName).FirstOrDefault();
        }
    }
}