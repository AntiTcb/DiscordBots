#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/25/2016 6:29 AM
// Last Revised: 09/25/2016 6:55 AM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.OSRS {
    #region Using

    using Discord.Commands;

    #endregion

    [Module]
    public class RuneScapeModule {
        #region Public Structs + Classes

        [Group("stats")]
        public partial class StatsGroup { }

        #endregion Public Structs + Classes
    }
}