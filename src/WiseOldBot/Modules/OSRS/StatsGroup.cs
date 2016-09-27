#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/25/2016 6:29 AM
// Last Revised: 09/27/2016 7:09 AM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.OSRS {
    #region Using

    using System;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using RestEase;

    #endregion

    public partial class RuneScapeModule {
        #region Public Structs + Classes

        [Group("stats")]
        public class StatsGroup {
            #region Internal Fields + Properties

            internal static IOSRSApi STATS_API = RestClient.For<IOSRSApi>(HS_ENDPOINT);
            const string HS_ENDPOINT = "http://services.runescape.com/";
            WiseOldBotConfig _config;

            #endregion Internal Fields + Properties

            #region Public Methods

            public StatsGroup(WiseOldBotConfig config) {
                _config = config;
            }

            internal static async Task<Account> DownloadStatsAsync(string playerName, HighScoreType hsType) {
                switch (hsType) {
                    case HighScoreType.Regular:
                        return await STATS_API.GetRegularHighScoresAsync(playerName);

                    case HighScoreType.Ironman:
                        return await STATS_API.GetIronmanHighScoresAsync(playerName);

                    case HighScoreType.Ultimate:
                        return await STATS_API.GetUltimateHighScoresAsync(playerName);

                    case HighScoreType.Deadman:
                        return await STATS_API.GetDeadmanHighScoresAsync(playerName);

                    case HighScoreType.Seasonal:
                        return await STATS_API.GetSeasonalHighScoresAsync(playerName);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(hsType), hsType, null);
                }
            }

            #endregion Public Methods

            [Command("stats")]
            public async Task GetStatsAsync
            (IUserMessage msg,
             [Summary("High Score Type")] HighScoreType hsType = HighScoreType.Regular,
             [Summary("Skill. Leave blank for all.")] SkillType skillType = SkillType.All,
             [Remainder] string playerName = "") {
                if (playerName == string.Empty) {
                    playerName = _config.UsernameMap[msg.Author.Id];
                }
                var player = await DownloadStatsAsync(playerName, hsType);

                switch (skillType) {
                    case SkillType.Agility:
                        await msg.Channel.SendMessageAsync(player.Agility.ToDiscordMessage());
                        break;

                    case SkillType.Attack:
                        await msg.Channel.SendMessageAsync(player.Attack.ToDiscordMessage());
                        break;

                    case SkillType.Construction:
                        await msg.Channel.SendMessageAsync(player.Construction.ToDiscordMessage());
                        break;

                    case SkillType.Cooking:
                        await msg.Channel.SendMessageAsync(player.Cooking.ToDiscordMessage());
                        break;

                    case SkillType.Crafting:
                        await msg.Channel.SendMessageAsync(player.Crafting.ToDiscordMessage());
                        break;

                    case SkillType.Defense:
                        await msg.Channel.SendMessageAsync(player.Defense.ToDiscordMessage());
                        break;

                    case SkillType.Farming:
                        await msg.Channel.SendMessageAsync(player.Farming.ToDiscordMessage());
                        break;

                    case SkillType.Firemaking:
                        await msg.Channel.SendMessageAsync(player.Firemaking.ToDiscordMessage());
                        break;

                    case SkillType.Fishing:
                        await msg.Channel.SendMessageAsync(player.Fishing.ToDiscordMessage());
                        break;

                    case SkillType.Fletching:
                        await msg.Channel.SendMessageAsync(player.Fletching.ToDiscordMessage());
                        break;

                    case SkillType.Herblore:
                        await msg.Channel.SendMessageAsync(player.Herblore.ToDiscordMessage());
                        break;

                    case SkillType.Hitpoints:
                        await msg.Channel.SendMessageAsync(player.Hitpoints.ToDiscordMessage());
                        break;

                    case SkillType.Hunter:
                        await msg.Channel.SendMessageAsync(player.Hunter.ToDiscordMessage());
                        break;

                    case SkillType.Magic:
                        await msg.Channel.SendMessageAsync(player.Magic.ToDiscordMessage());
                        break;

                    case SkillType.Mining:
                        await msg.Channel.SendMessageAsync(player.Mining.ToDiscordMessage());
                        break;

                    case SkillType.Prayer:
                        await msg.Channel.SendMessageAsync(player.Prayer.ToDiscordMessage());
                        break;

                    case SkillType.Ranged:
                        await msg.Channel.SendMessageAsync(player.Ranged.ToDiscordMessage());
                        break;

                    case SkillType.Runecrafting:
                        await msg.Channel.SendMessageAsync(player.Runecrafting.ToDiscordMessage());
                        break;

                    case SkillType.Slayer:
                        await msg.Channel.SendMessageAsync(player.Slayer.ToDiscordMessage());
                        break;

                    case SkillType.Smithing:
                        await msg.Channel.SendMessageAsync(player.Smithing.ToDiscordMessage());
                        break;

                    case SkillType.Strength:
                        await msg.Channel.SendMessageAsync(player.Strength.ToDiscordMessage());
                        break;

                    case SkillType.Thieving:
                        await msg.Channel.SendMessageAsync(player.Thieving.ToDiscordMessage());
                        break;

                    case SkillType.Total:
                        await msg.Channel.SendMessageAsync(player.Total.ToDiscordMessage());
                        break;

                    case SkillType.All:
                        await msg.Channel.SendMessageAsync(player.ToDiscordMessage());
                        break;

                    case SkillType.Woodcutting:
                        await msg.Channel.SendMessageAsync(player.Woodcutting.ToDiscordMessage());
                        break;

                    case SkillType.Combat:
                        break;

                    case SkillType.NonCombat:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(skillType), skillType, null);
                }
            }
        }

        #endregion Public Structs + Classes
    }
}