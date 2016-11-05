#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/15/2016 11:36 PM
// Last Revised: 10/15/2016 11:36 PM
// Last Revised by: Alex Gravely
#endregion
namespace BCL.Modules {
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;

    public class PublicModule : ModuleBase {
        [Command("info")]
        public async virtual Task InfoAsync() {
            // TODO: Async Sum
            var app = await Context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
            var completedChannelCount =
                await
                    Task.WhenAll((await Context.Client.GetGuildsAsync()).Select(async g => await g.GetChannelsAsync()));
            var completedUserCount =
                await Task.WhenAll((await Context.Client.GetGuildsAsync()).Select(async g => await g.GetUsersAsync()));
            await
                ReplyAsync
                    ($"{Format.Bold("Info")}\n" + $"- Author: {app.Owner.Username} (ID {app.Owner.Id})\n" +
                     $"- Repo: <https://github.com/AntiTcb/DiscordBots/{app.Name}>\n" +
                     $"- Assembly: {Assembly.GetEntryAssembly().GetName()} {Assembly.GetEntryAssembly().GetName().Version}\n" +
                     $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                     $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}\n" +
                     $"- Uptime: {GetUptime()}\n\n" + $"{Format.Bold("Stats")}\n" + $"- Heap Size: {GetHeapSize()} MB\n" +
                     $"- Guilds: {(await Context.Client.GetGuildsAsync().ConfigureAwait(false)).Count}\n" +
                     $"- Channels: {completedChannelCount.Sum(c => c.Count)} " +
                     $"- Users: {completedUserCount.Sum(u => u.Count)}").ConfigureAwait(false);
        }

        [Command("join"), Alias("invite"), Remarks("Returns the Invite URL of the bot")]
        public async virtual Task JoinAsync() {
            var app = await Context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
            await ReplyAsync($"<https://discordapp.com/oauth2/authorize?permissions=67496960&client_id={app.Id}&scope=bot>").ConfigureAwait(false);
        }

        static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString(CultureInfo.InvariantCulture);
        static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
    }
}