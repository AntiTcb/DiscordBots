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

namespace WiseOldBot.Modules {
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using APIs;
    using APIs.Entities;
    using Discord;
    using Discord.Commands;
    using RestEase;

    #endregion

    [Module]
    public class GETrackerModule {
        #region Private Fields + Properties

        const string BASE_URI = "https://ge-tracker.com/STATS_API";
        static readonly IGETrackerAPI API = RestClient.For<IGETrackerAPI>(BASE_URI);
        Dictionary<string, IEnumerable<int>> _itemMap;

        #endregion Private Fields + Properties

        #region Public Constructors

        public GETrackerModule() {
            _itemMap = API.GetItemsAsync().
                           Result.GroupBy(x => x.Name).
                           ToDictionary(g => g.Key, g => g.Select(x => x.ItemID).AsEnumerable());
        }

        #endregion Public Constructors

        #region Public Methods

        [Command("price"), Alias("p"), Remarks("Gets the GE-Tracker Price Info for an item")]
        public async Task GetPriceAsync(IUserMessage msg, [Remainder, Summary("The item name")] string itemName) {
            var tasks = _itemMap[itemName].Select(async x => await API.GetItemAsync(x));
            var jointTask = Task.WhenAll(tasks);
            var items = await jointTask as IEnumerable<GETrackerItem>;

            var sb = new StringBuilder();
            foreach (var item in items) {
                sb.AppendLine(item.ToDiscordMessage());
            }

            await msg.Channel.SendMessageAsync(sb.ToString());
        }

        [Command("rebuild"), Remarks("Rebuilds the item map.")]
        public async Task RebuildItemMapAsync(IUserMessage msg) {
            var inItems = await API.GetItemsAsync();
            _itemMap = inItems.GroupBy(x => x.Name).
                               ToDictionary(g => g.Key, g => g.Select(x => x.ItemID).AsEnumerable());
        }

        #endregion Public Methods
    }
}