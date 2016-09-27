#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 4:11 PM
// Last Revised: 09/15/2016 1:03 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using System;
    using System.Threading.Tasks;
    using Discord;
    using Discord.WebSocket;
    using Interfaces;

    #endregion

    public abstract class BotBase : IBotBase {
        #region Implementation of IBotBase

        public string BotToken { get; set; }
        public DiscordSocketClient Client { get; set; }
        public ICommandHandler Commands { get; set; }
        public IConfig Configs { get; set; }
        public const string CONFIG_PATH = "config.json";

        public async Task LoginAndConnectAsync() {
            await Client.LoginAsync(TokenType.Bot, BotToken);
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        public abstract void HandleConfigs();

        public abstract Task InstallCommandsAsync();

        /// <summary>
        /// Create a <see cref="DiscordSocketClient"/> and <see cref="CommandHandler"/>, then call <see cref="HandleConfigs"/> and <see cref="LoginAndConnectAsync"/>.
        /// </summary>
        public abstract Task StartAsync();

        public virtual Task Log(LogMessage log) {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        #endregion Implementation of IBotBase
    }
}