#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/14/2016 4:09 AM
// Last Revised: 09/15/2016 1:03 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot {
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using BCL;
    using BCL.Interfaces;
    using BCL.Modules;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.DependencyModel;
    using OSRS;
    using TypeReaders;
    using Game = Discord.API.Game;

    #endregion

    public class Program : BotBase {
        #region Public Methods
        public static void Main(string[] args) => new Program().StartAsync<WiseOldBotConfig>().GetAwaiter().GetResult();

        #endregion Public Methods

        #region Overrides of BotBase

        public async override Task StartAsync<T>() {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Debug });
            Client.Ready += ClientOnReadyAsync;
            Client.GuildAvailable += ClientOnGuildAvailableAsync;
            await HandleConfigsAsync<T>();
            await InstallCommandsAsync();
            await LoginAndConnectAsync(TokenType.Bot);
        }

        async Task ClientOnGuildAvailableAsync(SocketGuild socketGuild) {
            ServerConfig outValue;
            if (!Globals.ServerConfigs.TryGetValue(socketGuild.Id, out outValue)) {
                var defChannel = await socketGuild.GetDefaultChannelAsync(); 
                await defChannel.SendMessageAsync("Server config file not found! Generating one now!");
                Globals.ServerConfigs.Add(socketGuild.Id, new ServerConfig {CommandPrefix = Globals.DEFAULT_PREFIX});
                await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs);
            }
        }

        public async override Task InstallCommandsAsync() {
            Commands = new CommandHandler();
            Client.Log += Log;

            var map = new DependencyMap();
            map.Add(Client);
            await Commands.InstallAsync(map);
            Commands.Service.AddTypeReader<HighScoreType>(new HighScoreTypeReader());
            Commands.Service.AddTypeReader<SkillType>(new SkillTypeReader());
        }

        async Task ClientOnReadyAsync() => await Client.CurrentUser.ModifyStatusAsync(
            x => x.Game = new Optional<Game>(new Game {Name = "Spying on the Draynor Bank"}));

        #endregion Overrides of BotBase
    }
}