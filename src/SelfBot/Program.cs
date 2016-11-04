#region Header

// Description:
// 
// Solution: DiscordBots
// Project: SelfBot
// 
// Created: 10/11/2016 6:18 PM
// Last Revised: 10/30/2016 6:06 PM
// Last Revised by: Alex Gravely

#endregion

namespace SelfBot {
    public class Program {
        #region Public Methods

        public static void Main(string[] args) => new Bot().StartAsync<SelfConfig>().GetAwaiter().GetResult();

        #endregion Public Methods
    }
}