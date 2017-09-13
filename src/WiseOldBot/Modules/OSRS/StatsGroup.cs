// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 10/11/2016 6:18 PM
// Last Revised: 11/01/2016 8:10 PM
// Last Revised by: Alex Gravely

namespace WiseOldBot.Modules.OSRS
{
    using BCL;
    using Discord.Commands;
    using Entities;
    using System.Threading.Tasks;

    public partial class OSRSModule
    {
        [Name("OSRS Stats")]
        public class StatsGroup : ModuleBase<SocketCommandContext>
        {
            [Command("lookup"), Alias("l")]
            [Summary("Looks up an account from the high scores, with various skill and mode options.")]
            [Remarks("lookup regular all anti-tcb")]
            public async Task GetStatsAsync([Summary("Game mode")]GameMode gameMode = GameMode.Regular,
                [Summary("Skill")]SkillType skillType = SkillType.All, [Summary("Account to search"), Remainder] string playerName = "")
            {
                if (string.IsNullOrEmpty(playerName))
                {
                    if (!((WiseOldBotConfig)Globals.BotConfig).UsernameMap.TryGetValue(Context.User.Id, out playerName))
                    {
                        await ReplyAsync("You must specify a username, or set a default username using `defname`.");
                        return;
                    }
                }
                var player = await OSRSAPIClient.DownloadStatsAsync(playerName, gameMode);
                if (player != null)
                {
                    await ReplyAsync("", embed: player.SkillToDiscordEmbed(skillType).Build());
                    return;
                }
                await ReplyAsync("Player's highscores could not be found.");
            }

            [Command("lookup"), Alias("l"),
                Summary("Looks up an account from the high scores, with various skill and mode options")]
            public async Task GetStatsAsync([Summary("Game mode")]GameMode gameMode = GameMode.Regular, [Summary("Account to search"), Remainder]string playerName = "")
            {
                await GetStatsAsync(gameMode, SkillType.All, playerName);
            }

            [Command("lookup"), Alias("l"),
                Summary("Looks up an account from the high scores, with various skill and mode options")]
            public async Task GetStatsAsync([Summary("Skill")]SkillType skillType = SkillType.All, [Summary("Account to search"), Remainder]string playerName = "")
            {
                await GetStatsAsync(GameMode.Regular, skillType, playerName);
            }
        }
    }
}