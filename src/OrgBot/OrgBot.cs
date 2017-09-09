using Discord.Commands;
using DiscordBCL;
using DiscordBCL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OrgBot
{
    public class OrgBot : BotBase
    {
        protected override IServiceProvider ConfigureServices()
            => new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(new CommandService(CreateDefaultCommandServiceConfig()))
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<EvalService>()
                .AddSingleton<LiteDbService>()
                .AddSingleton<GuildConfigService>()
                .AddSingleton(_config)
                .BuildServiceProvider();
    }
}
