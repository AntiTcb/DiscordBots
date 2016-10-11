#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 11:50 PM
// Last Revised: 09/14/2016 11:50 PM
// Last Revised by: Alex Gravely
#endregion
namespace BCL.Modules {
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Interfaces;


    // TODO: Complete Refactor
    [Module("owner", AutoLoad = false)]
    public class OwnerModule {
        IApplication _app;
        DiscordSocketClient _client;
        IBotConfig _botConfig;
        public OwnerModule(DiscordSocketClient client, IBotConfig botConfig, IApplication app) {
            _client = client;
            _app = app;
            _botConfig = botConfig;
        }

        [Command("setlogchan")]
        public async Task SetLogChannelAsync(IUserMessage msg, ulong chanID) {
            if (msg.Author.Id != _app.Owner.Id) {
                await msg.Channel.SendMessageAsync("Insufficient permissions.");
                return;
            }
            _botConfig.LogChannel = chanID;
            ConfigHandler.SaveAsync("config.json", _botConfig);
        }

        [Command("shutdown")]
        public async Task ShutDown(IUserMessage msg) {
            if (msg.Author.Id != _app.Owner.Id) {
                await msg.Channel.SendMessageAsync("Insufficient permissions.");
                return;
            }
            await msg.Channel.SendMessageAsync("Shutting down...");
            await _client.DisconnectAsync();
        }
    }
}