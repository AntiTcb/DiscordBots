#region Header
// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/25/2016 6:37 AM
// Last Revised: 09/25/2016 6:37 AM
// Last Revised by: Alex Gravely
#endregion
namespace WiseOldBot.Modules.OSRS.Entities {
    using System.Threading.Tasks;
    using RestEase;

    public interface IOSRSAPI {
        [Get("m=hiscore_oldschool_deadman/index_lite.ws")]
        Task<Account> GetDeadmanHighScoresAsync([Query("player")] string playerName);

        [Get("m=hiscore_oldschool_ironman/index_lite.ws")]
        Task<Account> GetIronmanHighScoresAsync([Query("player")] string playerName);

        [Get("m=hiscore_oldschool/index_lite.ws")]
        Task<Account> GetRegularHighScoresAsync([Query("player")] string playerName);

        [Get("m=hiscore_oldschool_seasonal/index_lite.ws")]
        Task<Account> GetSeasonalHighScoresAsync([Query("player")] string playerName);

        [Get("m=hiscore_oldschool_ultimate/index_lite.ws")]
        Task<Account> GetUltimateHighScoresAsync([Query("player")] string playerName);

        [Get("m=hiscore_oldschool_hardcore_ironman/index_lite.ws")]
        Task<Account> GetHardcoreIronmanHighScoresAsync([Query("player")]string playerName);
    }
}