#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BotTests
// 
// Created: 11/01/2016 6:59 PM
// Last Revised: 11/01/2016 7:24 PM
// Last Revised by: Alex Gravely

#endregion

namespace BotTests {
    #region Using

    using WiseOldBot.Modules.OSRS;
    using Xunit;

    #endregion

    public class HighScoresTests {
        #region Public Methods

        [Fact]
        public void PullStatsFromRegular() {
            var player = OSRSAPIClient.DownloadStatsAsync("anti-tcb", HighScoreType.Regular);
            Assert.NotNull(player);
        }

        #endregion Public Methods
    }
}