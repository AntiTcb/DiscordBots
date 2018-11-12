using System;
using System.IO;
using System.Threading.Tasks;
using Angler.Services;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBCL;
using DiscordBCL.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Angler
{
    public class Angler
    {
        private IServiceProvider _services;
        private readonly DiscordShardedClient _client;
        private readonly BotConfig _config;
        private readonly HookService _hookService;

        public Angler(int shards = 1)
        {
            _client = new DiscordShardedClient(BotBase.CreateDefaultSocketConfig(shards));
            _config = BotConfig.Load();
            _services = ConfigureServices();
            _hookService = _services.GetRequiredService<HookService>();
            
            ConfigureWebhooks();

            _client.Log += LogMessageAsync;

            const string outputTemplate = "[{Timestamp:HH:mm:ss}] {Level:u3} {Message:lj}{Newline}{Exception}{Newline}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine("logs", DateTime.Now.Month.ToString(), "log.txt"), rollingInterval: RollingInterval.Day, outputTemplate: outputTemplate)
                .WriteTo.Console()
                .CreateLogger();
        }

        public async Task RunAsync()
        {
            await _services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private void ConfigureWebhooks() => _client.MessageReceived += async (msg) =>
        {
            if (msg.Author.IsWebhook && msg.Channel.Id == 363797362319032341)
            {
                switch (msg.Author.Id)
                {
                    // YGOrg
                    case 363797385081782284:
                        await _hookService.FireWebhooksAsync(Website.YGOrg, msg);
                        break;
                    // CardCoal
                    case 363799291745009665:
                        await _hookService.FireWebhooksAsync(Website.CardCoal, msg);
                        break;
                }
            }
        };

        private IServiceProvider ConfigureServices()
            => new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(new CommandService(BotBase.CreateDefaultCommandServiceConfig()))
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<DiscordBCL.Services.EvalService>()
                .AddSingleton<DiscordBCL.Services.LiteDbService>()
                .AddSingleton<DiscordBCL.Services.GuildConfigService>()
                .AddSingleton(new InteractiveService(_client.GetShard(0)))
                .AddSingleton<HookService>()
                .AddSingleton(_config)
                .BuildServiceProvider();

        private async Task LogMessageAsync(LogMessage logMsg)
        {
            await PrettyConsole.LogAsync(logMsg.Severity, logMsg.Source, logMsg.Exception?.ToString() ?? logMsg.Message);
            Log.Debug(logMsg.Exception?.ToString() ?? logMsg.Message);
        }
    }
}
