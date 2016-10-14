#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/26/2016 11:16 PM
// Last Revised: 10/13/2016 7:53 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Discord;
    using Discord.WebSocket;
    using Interfaces;

    #endregion

    public abstract class BotBase : IBotBase {
        #region Implementation of IBotBase

        public IBotConfig BotConfig { get; set; }
        public DiscordSocketClient Client { get; set; }
        public ICommandHandler Commands { get; set; }

        public Dictionary<ulong, IServerConfig> ServerConfigs { get; set; } = new Dictionary<ulong, IServerConfig>();

        public async virtual Task ClientOnJoinedGuildAsync(IGuild guild) {
            var defaultChannel = await guild.GetDefaultChannelAsync().ConfigureAwait(false);
            await defaultChannel.SendMessageAsync("Thank you for adding me to the server!").ConfigureAwait(false);
            var newServerConfig = new ServerConfig(Globals.DEFAULT_PREFIX);
            ServerConfigs.Add(guild.Id, newServerConfig);
            await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, ServerConfigs);
        }

        public async virtual Task ClientOnLeftGuildAsync(IGuild guild) {
            if (ServerConfigs.ContainsKey(guild.Id)) {
                ServerConfigs.Remove(guild.Id);
            }
            await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, ServerConfigs);
        }

        public async virtual Task HandleConfigsAsync() {
            BotConfig = await ConfigHandler.LoadBotConfigAsync<BotConfig>().ConfigureAwait(false);
            ServerConfigs = await ConfigHandler.LoadServerConfigsAsync().ConfigureAwait(false);
        }

        public abstract Task InstallCommandsAsync();

        public virtual Task Log(LogMessage log) {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        public async virtual Task LoginAndConnectAsync(TokenType tokenType) {
            await Client.LoginAsync(tokenType, BotConfig.BotToken).ConfigureAwait(false);
            await Client.ConnectAsync().ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }

        public async virtual Task StartAsync() {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Info });
            Client.JoinedGuild += ClientOnJoinedGuildAsync;
            Client.LeftGuild += ClientOnLeftGuildAsync;
            await HandleConfigsAsync().ConfigureAwait(false);
            await InstallCommandsAsync().ConfigureAwait(false);
            await LoginAndConnectAsync(TokenType.Bot).ConfigureAwait(false);
        }

        #endregion Implementation of IBotBase
    }
}