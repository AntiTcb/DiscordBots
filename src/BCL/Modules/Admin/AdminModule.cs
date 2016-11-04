#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/19/2016 6:19 PM
// Last Revised: 11/04/2016 12:41 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Modules.Admin {
    #region Using

    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;

    #endregion

    [RequirePermission(GuildPermission.ManageGuild), RequireContext(ContextType.Guild)]
    public class AdminModule : ModuleBase {
        #region Public Methods

        [Command("setprefix"), Alias("prefix")]
        public async Task ChangePrefixAsync(string prefix) {
            if (string.IsNullOrEmpty(prefix)) {
                await
                    ReplyAsync
                        ($"This guild's comand prefix is currently set to: `{Globals.ServerConfigs[Context.Guild.Id].CommandPrefix}`");
                return;
            }
            var newConfig = Globals.ServerConfigs[Context.Guild.Id];
            newConfig.CommandPrefix = prefix;
            Globals.ServerConfigs[Context.Guild.Id] = newConfig;
            await ConfigHandler.SaveAsync(Globals.SERVER_CONFIG_PATH, Globals.ServerConfigs).ConfigureAwait(false);
            await
                ReplyAsync($"Prefix changed to {Globals.ServerConfigs[Context.Guild.Id].CommandPrefix}").
                    ConfigureAwait(false);
        }

        #endregion Public Methods
    }
}