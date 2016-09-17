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
    using Preconditions;

    [Module]
    public class OwnerModule {
        IApplication _app;
        DiscordSocketClient _client;
        public OwnerModule(DiscordSocketClient client, IApplication app) {
            _client = client;
            _app = app;
        }

        [Command("shutdown")]
        public async Task ShutDown(IUserMessage msg) {
            if (msg.Author.Id != _app.Owner.Id) { return; }
            await msg.Channel.SendMessageAsync("Shutting down...");
            await _client.DisconnectAsync();
        }
    }
}