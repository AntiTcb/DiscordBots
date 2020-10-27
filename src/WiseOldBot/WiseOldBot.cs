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
    using WiseOldBot.Modules.GETracker;
    using global::WiseOldBot.Modules.GETracker;
    using System.Linq;

    public class WiseOldBot : BotBase
    {
        public WiseOldBot() : base() => _ = GETrackerAPIClient.BASE_URI;

        public override IServiceProvider ConfigureServices() 
            => new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(new CommandService(new CommandServiceConfig { LogLevel = LogSeverity.Debug }))
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();

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
            Client.MessageReceived += HandleRSUpdateAsync;
        }

        private async Task HandleRSUpdateAsync(SocketMessage msg)
        {
            if (msg.Author.IsWebhook && msg.Author.Id == 710419964254748722 && msg.Content.Contains("[RS Update]"))
            {
                var newItems = await GETrackerModule.RebuildItemsAsync();

                if (!newItems.Any())
                {
                    await msg.Channel.SendMessageAsync("Item map rebuilt. No new items.");
                    return;
                }

                await msg.Channel.SendMessageAsync($"Item map rebuilt! New items: {string.Join(", ", newItems.Select(x => x.Key.ToString()))}");
            }
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