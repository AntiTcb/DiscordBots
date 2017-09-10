﻿namespace BCL
{
    using Discord;
    using Discord.WebSocket;
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class BotBase : IBotBase
    {
        public DiscordSocketClient Client { get; set; }
        public CommandHandler Commands { get; set; }
        public IServiceProvider Services { get; set; }

        public BotBase()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig { MessageCacheSize = 100, LogLevel = LogSeverity.Debug });
            Services = ConfigureServices();
            Commands = Services.GetRequiredService<CommandHandler>();
        }

        public async virtual Task CreateGuildConfigAsync(SocketGuild guild)
        {
            if (guild.DefaultChannel != null)
                await guild.DefaultChannel.SendMessageAsync($"Thank you for adding me to the server! The default prefix is currently set to `{Globals.DEFAULT_PREFIX}`." +
                    $"Any user with the Manage Server permission may change this with the `setprefix` command. Use `{Globals.DEFAULT_PREFIX}help` to see all my commands").ConfigureAwait(false);

            var newServerConfig = new ServerConfig(Globals.DEFAULT_PREFIX, new Dictionary<string, string>());
            Globals.ServerConfigs.Add(guild.Id, newServerConfig);
            await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs).ConfigureAwait(false);
        }

        public async virtual Task DeleteGuildConfigAsync(SocketGuild guild)
        {
            if (Globals.ServerConfigs.ContainsKey(guild.Id))
                Globals.ServerConfigs.Remove(guild.Id);     

            await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs).ConfigureAwait(false);
        }

        public async virtual Task HandleConfigsAsync<T>() where T : IBotConfig, new()
        {
            Globals.BotConfig = await ConfigHandler.LoadBotConfigAsync<T>().ConfigureAwait(false);
            Globals.ServerConfigs = await ConfigHandler.LoadServerConfigsAsync<ServerConfig>().ConfigureAwait(false);
        }

        public abstract Task InstallCommandsAsync();
        public abstract IServiceProvider ConfigureServices();

        public virtual Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        public async virtual Task LoginAndConnectAsync(TokenType tokenType)
        {
            await Client.LoginAsync(tokenType, Globals.BotConfig.BotToken).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }

        public async virtual Task StartAsync<T>() where T : IBotConfig, new()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Info });
            Client.JoinedGuild += CreateGuildConfigAsync;
            Client.LeftGuild += DeleteGuildConfigAsync;
            await HandleConfigsAsync<T>().ConfigureAwait(false);
            await InstallCommandsAsync().ConfigureAwait(false);
            await LoginAndConnectAsync(TokenType.Bot).ConfigureAwait(false);
        }
    }
}