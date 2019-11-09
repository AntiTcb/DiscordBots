namespace BCL
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public abstract class BotBase : IBotBase
    {
        public DiscordSocketClient Client { get; set; }
        public CommandHandler Commands { get; set; }
        public IServiceProvider Services { get; set; }

        public BotBase()
        {
            const string outputTemplate = "[{Timestamp:HH:mm:ss}] {Level:u3} {Message:lj}{Exception}{Newline}";

            Client = new DiscordSocketClient(new DiscordSocketConfig { MessageCacheSize = 100, LogLevel = LogSeverity.Debug });
            Services = ConfigureServices();
            Commands = Services.GetRequiredService<CommandHandler>();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine("logs", "log.txt"), rollingInterval: RollingInterval.Day, outputTemplate: outputTemplate)
                .WriteTo.Console()
                .CreateLogger();
        }

        public async virtual Task CreateGuildConfigAsync(SocketGuild guild)
        {
            var newServerConfig = new ServerConfig(Globals.DEFAULT_PREFIX, new Dictionary<string, string>());
            Globals.ServerConfigs.Add(guild.Id, newServerConfig);
            await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs).ConfigureAwait(false);

            if (guild.DefaultChannel != null && guild.CurrentUser.GetPermissions(guild.DefaultChannel).SendMessages)
            {
                await guild.DefaultChannel.SendMessageAsync($"Thank you for adding me to the server! The default prefix is currently set to `{Globals.DEFAULT_PREFIX}`." +
                    $"Any user with the Manage Server permission may change this with the `setprefix` command. Use `{Globals.DEFAULT_PREFIX}help` to see all my commands").ConfigureAwait(false);
            }
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

        public virtual async Task LogAsync(LogMessage log)
        {
            string msg = log.Exception?.ToString() ?? log.Message;

            if (!msg.Contains("Received Dispatch"))
                Log.Debug(msg);

            if (log.Exception is CommandException cmdEx)
            {
                msg = $"{cmdEx.InnerException.GetType().Name}: {cmdEx.InnerException.Message}";
                var eb = new EmbedBuilder
                {
                    Title = msg,
                    Color = Color.DarkRed,
                    //Description = Format.Sanitize(cmdEx.InnerException.StackTrace).Truncate(5500)
                };
                var ex = cmdEx.InnerException;
                var sb = new StringBuilder("**Exceptions:**\n");
                int exCount = 1;
                while (ex != null)
                {
                    sb.AppendLine($"Exception #{exCount}: {ex.ToString()}");
                    ex = ex.InnerException;
                }
                eb.WithDescription(sb.ToString());
                await cmdEx.Context.Channel.SendMessageAsync("", embed: eb.Build()).ConfigureAwait(false);
                var loggingChannel = Client.GetChannel(Globals.BotConfig.LogChannel) as SocketTextChannel;
                await loggingChannel.SendMessageAsync("", embed: eb.Build()).ConfigureAwait(false);
            }
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