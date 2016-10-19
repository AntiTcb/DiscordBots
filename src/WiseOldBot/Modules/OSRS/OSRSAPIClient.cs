#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 10/17/2016 9:00 PM
// Last Revised: 10/18/2016 1:16 AM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.Modules.OSRS {
    #region Using

    using System;
    using System.Threading.Tasks;
    using RestEase;

    #endregion

    public static class OSRSAPIClient {
        #region Internal Fields + Properties

        internal static IOSRSApi STATS_API =
            new RestClient(HS_ENDPOINT) { ResponseDeserializer = new AccountDeserializer() }.For<IOSRSApi>();

        #endregion Internal Fields + Properties

        #region Private Fields + Properties

        const string HS_ENDPOINT = "http://services.runescape.com";

        #endregion Private Fields + Properties

        #region Internal Methods

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

        #endregion Internal Methods
    }
}