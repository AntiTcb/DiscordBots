#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 11:22 PM
// Last Revised: 09/14/2016 11:40 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Modules {
    #region Using

    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;

    #endregion

    [Module(AutoLoad = false)]
    public class PublicModule {
        #region Private Fields + Properties

        readonly DiscordSocketClient _client;

        #endregion Private Fields + Properties

        #region Public Constructors

        public PublicModule(DiscordSocketClient client) {
            _client = client;
        }

        #endregion Public Constructors

        #region Public Methods

        [Command("info")]
        public async virtual Task Info(IUserMessage msg) {
            var app = await _client.GetApplicationInfoAsync();
            await
                msg.Channel.SendMessageAsync
                    ($"{Format.Bold("Info")}\n" + $"- Author: {app.Owner.Username} (ID {app.Owner.Id})\n" +
                     $"- Repo: <https://github.com/AntiTcb/DiscordBots/{app.Name}>\n" +
                     $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                     $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}\n" +
                     $"- Uptime: {GetUptime()}\n\n" + $"{Format.Bold("Stats")}\n" + $"- Heap Size: {GetHeapSize()} MB\n" +
                     $"- Guilds: {(await _client.GetGuildSummariesAsync()).Count}\n" +
                     $"- Channels: {(await _client.GetGuildsAsync()).Select(async g => await g.GetChannelsAsync()).Count()}" +
                     $"- Users: {(await _client.GetGuildsAsync()).Select(async g => await g.GetUsersAsync()).Count()}");
        }

        [Command("join"), Alias("invite"), Remarks("Returns the Invite URL of the bot.")]
        public async Task Join(IUserMessage msg) {
            var app = await _client.GetApplicationInfoAsync();
            await
                msg.Channel.SendMessageAsync($"<https://discordapp.com/oauth2/authorize?client_id={app.Id}&scope=bot>");
        }

        #endregion Public Methods

        #region Private Methods

        static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();

        static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");

        #endregion Private Methods
    }
}