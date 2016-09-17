#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/14/2016 4:09 AM
// Last Revised: 09/15/2016 1:03 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot {
    #region Using

    using System.Threading.Tasks;
    using APIs.Entities;
    using BCL;
    using Discord;
    using Discord.WebSocket;
    using TypeReaders;

    #endregion

    public class Program : BotBase {
        #region Public Methods

        public static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        #endregion Public Methods

        #region Overrides of BotBase

        public async override Task Start() {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Debug });
            Client.Ready += ClientOnReadyAsync;
            Commands = new CommandHandler();
            Commands.Service.AddTypeReader<HighScoreType>(new HighScoreTypeReader());
            await LoginAndConnect();
        }

        async Task ClientOnReadyAsync() {
            var self = Commands.Self;
            await self.ModifyStatusAsync(x => x.Game = new Game("Spying on the Draynor Bank"));
        }

        #endregion Overrides of BotBase
    }
}