#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 10/11/2016 6:18 PM
// Last Revised: 10/16/2016 10:46 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.OSRS {
    #region Using

    using System;
    using System.Threading.Tasks;
    using BCL;
    using Discord.Commands;
    using RestEase;

    #endregion

    public partial class RuneScapeModule {
        #region Public Structs + Classes

        [Group("stats")]
        public class StatsGroup : ModuleBase {
            #region Internal Fields + Properties

            internal static IOSRSApi STATS_API = RestClient.For<IOSRSApi>(HS_ENDPOINT);
            const string HS_ENDPOINT = "http://services.runescape.com/";

            #endregion Internal Fields + Properties

            #region Public Methods

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
            ([Summary("High Score Type")] HighScoreType hsType = HighScoreType.Regular,
             [Summary("Skill. Leave blank for all.")] SkillType skillType = SkillType.All,
             [Remainder] string playerName = "") {
                if (playerName == string.Empty) {
                    playerName = ((WiseOldBotConfig) Globals.BotConfig).UsernameMap[Context.User.Id];
                }
                var player = await DownloadStatsAsync(playerName, hsType);

                switch (skillType) {
                    case SkillType.Agility:
                        await Context.Channel.SendMessageAsync(player.Agility.ToDiscordMessage());
                        break;

                    case SkillType.Attack:
                        await Context.Channel.SendMessageAsync(player.Attack.ToDiscordMessage());
                        break;

                    case SkillType.Construction:
                        await Context.Channel.SendMessageAsync(player.Construction.ToDiscordMessage());
                        break;

                    case SkillType.Cooking:
                        await Context.Channel.SendMessageAsync(player.Cooking.ToDiscordMessage());
                        break;

                    case SkillType.Crafting:
                        await Context.Channel.SendMessageAsync(player.Crafting.ToDiscordMessage());
                        break;

                    case SkillType.Defense:
                        await Context.Channel.SendMessageAsync(player.Defense.ToDiscordMessage());
                        break;

                    case SkillType.Farming:
                        await Context.Channel.SendMessageAsync(player.Farming.ToDiscordMessage());
                        break;

                    case SkillType.Firemaking:
                        await Context.Channel.SendMessageAsync(player.Firemaking.ToDiscordMessage());
                        break;

                    case SkillType.Fishing:
                        await Context.Channel.SendMessageAsync(player.Fishing.ToDiscordMessage());
                        break;

                    case SkillType.Fletching:
                        await Context.Channel.SendMessageAsync(player.Fletching.ToDiscordMessage());
                        break;

                    case SkillType.Herblore:
                        await Context.Channel.SendMessageAsync(player.Herblore.ToDiscordMessage());
                        break;

                    case SkillType.Hitpoints:
                        await Context.Channel.SendMessageAsync(player.Hitpoints.ToDiscordMessage());
                        break;

                    case SkillType.Hunter:
                        await Context.Channel.SendMessageAsync(player.Hunter.ToDiscordMessage());
                        break;

                    case SkillType.Magic:
                        await Context.Channel.SendMessageAsync(player.Magic.ToDiscordMessage());
                        break;

                    case SkillType.Mining:
                        await Context.Channel.SendMessageAsync(player.Mining.ToDiscordMessage());
                        break;

                    case SkillType.Prayer:
                        await Context.Channel.SendMessageAsync(player.Prayer.ToDiscordMessage());
                        break;

                    case SkillType.Ranged:
                        await Context.Channel.SendMessageAsync(player.Ranged.ToDiscordMessage());
                        break;

                    case SkillType.Runecrafting:
                        await Context.Channel.SendMessageAsync(player.Runecrafting.ToDiscordMessage());
                        break;

                    case SkillType.Slayer:
                        await Context.Channel.SendMessageAsync(player.Slayer.ToDiscordMessage());
                        break;

                    case SkillType.Smithing:
                        await Context.Channel.SendMessageAsync(player.Smithing.ToDiscordMessage());
                        break;

                    case SkillType.Strength:
                        await Context.Channel.SendMessageAsync(player.Strength.ToDiscordMessage());
                        break;

                    case SkillType.Thieving:
                        await Context.Channel.SendMessageAsync(player.Thieving.ToDiscordMessage());
                        break;

                    case SkillType.Total:
                        await Context.Channel.SendMessageAsync(player.Total.ToDiscordMessage());
                        break;

                    case SkillType.All:
                        await Context.Channel.SendMessageAsync(player.ToDiscordMessage());
                        break;

                    case SkillType.Woodcutting:
                        await Context.Channel.SendMessageAsync(player.Woodcutting.ToDiscordMessage());
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