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

    using System.Reflection;
    using System.Threading.Tasks;
    using BCL;
    using BCL.Modules;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using OSRS;
    using TypeReaders;
    using Game = Discord.API.Game;

    #endregion

    public class Program : BotBase {
        #region Public Methods

        public static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        #endregion Public Methods

        #region Overrides of BotBase

        public async override Task StartAsync() {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Debug });
            Client.Ready += ClientOnReadyAsync;
            await HandleConfigsAsync();
            await InstallCommandsAsync();
            await LoginAndConnectAsync(TokenType.Bot);
        }

        public async override Task InstallCommandsAsync() {
            Commands = new CommandHandler();
            Commands.Service.AddTypeReader<HighScoreType>(new HighScoreTypeReader());
            Commands.Service.AddTypeReader<SkillType>(new SkillTypeReader());
            Client.Log += Log;

            var map = new DependencyMap();
            map.Add(Client);
            await Commands.InstallAsync(map);
        }

        async Task ClientOnReadyAsync() => await Client.CurrentUser.ModifyStatusAsync(
            x => x.Game = new Optional<Game>(new Game {Name = "Spying on the Draynor Bank"}));

        #endregion Overrides of BotBase
    }
}