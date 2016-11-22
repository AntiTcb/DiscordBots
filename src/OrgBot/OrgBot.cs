#region Header

// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/18/2016 4:22 PM
// Last Revised: 11/08/2016 11:11 PM
// Last Revised by: Alex Gravely

#endregion Header

namespace OrgBot {

    #region Using

    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using System.Threading.Tasks;

    #endregion Using

    public class OrgBot : BotBase {

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
            Client.JoinedGuild += ClientOnJoinedGuildAsync;
            Client.LeftGuild += ClientOnLeftGuildAsync;
            Globals.EvalImports.AddRange(new[] { "OrgBot",
                "OrgBot.Modules", "OrgBot.Modules.YGOCard", "OrgBot.Modules.YGOCard.Entities",
                "OrgBot.Modules.YGOWikia", "OrgBot.Modules.YGOWikia.Entities" });
            await HandleConfigsAsync<T>();
            await InstallCommandsAsync();
            await LoginAndConnectAsync(TokenType.Bot);
        }

        async Task ClientOnGuildAvailableAsync(SocketGuild socketGuild) {
            ServerConfig outValue;
            if (!Globals.ServerConfigs.TryGetValue(socketGuild.Id, out outValue)) {
                var defChannel = await socketGuild.GetDefaultChannelAsync();
#if !DEBUG
                await defChannel.SendMessageAsync("Server config file not found! Generating one now!");
#endif
                Globals.ServerConfigs.Add(socketGuild.Id, new ServerConfig { CommandPrefix = Globals.DEFAULT_PREFIX });
                await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs);
            }
        }

        async Task ClientOnReadyAsync() => await Client.SetGame("Ending Misinformation");

        #endregion Overrides of BotBase
    }
}