#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 10/18/2016 4:17 PM
// Last Revised: 10/23/2016 6:51 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot {
    #region Using

    using System.Threading.Tasks;
    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Modules.OSRS;
    using OSRS.TypeReaders;

    #endregion

    public class WiseOldBot : BotBase {
        #region Overrides of BotBase

        public async override Task InstallCommandsAsync() {
            Commands = new CommandHandler();
            Client.Log += Log;

            var map = new DependencyMap();
            map.Add(Client);
            await Commands.InstallAsync(map);
        }

        public async override Task StartAsync<T>() {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Debug });
            Client.Ready += ClientOnReadyAsync;
            Client.GuildAvailable += ClientOnGuildAvailableAsync;
            Globals.EvalImports.AddRange(new[] {"WiseOldBot", "WiseOldBot.Modules.GETracker"});
            await HandleConfigsAsync<T>();
            await InstallCommandsAsync();
            await LoginAndConnectAsync(TokenType.Bot);
        }

        async Task ClientOnGuildAvailableAsync(SocketGuild socketGuild) {
            ServerConfig outValue;
            if (!Globals.ServerConfigs.TryGetValue(socketGuild.Id, out outValue)) {
                var defChannel = await socketGuild.GetDefaultChannelAsync();
                await defChannel.SendMessageAsync("Server config file not found! Generating one now!");
                Globals.ServerConfigs.Add(socketGuild.Id, new ServerConfig { CommandPrefix = Globals.DEFAULT_PREFIX });
                await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs);
            }
        }

        async Task ClientOnReadyAsync() => await Client.SetGame("Spying on the Draynor Bank");

        #endregion Overrides of BotBase
    }
}