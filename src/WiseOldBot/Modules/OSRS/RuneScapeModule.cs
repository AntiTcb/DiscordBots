#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/25/2016 6:29 AM
// Last Revised: 09/25/2016 6:55 AM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.OSRS {
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BCL;
    using Discord;
    using Discord.Commands;

    #endregion

    [Module]
    public partial class RuneScapeModule {
        WiseOldBotConfig _config;
        #region Public Structs + Classes

        public RuneScapeModule(WiseOldBotConfig config) {
            _config = config;

        }

        [Command("defname"), Alias("account")]
        public async Task ManageUsernameMapAsync(IUserMessage msg, [Remainder] string username)
        {
            var userID = msg.Author.Id;
            if (!_config.UsernameMap.ContainsKey(userID))
            {
                _config.UsernameMap.Add(userID, null);
            }
            switch (username)
            {
                case "--unset":
                    _config.UsernameMap[userID] = null;
                    await msg.Channel.SendMessageAsync("Default account name unset!");
                    break;
                default:
                    _config.UsernameMap[userID] = username;
                    await msg.Channel.SendMessageAsync($"Default account name set to {Format.Code(_config.UsernameMap[userID])}");
                    break;
            }
            ConfigHandler.Save(BotBase.CONFIG_PATH, _config);
        }

        [Command("lvl2exp"), Alias("l2e")]
        public async Task CalculateLevelAsync(IUserMessage msg, uint level) {
            var exp = LevelToExperience(level);

            if (level > 99) {
                await msg.Channel.SendMessageAsync($"Minimum exp: {exp}.");
            }
            else {
                var nextLevelExp = LevelToExperience(level + 1);
                await msg.Channel.SendMessageAsync($"Minimum exp: {exp}. Next level: {nextLevelExp}. Difference:{nextLevelExp - exp}");
            }
        }


        [Command("exp2lvl"), Alias("e2l")]
        public async Task CalculateExperienceAsync(IUserMessage msg, uint exp) => await msg.Channel.SendMessageAsync($"Level: {ExperienceToLevel(exp)}");

        double LevelToExperience(uint level) {
            double total = 0;
            for (var i = 1 ; i < level ; i++) {
                total += Math.Floor(i + 300 * Math.Pow(2, i / 7.0));
            }
            return Math.Floor(total / 4);
        }

        uint ExperienceToLevel(double exp) {
            double pts = 0;
            for (var lvl = 1; lvl < 99; ++lvl)
            {
                pts += Math.Floor(lvl + (300 * Math.Pow(2, lvl / 7D)));
                var output = pts / 4D;
                if (!(exp - output < 0)) {
                    continue;
                }
                var l_out = (pts - Math.Floor(lvl + (300 * Math.Pow(2, lvl / 7D)))) / 4;
                return (uint) (lvl + (exp - l_out) / (output - l_out));
            }
            return 99;
        }


        #endregion Public Structs + Classes
    }
}