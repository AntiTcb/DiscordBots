#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/15/2016 10:00 PM
// Last Revised: 09/16/2016 11:25 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.APIs {
    #region Using

    using System.Threading.Tasks;
    using Entities;
    using RestEase;

    #endregion

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