#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/19/2016 6:19 PM
// Last Revised: 10/19/2016 6:19 PM
// Last Revised by: Alex Gravely
#endregion
namespace BCL.Modules.Admin {
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    [RequirePermission(GuildPermission.ManageGuild), RequireContext(ContextType.Guild)]
    public class AdminModule : ModuleBase {

        [Command("setprefix")]
        public async Task ChangePrefixAsync(string prefix) {
            var newConfig = Globals.ServerConfigs[Context.Guild.Id];
            newConfig.CommandPrefix = prefix;
            Globals.ServerConfigs[Context.Guild.Id] = newConfig;
            await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs).ConfigureAwait(false);
            await ReplyAsync($"Prefix changed to {Globals.ServerConfigs[Context.Guild.Id].CommandPrefix}").ConfigureAwait(false);
        }
    }
}