namespace WiseOldBot.Modules.OSRS.Entities
{
    using RestEase;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public static class OSRSAPIClient
    {
        internal static IOSRSAPI STATS_API =
            new RestClient(HS_ENDPOINT) { ResponseDeserializer = new AccountDeserializer() }.For<IOSRSAPI>();

        const string HS_ENDPOINT = "http://services.runescape.com";

        public static async Task<Account> DownloadStatsAsync(string playerName, GameMode hsType)
        {
            try
            {
                switch (hsType)
                {
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
            catch (ApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}