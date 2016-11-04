#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 10/11/2016 6:18 PM
// Last Revised: 11/01/2016 8:10 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.Modules.OSRS {
    #region Using

    using System;
    using System.Threading.Tasks;
    using BCL;
    using Discord.Commands;

    #endregion

    public partial class OSRSModule {
        #region Public Structs + Classes

        [Name("stats")]
        public class StatsGroup : ModuleBase {
            #region Public Methods

            [Command("lookup"), Alias("l")]
            public async Task GetStatsAsync(HighScoreType hsType = HighScoreType.Regular, SkillType skillType = SkillType.All, [Remainder] string playerName = "") {
                if (string.IsNullOrEmpty(playerName)) {
                    if (((WiseOldBotConfig) Globals.BotConfig).UsernameMap.ContainsKey(Context.User.Id)) {
                        playerName = ((WiseOldBotConfig) Globals.BotConfig).UsernameMap[Context.User.Id];
                    }
                    else {
                        await ReplyAsync("You must specify a username, or set a default username using `defname`.");
                        return;
                    }
                }
                var player = await OSRSAPIClient.DownloadStatsAsync(playerName, hsType);
                await ReplyAsync(player.SkillToDiscordMessage(skillType));
            }

            #endregion Public Methods
        }

        #endregion Public Structs + Classes
    }
}