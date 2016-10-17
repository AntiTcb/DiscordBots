#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 10/11/2016 6:18 PM
// Last Revised: 10/16/2016 10:44 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.OSRS {
    #region Using

    using System;
    using System.Threading.Tasks;
    using BCL;
    using Discord;
    using Discord.Commands;

    #endregion

    public partial class RuneScapeModule : ModuleBase {
        #region Public Structs + Classes

        [Command("exp2lvl"), Alias("e2l")]
        public async Task CalculateExperienceAsync(uint exp)
            => await Context.Channel.SendMessageAsync($"Level: {ExperienceToLevel(exp)}");

        [Command("lvl2exp"), Alias("l2e")]
        public async Task CalculateLevelAsync(uint level) {
            var exp = LevelToExperience(level);

            if (level > 99) {
                await Context.Channel.SendMessageAsync($"Minimum exp: {exp}.");
            }
            else {
                var nextLevelExp = LevelToExperience(level + 1);
                await
                    Context.Channel.SendMessageAsync
                            ($"Minimum exp: {exp}. Next level: {nextLevelExp}. Difference:{nextLevelExp - exp}");
            }
        }

        [Command("defname"), Alias("account")]
        public async Task ManageUsernameMapAsync([Remainder] string username) {
            var userID = Context.User.Id;
            if (!((WiseOldBotConfig) Globals.BotConfig).UsernameMap.ContainsKey(userID)) {
                ((WiseOldBotConfig) Globals.BotConfig).UsernameMap.Add(userID, null);
            }
            switch (username) {
                case "--unset":
                    ((WiseOldBotConfig) Globals.BotConfig).UsernameMap[userID] = null;
                    await Context.Channel.SendMessageAsync("Default account name unset!");
                    break;

                default:
                    ((WiseOldBotConfig) Globals.BotConfig).UsernameMap[userID] = username;
                    await
                        Context.Channel.SendMessageAsync
                                ($"Default account name set to {Format.Code(((WiseOldBotConfig) Globals.BotConfig).UsernameMap[userID])}");
                    break;
            }
            await ConfigHandler.SaveAsync(Globals.CONFIG_PATH, Globals.BotConfig);
        }

        uint ExperienceToLevel(double exp) {
            double pts = 0;
            for (var lvl = 1 ; lvl < 99 ; ++lvl) {
                pts += Math.Floor(lvl + (300 * Math.Pow(2, lvl / 7D)));
                var output = pts / 4D;
                if (!((exp - output) < 0)) {
                    continue;
                }
                var l_out = (pts - Math.Floor(lvl + (300 * Math.Pow(2, lvl / 7D)))) / 4;
                return (uint) (lvl + ((exp - l_out) / (output - l_out)));
            }
            return 99;
        }

        double LevelToExperience(uint level) {
            double total = 0;
            for (var i = 1 ; i < level ; i++) {
                total += Math.Floor(i + (300 * Math.Pow(2, i / 7.0)));
            }
            return Math.Floor(total / 4);
        }

        #endregion Public Structs + Classes
    }
}