﻿using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using DiscordBCL.Services;

namespace DiscordBCL.Modules
{
    [Name("Config")]
    public class ConfigModule : ModuleBase<ShardedCommandContext>
    {
        public GuildConfigService GuildConfigService { get; set; }

        [Command("prefix"), Alias("setprefix", "changeprefix")]
        [RequireContext(ContextType.Guild), RequireUserPermission(GuildPermission.ManageGuild)]
        [Summary("Gets or sets the bot's command prefix."), Remarks("changeprefix !!")]
        public async Task ChangePrefixAsync(string prefix = null)
        {
            var config = GuildConfigService.GetConfig(Context.Guild.Id);

            if (config == null)
                await ReplyAsync("No config found!");

            if (string.IsNullOrWhiteSpace(prefix))
            {
                await ReplyAsync(config.Prefix).ConfigureAwait(false);
                return;
            }
            config.Prefix = prefix;
            GuildConfigService.UpdateConfig(config);
            await ReplyAsync($"Changed guild command prefix to {config.Prefix}").ConfigureAwait(false);
        }
    }
}