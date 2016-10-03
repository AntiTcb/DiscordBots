using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SelfBot {
    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;

    public class Program : BotBase {
        public static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        #region Overrides of BotBase

        public async override Task StartAsync() {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Debug });
            HandleConfigs();
            await InstallCommandsAsync();
            await LoginAndConnectAsync();
        }

        public override void HandleConfigs() => Configs = ConfigHandler.Load<SelfConfig>(CONFIG_PATH);

        public async override Task InstallCommandsAsync() {
            Commands = new CommandHandler();
            var map = new DependencyMap();
            map.Add(Configs);
            await Commands.Install(Client, Configs, map);
        }

        #endregion
    }
}
