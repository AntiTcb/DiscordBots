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

        public struct Paths {
            public static string CONFIG_PATH = "config.json";
            public static string SERVER_CONFIG_PATH = "server_configs.json";
        }

        #region Implementation of IBotBase

        public DiscordSocketClient Client { get; set; }
        public ICommandHandler Commands { get; set; }
        public IConfig Configs { get; set; }
        public Dictionary<ulong, IServerConfig> ServerConfigs { get; set; } =
            new Dictionary<ulong, IServerConfig>();

        public abstract Task HandleConfigsAsync();

        public abstract Task InstallCommandsAsync();

        public async virtual Task LoadServerConfig(ulong serverID) {
            //ServerConfigs = await ConfigHandler.LoadAsync<ServerConfig>(SERVER_CONFIG_PATH);
        }

        public virtual Task Log(LogMessage log) {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        public async virtual Task LoginAndConnectAsync(TokenType tokenType) {
            await Client.LoginAsync(tokenType, Configs.BotToken);
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        /// <summary>
        ///     Create a <see cref="DiscordSocketClient" /> and <see cref="CommandHandler" />, then call
        ///     <see cref="HandleConfigsAsync" /> and <see cref="LoginAndConnectAsync" />.
        /// </summary>
        public abstract Task StartAsync();

        #endregion Implementation of IBotBase
    }
}