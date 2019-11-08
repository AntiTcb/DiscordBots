namespace OrgBot
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.DependencyInjection;
    using WikiClientLibrary.Client;
    using WikiClientLibrary.Sites;

    public class OrgBot : BotBase
    {
        public OrgBot() : base() { }


        public override IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(new CommandService(new CommandServiceConfig { LogLevel = LogSeverity.Debug }))
                .AddSingleton<CommandHandler>()
                .AddSingleton(new WikiSite(new WikiClient() { ClientUserAgent = $"OrgBot/{Assembly.GetEntryAssembly().GetName().Version} (AntiTcb)" }, "https://yugipedia.com/api.php"))
                .AddSingleton<YugipediaService>()
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

            var wiki = Services.GetRequiredService<WikiSite>();
            await wiki.Initialization;

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
            if (!Globals.ServerConfigs.ContainsKey(socketGuild.Id))
            {
                Globals.ServerConfigs.Add(socketGuild.Id, new ServerConfig { CommandPrefix = Globals.DEFAULT_PREFIX });
                await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs);
            }
        }
    }
}