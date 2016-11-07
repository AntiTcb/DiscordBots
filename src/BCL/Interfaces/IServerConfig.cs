#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/05/2016 7:12 PM
// Last Revised: 11/06/2016 2:15 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Interfaces {
    #region Using

    using System.Collections.Generic;

    #endregion

    public interface IServerConfig {
        #region Public Fields + Properties

        string CommandPrefix { get; set; }
        Dictionary<string, string> Tags { get; set; }

        #endregion Public Fields + Properties
    }
}