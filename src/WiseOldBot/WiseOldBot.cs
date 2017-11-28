namespace WiseOldBot
{
    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Modules.GETracker.Entities;

    public class WiseOldBot : BotBase
    {
        public WiseOldBot() : base() {
            var _ = GETrackerAPIClient.BASE_URI;   
        }

        public override IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(new CommandService(new CommandServiceConfig { LogLevel = LogSeverity.Debug }))
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
        }
        public async override Task InstallCommandsAsync()
        {
            Client.Log += LogAsync;
            Commands.CommandService.Log += LogAsync;
            await Commands.InstallAsync();
        }

        public async override Task StartAsync<T>()
        {
            AddEventHandlers();
            AddEvalImports();

            await HandleConfigsAsync<T>();
            await InstallCommandsAsync();
            await LoginAndConnectAsync(TokenType.Bot);
        }

        private static void AddEvalImports() =>
            Globals.EvalImports.AddRange(new[] {"WiseOldBot",
                "WiseOldBot.Modules.GETracker", "WiseOldBot.Modules.GETracker.Entities",
                "WiseOldBot.Modules.OSRS", "WiseOldBot.Modules.OSRS.Entities"});

        private void AddEventHandlers()
        {
            Client.GuildAvailable += CheckForGuildConfigAsync;
            Client.JoinedGuild += CreateGuildConfigAsync;
            Client.LeftGuild += DeleteGuildConfigAsync;
        }

        private async Task CheckForGuildConfigAsync(SocketGuild socketGuild)
        {
            if (!Globals.ServerConfigs.TryGetValue(socketGuild.Id, out var outValue))
            {
                Globals.ServerConfigs.Add(socketGuild.Id, new ServerConfig { CommandPrefix = Globals.DEFAULT_PREFIX });
                await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs);
            }
        }
    }
}