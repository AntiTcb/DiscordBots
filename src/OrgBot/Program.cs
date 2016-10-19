#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/18/2016 4:02 PM
// Last Revised: 10/18/2016 6:46 PM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot {
    public class Program {
        #region Public Methods

        public static void Main(string[] args) => new OrgBot().StartAsync<OrgBotConfig>().GetAwaiter().GetResult();

        #endregion Public Methods
    }
}