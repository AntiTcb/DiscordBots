#region Header

// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 10/17/2016 9:00 PM
// Last Revised: 10/18/2016 1:16 AM
// Last Revised by: Alex Gravely

#endregion Header

namespace WiseOldBot.Modules.OSRS.Entities {

    #region Using

    using RestEase;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    #endregion Using

    public static class OSRSAPIClient {

        #region Internal Fields + Properties

        internal static IOSRSAPI STATS_API =
            new RestClient(HS_ENDPOINT) { ResponseDeserializer = new AccountDeserializer() }.For<IOSRSAPI>();

        #endregion Internal Fields + Properties

        #region Private Fields + Properties

        const string HS_ENDPOINT = "http://services.runescape.com";

        #endregion Private Fields + Properties

        #region Internal Methods

        public static async Task<Account> DownloadStatsAsync(string playerName, GameMode hsType) {
            try {
                switch (hsType) {
                    case GameMode.Regular:
                        return await STATS_API.GetRegularHighScoresAsync(playerName);

                    case GameMode.Ironman:
                        return await STATS_API.GetIronmanHighScoresAsync(playerName);

                    case GameMode.Ultimate:
                        return await STATS_API.GetUltimateHighScoresAsync(playerName);

                    case GameMode.Deadman:
                        return await STATS_API.GetDeadmanHighScoresAsync(playerName);

                    case GameMode.DeadmanSeasonal:
                        return await STATS_API.GetSeasonalHighScoresAsync(playerName);

                    case GameMode.HardcoreIronman:
                        return await STATS_API.GetHardcoreIronmanHighScoresAsync(playerName);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(hsType), hsType, null);
                }
            }
            catch (ApiException e) when (e.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }
        }

        #endregion Internal Methods
    }
}