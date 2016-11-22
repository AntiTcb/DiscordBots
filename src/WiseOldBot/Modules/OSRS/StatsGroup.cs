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

    using System.Threading.Tasks;
    using BCL;
    using Discord.Commands;
    using Entities;

    #endregion

    public partial class OSRSModule {
        #region Public Structs + Classes

        [Name("stats")]
        public class StatsGroup : ModuleBase {
            #region Public Methods  
            [Command("lookup"), Alias("l"), 
                Summary("Looks up an account from the high scores, with various skill and mode options."),
                Remarks("lookup regular all anti-tcb")]
            public async Task GetStatsAsync([Summary("Game mode")]GameMode hsType = GameMode.Regular, 
                [Summary("Skill")]SkillType skillType = SkillType.All, [Summary("Account to search"), Remainder] string playerName = "") {
                if (string.IsNullOrEmpty(playerName)) {
                    if (!((WiseOldBotConfig)Globals.BotConfig).UsernameMap.TryGetValue(Context.User.Id, out playerName)) {
                        await ReplyAsync("You must specify a username, or set a default username using `defname`.");
                        return;
                    }
                }
                var player = await OSRSAPIClient.DownloadStatsAsync(playerName, hsType);
                if (player != null) {
                    await ReplyAsync("", embed: player.SkillToDiscordEmbed(skillType));
                    return;
                }
                await ReplyAsync("Player's highscores could not be found.");
            }

            #endregion Public Methods 
        }

        #endregion Public Structs + Classes
    }
}