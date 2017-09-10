namespace OrgBot
{
    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public class OrgBot : BotBase
    {
        public OrgBot() : base() { }

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
            Client.Log += Log;
            Commands.CommandService.Log += Log;
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
            Globals.EvalImports.AddRange(new[] { "OrgBot",
                "OrgBot.Modules", "OrgBot.Modules.YGOCard", "OrgBot.Modules.YGOCard.Entities",
                "OrgBot.Modules.YGOWikia", "OrgBot.Modules.YGOWikia.Entities" });

        private void AddEventHandlers()
        {
            Client.GuildAvailable += CheckForGuildConfigAsync;
            Client.JoinedGuild += CreateGuildConfigAsync;
            Client.LeftGuild += DeleteGuildConfigAsync;
        }

        private async Task CheckForGuildConfigAsync(SocketGuild socketGuild)
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