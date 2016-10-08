#region Header

// Description:
// 
// Solution: DiscordBots
// Project: SelfBot
// 
// Created: 10/03/2016 3:09 PM
// Last Revised: 10/03/2016 3:20 PM
// Last Revised by: Alex Gravely

#endregion

namespace SelfBot {
    #region Using

    using System.Threading.Tasks;
    using System.Linq;
    using BCL;
    using BCL.Modules;
    using Discord;
    using Discord.WebSocket;

    #endregion

    public class Program : BotBase {
        #region Public Methods

        public static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        #endregion Public Methods

        #region Overrides of BotBase

        public async override Task HandleConfigsAsync() => Configs = await ConfigHandler.LoadAsync<SelfConfig>(CONFIG_PATH);

        public async override Task InstallCommandsAsync() {
            Commands = new SelfCommandHandler();
            Client.Log += Log;
            await Commands.Install(Client, Configs);
        }

        public async override Task StartAsync() {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Info });
            await HandleConfigsAsync();
            await InstallCommandsAsync();

            Client.MessageReceived += Commands.HandleCommand;
            await LoginAndConnectAsync(TokenType.User);
        }

        #endregion Overrides of BotBase
    }
}