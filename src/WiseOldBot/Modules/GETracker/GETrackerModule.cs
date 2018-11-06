namespace WiseOldBot.Modules.GETracker
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Entities;
    using Humanizer;
    using Humanizer.Localisation;

    [Name("GE-Tracker")]
    public class GETrackerModule : ModuleBase<SocketCommandContext>
    {
        static ulong[] WhitelistedPriceRoleIds => new ulong[] { 179648866138718212, 179649074260082688, 270649339225964554, 302269308451422228, 445961451023106058 };

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

        [Command("price", RunMode = RunMode.Async), Alias("p"), Summary("Gets the GE-Tracker info for an item"), Remarks("price rune platebody")]
        public async Task GetPriceAsync([Remainder, Summary("Item name")] string itemName = "cabbage")
        {
            const ulong tomxGuildId = 271346318352449539;
            using (Context.Channel.EnterTypingState())
            {
                if (Context.Guild?.Id == tomxGuildId && Context.Channel.Id != 275374396003319808)
                {
                    await ReplyAsync($"Please use the <#{275374396003319808}> channel for the price command.");
                    return;
                }
                // GE-Tracker guild
                if (Context.Guild?.Id == 169578245837029376 && Context.Channel.Id != 181778450967822336 &&
                    (Context.User is IGuildUser gUser && !gUser.RoleIds.Intersect(WhitelistedPriceRoleIds).Any()))
                {
                    await ReplyAsync($"Please use the <#{181778450967822336}> channel for the price command.");
                    return;
                }

                var returnItems = GETrackerAPIClient.FindItemOrItems(itemName);

                foreach (var item in returnItems.Take(5))
                {
                    if (item.CachedUntil <= DateTime.Now || !item.ApproximateProfit.HasValue)
                        await item.UpdateAsync();
                }

                if (!returnItems.Any())
                {
                    await ReplyAsync("Item not found! If this is a new item, please run the `rebuild` command, then try again. If not, double check your spelling! :smiley_cat:");
                    return;
                }
                else if (returnItems.Count == 1)
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
                    ToDictionary(g => g.Key, g => g.OrderBy(x => x.ItemId).ToList());
                var newItems = inItems.Values.SelectMany(x => x)
                    .Except(GETrackerAPIClient.Items.Values.SelectMany(x => x))
                    .GroupBy(x => x.Name).ToDictionary(g => g.Key, g => g.OrderBy(x => x.ItemId).ToList());
                int newItemCount = newItems.Count;
                
                if (!newItems.Any())
                {
                    await ReplyAsync("Item map rebuilt. No new items.");
                    return;
                }

                foreach (var item in newItems)
                    GETrackerAPIClient.Items.Add(item.Key.ToLower(), item.Value);

                await ReplyAsync($"Item map rebuilt! New items: {string.Join(", ", newItems.Select(x => x.Key.ToString()))}");
            }
        }

        [Command("status"), Alias("osbstatus")]
        [Summary("Get the OSBuddy API status.")]
        [Remarks("status")]
        public async Task GetOsbStatusAsync()
        {
            using (Context.Channel.EnterTypingState())
            {
                var status = await GETrackerAPIClient.GetOsbStatusAsync();

                var eb = new EmbedBuilder
                {
                    Description = status.Data.Message.Humanize()
                };
                eb.AddField("Status", status.Data.Status.Humanize(), true);
                eb.AddField("Health", $"{status.Data.Health}%", true);
                eb.AddField("Update Frequency", TimeSpan.FromMinutes(status.Data.UpdateFrequency).Humanize(), true);

                await ReplyAsync("", embed: eb.Build());
            }
        }
    }
}