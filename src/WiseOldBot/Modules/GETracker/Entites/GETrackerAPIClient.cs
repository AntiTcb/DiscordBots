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

        public static Task<GETrackerItem.Wrapper> DownloadItemAsync(int id) 
            => API.DownloadItemAsync(id);

        public static Task<Dictionary<string, List<GETrackerItem>>> DownloadItemsAsync() 
            => API.DownloadItemsAsync();

        public static IReadOnlyCollection<GETrackerItem> FindItemOrItems(string itemName) 
            => Items.FindItemOrItems(itemName);

        public static IReadOnlyCollection<GETrackerItem> FindItems(string itemName) 
            => Items.FindItems(itemName);

        public static GETrackerItem GetItemOrDefault(string itemName)
        {
            if (Items.TryGetValue(itemName.ToLower(), out var item))
                return item.FirstOrDefault();

            return Items.FindItems(itemName).FirstOrDefault();
        }

        public static Task<OsbStatus.Wrapper> GetOsbStatusAsync() 
            => API.GetOsbStatusAsync();
    }
}