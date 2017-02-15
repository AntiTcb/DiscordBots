// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/18/2016 4:22 PM
// Last Revised: 11/08/2016 11:11 PM
// Last Revised by: Alex Gravely

namespace OrgBot
{
    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class OrgBot : BotBase
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

        internal async Task AnnounceStreaming()
        {
            await (Client.GetChannel(87463676595949568) as SocketTextChannel).SendMessageAsync("We are currently streaming! Check us out: https://twitch.tv/ygorganization");
        }

        Timer StreamAnnouceTimer;

        static void AddEvalImports() =>
            Globals.EvalImports.AddRange(new[] { "OrgBot",
                "OrgBot.Modules", "OrgBot.Modules.YGOCard", "OrgBot.Modules.YGOCard.Entities",
                "OrgBot.Modules.YGOWikia", "OrgBot.Modules.YGOWikia.Entities" });

        void AddEventHandlers()
        {
            Client.GuildAvailable += CheckForGuildConfigAsync;
            Client.JoinedGuild += CreateGuildConfigAsync;
            Client.LeftGuild += DeleteGuildConfigAsync;
            Client.UserUpdated += WatchDanForStreamingAsync;
        }

        async Task CheckForGuildConfigAsync(SocketGuild socketGuild)
        {
            ServerConfig outValue;
            if (!Globals.ServerConfigs.TryGetValue(socketGuild.Id, out outValue))
            {
                var defChannel = await socketGuild.GetDefaultChannelAsync();
#if !DEBUG
                await defChannel.SendMessageAsync("Server config file not found! Generating one now!");
#endif
                Globals.ServerConfigs.Add(socketGuild.Id, new ServerConfig { CommandPrefix = Globals.DEFAULT_PREFIX });
                await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs);
            }
        }

        void EnableStreamAnnounceTimer()
        {
            StreamAnnouceTimer = new Timer(async s => { await AnnounceStreaming(); }, null, TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(10));
        }

        Task WatchDanForStreamingAsync(SocketUser before, SocketUser after)
        {
            if (before.Id != 107522436093734912)
                return Task.CompletedTask;
            if (after.Game?.StreamType == StreamType.Twitch && after.Game?.StreamUrl == "https://twitch.tv/ygorganization")
            {
                EnableStreamAnnounceTimer();
            }
            else
            {
                StreamAnnouceTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            return Task.CompletedTask;
        }
    }
}