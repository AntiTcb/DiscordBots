#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/15/2016 1:04 PM
// Last Revised: 09/15/2016 4:33 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.GETracker {
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using RestEase;

    #endregion

    [Module]
    public class GETrackerModule {
        #region Private Fields + Properties

        const string BASE_URI = "https://ge-tracker.com/STATS_API";
        static readonly IGETrackerAPI API = RestClient.For<IGETrackerAPI>(BASE_URI);
        ItemMap _itemMap;
        DiscordSocketClient _client;

        #endregion Private Fields + Properties

        #region Public Constructors

        public GETrackerModule(DiscordSocketClient client) {
            _client = client;
            var items = API.GetItemsAsync()
                .Result["data"]
                .GroupBy(x => x.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.OrderBy(x => x.ItemID).ToList());
            _itemMap = new ItemMap(items);
        }

        #endregion Public Constructors

        #region Public Methods

        [Command("price"), Alias("p"), Remarks("Gets the GE-Tracker Price Info for an item")]
        public async Task GetPriceAsync(IUserMessage msg, [Remainder, Summary("The item name")] string itemName) {
            await msg.Channel.TriggerTypingAsync();
            var returnItems = _itemMap.ContainsKey(itemName) ?
                _itemMap[itemName] : _itemMap.PartialMatch(itemName);
            var sb = new StringBuilder();

            foreach (var item in returnItems) {
                if (item.CachedUntil <= DateTime.Now) {
                    await item.UpdateAsync(API);
                }
                sb.AppendLine(item.ToDiscordMessage());
            }

            await msg.Channel.SendMessageAsync(sb.ToString());
        }

        [Command("rebuild"), Remarks("Rebuilds the item map.")]
        public async Task RebuildItemMapAsync(IUserMessage msg) {
            await msg.Channel.TriggerTypingAsync();
            var inItems = await API.GetItemsAsync();
            var a = inItems["data"].GroupBy(x => x.Name).
                               ToDictionary(g => g.Key, g => g.OrderBy(x => x.ItemID).ToList());
            var newItems = a.Where(x => _itemMap.ContainsKey(x.Key));
            _itemMap = new ItemMap(a);
            await msg.Channel.SendMessageAsync("Item map rebuilt! New items:" +
                $"{Format.Code($"{newItems.Select(x => x.Key.ToString()).Aggregate((x, y) => $"{x}, {y}" )}")}");
        }

        #endregion Public Methods
    }

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