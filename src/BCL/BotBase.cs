#region Header

// Description:
//
// Solution: DiscordBots
// Project: BCL
//
// Created: 09/26/2016 11:16 PM
// Last Revised: 10/05/2016 7:04 PM
// Last Revised by: Alex Gravely

#endregion Header

namespace BCL
{
    #region Using

    using Discord;
    using Discord.WebSocket;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #endregion Using

    public abstract class BotBase : IBotBase {

        #region Implementation of IBotBase

        public DiscordSocketClient Client { get; set; }
        public ICommandHandler Commands { get; set; }
        public IBotConfig BotConfig { get; set; }
        public Dictionary<ulong, IServerConfig> ServerConfigs { get; set; } =
            new Dictionary<ulong, IServerConfig>();

        public async virtual Task HandleConfigsAsync() {
            //BotConfig = ConfigHandler.LoadBotConfigAsync<BotConfig>(Globals.CONFIG_PATH);
            //ServerConfigs =  
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

        /// <summary>
        ///     Create a <see cref="DiscordSocketClient" /> and <see cref="CommandHandler" />, then call
        ///     <see cref="HandleConfigsAsync" /> and <see cref="LoginAndConnectAsync" />.
        /// </summary>
        public async virtual Task StartAsync() {
            Client = new DiscordSocketClient(new DiscordSocketConfig {LogLevel = LogSeverity.Info});
            Client.JoinedGuild += ClientOnJoinedGuildAsync;
            Client.LeftGuild += ClientOnLeftGuildAsync;
            await HandleConfigsAsync().ConfigureAwait(false);
            await InstallCommandsAsync().ConfigureAwait(false);
            await LoginAndConnectAsync(TokenType.Bot).ConfigureAwait(false);
        }

        public async virtual Task ClientOnLeftGuildAsync(IGuild guild)
        {
            if (ServerConfigs.ContainsKey(guild.Id)) {
                ServerConfigs.Remove(guild.Id);
            }
        }

        public async virtual Task ClientOnJoinedGuildAsync(IGuild guild) {
            var defaultChannel = await guild.GetDefaultChannelAsync().ConfigureAwait(false);
            await defaultChannel.SendMessageAsync("Thank you for adding me to the server! A few of my settings need to be configured, and then I'm ready to roll!").ConfigureAwait(false);
            var newServerConfig = new ServerConfig(Globals.DEFAULT_PREFIX);
            ServerConfigs.Add(guild.Id, newServerConfig);
        }

        #endregion Implementation of IBotBase
    }
}