// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 10/18/2016 4:17 PM
// Last Revised: 10/23/2016 6:51 PM
// Last Revised by: Alex Gravely

namespace WiseOldBot
{
    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using System.Threading.Tasks;

    public class WiseOldBot : BotBase
    {
        public async override Task InstallCommandsAsync()
        {
            Commands = new CommandHandler();
            Client.Log += Log;

            var map = new DependencyMap();
            map.Add(Client);
            await Commands.InstallAsync(map);
        }

        public async override Task StartAsync<T>()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Debug });

            AddEventHandlers();
            AddEvalImports();

            await HandleConfigsAsync<T>();
            await InstallCommandsAsync();
            await LoginAndConnectAsync(TokenType.Bot);
        }

        static void AddEvalImports() =>
            Globals.EvalImports.AddRange(new[] {"WiseOldBot",
                "WiseOldBot.Modules.GETracker", "WiseOldBot.Modules.GETracker.Entities",
                "WiseOldBot.Modules.OSRS", "WiseOldBot.Modules.OSRS.Entities"});

        void AddEventHandlers()
        {
            Client.GuildAvailable += CheckForGuildConfigAsync;
            Client.JoinedGuild += CreateGuildConfigAsync;
            Client.LeftGuild += DeleteGuildConfigAsync;
        }

        async Task CheckForGuildConfigAsync(SocketGuild socketGuild)
        {
            ServerConfig outValue;
            if (!Globals.ServerConfigs.TryGetValue(socketGuild.Id, out outValue))
            {
                Globals.ServerConfigs.Add(socketGuild.Id, new ServerConfig { CommandPrefix = Globals.DEFAULT_PREFIX });
                await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs);
            }
        }
    }
}