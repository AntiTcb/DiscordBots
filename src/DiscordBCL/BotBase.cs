using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBCL.Configuration;
using DiscordBCL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DiscordBCL
{
    public abstract partial class BotBase
    {
        protected DiscordShardedClient Client { get; set; } 
        protected BotConfig _config;
        protected IServiceProvider _services;
        protected GuildConfigService _guildConfigService;

        public BotBase(int totalShards = 1) 
            : this(CreateDefaultSocketConfig(totalShards))
        {
            _config = BotConfig.Load();
            _services = ConfigureServices();
            // Initialize services
            _services.GetRequiredService<EvalService>();
            _guildConfigService = _services.GetRequiredService<GuildConfigService>();
            AttachEventHandlers();
        }

        public BotBase(DiscordSocketConfig config) 
            => Client = new DiscordShardedClient(config);

        public virtual void AttachEventHandlers()
        {
            Client.Log += LogMessageAsync;
            Client.JoinedGuild += CreateGuildConfigAsync;
            Client.GuildAvailable += ValidateGuildConfigAsync;
            Client.LeftGuild += RemoveGuildConfigAsync;
        }

        public virtual async Task RunAsync(TokenType tokenType)
        {
            await _services.GetRequiredService<CommandHandlingService>().InitializeAsync().ConfigureAwait(false);
            await Client.LoginAsync(tokenType, _config.Token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }     

        protected virtual IServiceProvider ConfigureServices() 
            => new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(new CommandService(CreateDefaultCommandServiceConfig()))
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<EvalService>()
                .AddSingleton<LiteDbService>()
                .AddSingleton<GuildConfigService>()
                .AddSingleton(_config)
                .BuildServiceProvider();

        public static CommandServiceConfig CreateDefaultCommandServiceConfig()
            => new CommandServiceConfig
            {   
                ThrowOnError = true,
#if DEBUG
                LogLevel = LogSeverity.Debug
#else
                LogLevel = LogSeverity.Info
#endif
            };
        public static DiscordSocketConfig CreateDefaultSocketConfig(int shardAmount) 
            => new DiscordSocketConfig
            {
                MessageCacheSize = 1000,
                TotalShards = shardAmount,
    #if DEBUG
                LogLevel = LogSeverity.Debug
    #else
                LogLevel = LogSeverity.Info
    #endif
            };
    }
}
