using System;
using System.IO;      
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBCL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBCL
{                                
    public abstract class BotBase
    {
        protected DiscordShardedClient Client { get; set; }
        protected IConfiguration _config;
        protected IServiceProvider _services; 

        public BotBase(int totalShards = 1) 
            : this(CreateDefaultSocketConfig(totalShards))
        {
            _config = LoadConfig();
            _services = ConfigureServices();              
            // Initialize services
            _services.GetRequiredService<EvalService>();
            Client.Log += OnClientLogAsync;
        }

        private Task OnClientLogAsync(LogMessage arg)
            => PrettyConsole.LogAsync(arg.Severity, arg.Source, arg.Exception?.ToString() ?? arg.Message);

        public BotBase(DiscordSocketConfig config) 
            => Client = new DiscordShardedClient(config);

        public virtual async Task RunAsync(TokenType tokenType)
        {
            PrettyConsole.WriteLine("Loading Command Handling Service...");
            await _services.GetRequiredService<CommandHandlingService>().InitializeAsync().ConfigureAwait(false);
            await Client.LoginAsync(tokenType, _config["token"]).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);                     
            await Task.Delay(-1).ConfigureAwait(false);                                       
        }

        protected virtual IConfiguration LoadConfig()
        {
            if (!File.Exists("config.json"))
            {
                File.Create("config.json");
                throw new InvalidOperationException("config.json file not found, one has been created. Please restart!");
            }

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", false, true)
                .Build();
        }

        protected virtual IServiceProvider ConfigureServices() 
            => new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(new CommandService(CreateDefaultCommandServiceConfig()))
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<EvalService>()
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
