// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 09/25/2016 6:53 AM
// Last Revised: 10/19/2016 6:17 PM
// Last Revised by: Alex Gravely

namespace WiseOldBot.Modules.GETracker
{
    using Discord.Commands;
    using Entities;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Name("GE-Tracker")]
    public class GETrackerModule : ModuleBase
    {
        [Command("alch"), Summary("Gets the alchemy values for an item"), Remarks("alch rune platebody")]
        public async Task GetAlchPriceAsync([Remainder, Summary("Item name")] string itemName = "cabbage")
        {
            using (Context.Channel.EnterTypingState())
            {
                var returnItem = GETrackerAPIClient.FindItemOrItems(itemName).FirstOrDefault();
                if (returnItem == null)
                {
                    await ReplyAsync($"Item not found.");
                    return;
                }
                await ReplyAsync($"`{returnItem.Name} / Low Alch: {returnItem.LowAlchemy} / High Alch: {returnItem.HighAlchemy}`");
            }
        }

        [Command("price"), Alias("p"), Summary("Gets the GE-Tracker info for an item"), Remarks("price rune platebody")]
        public async Task GetPriceAsync([Remainder, Summary("Item name")] string itemName = "cabbage")
        {
            using (Context.Channel.EnterTypingState())
            {
                var returnItems = GETrackerAPIClient.FindItemOrItems(itemName);
                ulong tomxGuildId = 271346318352449539;

                if (Context.Guild.Id == tomxGuildId && Context.Channel.Id != 275374396003319808)
                {
                    await ReplyAsync($"Please use the <#{275374396003319808}> channel for the price command.");
                    return;
                }
                if (Context.Guild.Id == 169578245837029376 && Context.Channel.Id != 181778450967822336)
                {
                    await ReplyAsync($"Please use the <#{181778450967822336}> channel for the price command.");
                }

                foreach (var item in returnItems.Take(5))
                {
                    if (item.CachedUntil <= DateTime.Now)
                    {
                        await item.UpdateAsync();
                    }
                }
                if (!returnItems.Any())
                {
                    await ReplyAsync("Item not found! If this is a new item, please run the `rebuild` command, then try again. If not, double check your spelling! :smiley_cat:");
                    return;
                }
                else if (returnItems.Count() == 1)
                {
                    await ReplyAsync("", embed: returnItems.FirstOrDefault().ToDiscordEmbed());
                    return;
                }
                await ReplyAsync(string.Join("\n", returnItems.Take(5).Select(x => x.ToDiscordMessage())));
            }
        }

        [Command("rebuild"), Summary("Rebuilds the item map, allowing new items to be added."), Remarks("rebuild")]
        public async Task RebuildItemMapAsync()
        {
            using (Context.Channel.EnterTypingState())
            {
                var inItems = (await GETrackerAPIClient.DownloadItemsAsync())["data"].GroupBy(x => x.Name).
                    ToDictionary(g => g.Key, g => g.OrderBy(x => x.ItemID).ToList());
                var newItems = inItems.Values.SelectMany(x => x).Except(GETrackerAPIClient.Items.Values.SelectMany(x => x)).GroupBy(x => x.Name).ToDictionary(g => g.Key, g => g.OrderBy(x => x.ItemID).ToList());
                var newItemCount = newItems.Count();
                foreach (var item in newItems)
                {
                    GETrackerAPIClient.Items.Add(item.Key, item.Value);
                }
                if (!newItems.Any())
                {
                    await ReplyAsync("Item map rebuilt. No new items.");
                    return;
                }
                await ReplyAsync($"Item map rebuilt! New items: {string.Join(", ", newItems.Select(x => x.Key.ToString()))}");
            }
        }
    }
}