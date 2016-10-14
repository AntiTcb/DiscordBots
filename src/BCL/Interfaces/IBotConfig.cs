#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/27/2016 1:48 AM
// Last Revised: 10/13/2016 7:57 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Interfaces {
    public interface IBotConfig {
        #region Public Fields + Properties

        string BotToken { get; set; }
        ulong LogChannel { get; set; }

        #endregion Public Fields + Properties
    }
}