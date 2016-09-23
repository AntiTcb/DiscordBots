#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/16/2016 3:56 PM
// Last Revised: 09/17/2016 12:08 AM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.Modules {
    #region Using

    using System;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Entities;
    using RestEase;

    #endregion

    [Module]
    public class RuneScapeModule {
        #region Public Structs + Classes

        [Group("stats")]
        public class StatsModule {
            #region Internal Fields + Properties

            internal static IRuneScapeAPI STATS_API = RestClient.For<IRuneScapeAPI>(HS_ENDPOINT);

            #endregion Internal Fields + Properties

            #region Public Methods

            [Command("stats")]
            public async Task GetAllStatsAsync(IUserMessage msg,
                HighScoreType hsType,
                Skill skill,
                [Remainder] string playerName) {
                var player = await DownloadStatsAsync(playerName, hsType);

                switch (skill) {
                    case Skill.Agility:
                        await msg.Channel.SendMessageAsync(player.Agility.ToDiscordMessage());
                        break;
                    case Skill.Attack:
                        await msg.Channel.SendMessageAsync(player.Attack.ToDiscordMessage());
                        break;
                    case Skill.Construction:
                        await msg.Channel.SendMessageAsync(player.Construction.ToDiscordMessage());
                        break;
                    case Skill.Cooking:
                        await msg.Channel.SendMessageAsync(player.Cooking.ToDiscordMessage());
                        break;
                    case Skill.Crafting:
                        await msg.Channel.SendMessageAsync(player.Crafting.ToDiscordMessage());
                        break;
                    case Skill.Defense:
                        await msg.Channel.SendMessageAsync(player.Defense.ToDiscordMessage());
                        break;
                    case Skill.Farming:
                        await msg.Channel.SendMessageAsync(player.Farming.ToDiscordMessage());
                        break;
                    case Skill.Firemaking:
                        await msg.Channel.SendMessageAsync(player.Firemaking.ToDiscordMessage());
                        break;
                    case Skill.Fishing:
                        await msg.Channel.SendMessageAsync(player.Fishing.ToDiscordMessage());
                        break;
                    case Skill.Fletching:
                        await msg.Channel.SendMessageAsync(player.Fletching.ToDiscordMessage());
                        break;
                    case Skill.Herblore:
                        await msg.Channel.SendMessageAsync(player.Herblore.ToDiscordMessage());
                        break;
                    case Skill.Hitpoints:
                        await msg.Channel.SendMessageAsync(player.Hitpoints.ToDiscordMessage());
                        break;
                    case Skill.Hunter:
                        await msg.Channel.SendMessageAsync(player.Hunter.ToDiscordMessage());
                        break;
                    case Skill.Magic:
                        await msg.Channel.SendMessageAsync(player.Magic.ToDiscordMessage());
                        break;
                    case Skill.Mining:
                        await msg.Channel.SendMessageAsync(player.Mining.ToDiscordMessage());
                        break;
                    case Skill.Prayer:
                        await msg.Channel.SendMessageAsync(player.Prayer.ToDiscordMessage());
                        break;
                    case Skill.Ranged:
                        await msg.Channel.SendMessageAsync(player.Ranged.ToDiscordMessage());
                        break;
                    case Skill.Runecrafting:
                        await msg.Channel.SendMessageAsync(player.Runecrafting.ToDiscordMessage());
                        break;
                    case Skill.Slayer:
                        await msg.Channel.SendMessageAsync(player.Slayer.ToDiscordMessage());
                        break;
                    case Skill.Smithing:
                        await msg.Channel.SendMessageAsync(player.Smithing.ToDiscordMessage());
                        break;
                    case Skill.Strength:
                        await msg.Channel.SendMessageAsync(player.Strength.ToDiscordMessage());
                        break;
                    case Skill.Thieving:
                        await msg.Channel.SendMessageAsync(player.Thieving.ToDiscordMessage());
                        break;
                    case Skill.Total:
                        await msg.Channel.SendMessageAsync(player.Total.ToDiscordMessage());
                        break;
                    case Skill.All:
                        await msg.Channel.SendMessageAsync(player.ToDiscordMessage());
                        break;
                    case Skill.Woodcutting:
                        await msg.Channel.SendMessageAsync(player.Woodcutting.ToDiscordMessage());
                        break;
                    case Skill.Combat:
                        break;
                    case Skill.NonCombat:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(skill), skill, null);
                }
            }

            internal static async Task<RuneScapeStats> DownloadStatsAsync(string playerName, HighScoreType hsType) {
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
        }

        #endregion Public Structs + Classes

        #region Private Fields + Properties

        const string HS_ENDPOINT = "http://services.runescape.com/";

        #endregion Private Fields + Properties
    }

    public interface IRuneScapeAPI {
        #region Public Methods

        [Get("m=hiscore_oldschool_deadman/index_lite.ws")]
        Task<RuneScapeStats> GetDeadmanHighScoresAsync([Query("player")] string playerName);

        [Get("m=hiscore_oldschool_ironman/index_lite.ws")]
        Task<RuneScapeStats> GetIronmanHighScoresAsync([Query("player")] string playerName);

        [Get("m=hiscore_oldschool/index_lite.ws")]
        Task<RuneScapeStats> GetRegularHighScoresAsync([Query("player")] string playerName);

        [Get("m=hiscore_oldschool_seasonal/index_lite.ws")]
        Task<RuneScapeStats> GetSeasonalHighScoresAsync([Query("player")] string playerName);

        [Get("m=hiscore_oldschool_ultimate/index_lite.ws")]
        Task<RuneScapeStats> GetUltimateHighScoresAsync([Query("player")] string playerName);

        #endregion Public Methods
    }
}