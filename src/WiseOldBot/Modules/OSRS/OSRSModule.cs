// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 10/11/2016 6:18 PM
// Last Revised: 10/16/2016 10:44 PM
// Last Revised by: Alex Gravely

namespace WiseOldBot.Modules.OSRS {

    using BCL;
    using Discord;
    using Discord.Commands;
    using System;
    using System.Threading.Tasks;

    [Name("OSRS")]
    public partial class OSRSModule : ModuleBase<SocketCommandContext> {

        [Command("lvl2exp"), Alias("l2e"),
            Summary("Gets the minimum experience required for the input level."),
            Remarks("lvl2exp 95")]
        public async Task CalculateExperienceAsync([Summary("Level")]uint level) {
            var exp = LevelToExperience(level);
            if (level > 99)
                await ReplyAsync($"Minimum exp: {exp}.");
            else {
                var nextLevelExp = LevelToExperience(level + 1);
                await ReplyAsync($"Minimum exp: {exp}. Next level: {nextLevelExp}. Difference:{nextLevelExp - exp}");
            }
        }

        [Command("exp2lvl"), Alias("e2l")]
        [Summary("Gets the Level the input experience amount would equate to."), Remarks("exp2lvl 100000")]
        public async Task CalculateLevelAsync([Summary("Experience amount")]uint exp)
            => await ReplyAsync($"Level: {ExperienceToLevel(exp)}");

        [Command("defname"), Alias("account"),
            Summary("Sets or unsets a default runescape username. Pass the --unset flag before your username to unset.")
            Remarks("defname anti-tcb")]
        public async Task ManageUsernameMapAsync([Remainder, Summary("RuneScape username")] string username) {
            var userID = Context.User.Id;
            if (!((WiseOldBotConfig)Globals.BotConfig).UsernameMap.ContainsKey(userID)) {
                ((WiseOldBotConfig)Globals.BotConfig).UsernameMap.Add(userID, null);
            }
            switch (username) {
                case "--unset":
                    ((WiseOldBotConfig)Globals.BotConfig).UsernameMap.Remove(userID);
                    await ReplyAsync("Default account name unset!");
                    break;

                default:
                    ((WiseOldBotConfig)Globals.BotConfig).UsernameMap[userID] = username;
                    await ReplyAsync($"Default account name set to {Format.Code(((WiseOldBotConfig)Globals.BotConfig).UsernameMap[userID])}");
                    break;
            }
            await ConfigHandler.SaveAsync(Globals.CONFIG_PATH, Globals.BotConfig);
        }

        uint ExperienceToLevel(double exp) {
            double pts = 0;
            for (var lvl = 1; lvl < 99; ++lvl) {
                pts += Math.Floor(lvl + (300 * Math.Pow(2, lvl / 7D)));
                var output = pts / 4D;
                if (!((exp - output) < 0)) {
                    continue;
                }
                var l_out = (pts - Math.Floor(lvl + (300 * Math.Pow(2, lvl / 7D)))) / 4;
                return (uint)(lvl + ((exp - l_out) / (output - l_out)));
            }
            return 99;
        }

        double LevelToExperience(uint level) {
            double total = 0;
            for (var i = 1; i < level; i++) {
                total += Math.Floor(i + (300 * Math.Pow(2, i / 7.0)));
            }
            return Math.Floor(total / 4);
        }
    }
}