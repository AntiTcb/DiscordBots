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
    using APIs;
    using APIs.Entities;
    using Discord;
    using Discord.Commands;
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
            public async Task GetAllStatsAsync(IUserMessage msg, HighScoreType hsType, [Remainder] string playerName) {
                var player = await DownloadStatsAsync(playerName, hsType);
                await msg.Channel.SendMessageAsync(player.ToDiscordMessage());
            }

            [Command("att"), Alias("attack")]
            public async Task GetAttackAsync(IUserMessage msg, HighScoreType hsType, [Remainder] string playerName) {
                var player = await DownloadStatsAsync(playerName, hsType);
                await msg.Channel.SendMessageAsync(player.Attack.ToDiscordMessage());
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
}