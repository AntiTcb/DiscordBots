#region Header
// Description:
// 
// Solution: DiscordBots
// Project: SelfBot
// 
// Created: 10/30/2016 5:45 PM
// Last Revised: 10/30/2016 5:45 PM
// Last Revised by: Alex Gravely
#endregion
namespace SelfBot {
    using System.Threading.Tasks;
    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;

    public class Bot : BotBase{
        #region Overrides of BotBase

        public async override Task InstallCommandsAsync() {
            Commands = new SelfCommandHandler();
            Client.Log += Log;
            var map = new DependencyMap();
            map.Add(Client);
            await Commands.InstallAsync(map);
        }

        public async override Task HandleConfigsAsync<T>() => Globals.BotConfig = await ConfigHandler.LoadBotConfigAsync<T>().ConfigureAwait(false);

        public async override Task StartAsync<T>() {
            Client = new DiscordSocketClient(new DiscordSocketConfig {LogLevel = LogSeverity.Info});
            await HandleConfigsAsync<T>();
            await InstallCommandsAsync();
            await LoginAndConnectAsync(TokenType.User);
        } 

        #endregion
    }
}