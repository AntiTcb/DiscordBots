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
    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using OSRS;
    using TypeReaders;

    #endregion

    public class Program : BotBase {
        #region Public Methods

        public static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        #endregion Public Methods

        #region Overrides of BotBase

        public async override Task StartAsync() {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Debug });
            Client.Ready += ClientOnReadyAsync;
            HandleConfigs();
            await InstallCommandsAsync();
            await LoginAndConnectAsync();
        }

        public override void HandleConfigs() => Configs = ConfigHandler.Load<WiseOldBotConfig>(CONFIG_PATH);

        public async override Task InstallCommandsAsync() {
            Commands = new CommandHandler();
            Commands.Service.AddTypeReader<HighScoreType>(new HighScoreTypeReader());
            Commands.Service.AddTypeReader<SkillType>(new SkillTypeReader());
            Client.Log += Log;
            var map = new DependencyMap();
            map.Add(Configs);
            await Commands.Install(Client, map);
        }

        async Task ClientOnReadyAsync() => await Commands.Self.ModifyStatusAsync(x => x.Game = new Game("Spying on the Draynor Bank"));

        #endregion Overrides of BotBase
    }
}