#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/25/2016 6:53 AM
// Last Revised: 10/19/2016 6:17 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.GETracker {
    #region Using

    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;

    #endregion

    public class GETrackerModule : ModuleBase {
        #region Public Methods

        [Command("alch")]
        public async Task GetAlchPriceAsync([Remainder] string itemName = "cabbage") {
            await Context.Channel.TriggerTypingAsync();
            var returnItems = GETrackerAPIClient.Items.ContainsKey(itemName)
                                  ? GETrackerAPIClient.Items[itemName] : GETrackerAPIClient.Items.PartialMatch(itemName);
            var sb = new StringBuilder();

            foreach (var item in returnItems.Take(5)) {
                sb.AppendLine($"`{item.Name} / Low Alch: {item.LowAlchemy} / {item.HighAlchemy}`");
            }
            await Context.Channel.SendMessageAsync(sb.ToString());
        }

        [Command("price"), Alias("p"), Remarks("Gets the GE-Tracker Price Info for an item")]
        public async Task GetPriceAsync([Remainder, Summary("The item name")] string itemName = "cabbage") {
            await Context.Channel.TriggerTypingAsync();
            var returnItems = GETrackerAPIClient.Items.ContainsKey(itemName)
                                  ? GETrackerAPIClient.Items[itemName] : GETrackerAPIClient.Items.PartialMatch(itemName);
            var sb = new StringBuilder();

            foreach (var item in returnItems.Take(5)) {
                if (item.CachedUntil <= DateTime.Now) {
                    await item.UpdateAsync();
                }
                sb.AppendLine(item.ToDiscordMessage());
            }

            await Context.Channel.SendMessageAsync(sb.ToString());
        }

        [Command("rebuild"), Remarks("Rebuilds the item map.")]
        public async Task RebuildItemMapAsync() {
            await Context.Channel.TriggerTypingAsync();
            var inItems = await GETrackerAPIClient.GetItemsAsync();
            var a = inItems["data"].GroupBy(x => x.Name).
                                    ToDictionary(g => g.Key, g => g.OrderBy(x => x.ItemID).ToList());
            var newItems = a.Where(x => GETrackerAPIClient.Items.ContainsKey(x.Key));
            GETrackerAPIClient.Items = new ItemMap(a);
            await
                Context.Channel.SendMessageAsync
                        ("Item map rebuilt! New items:" +
                         $"{Format.Code($"{newItems.Select(x => x.Key.ToString()).Aggregate((x, y) => $"{x}, {y}")}")}");
        }

        #endregion Public Methods
    }
}