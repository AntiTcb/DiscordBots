#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 9:51 PM
// Last Revised: 10/05/2016 7:58 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using Interfaces;
    using Newtonsoft.Json;

    #endregion

    public struct Config : IConfig {
        #region Implementation of IConfig

        [JsonProperty("botToken")]
        public string BotToken { get; set; }

        [JsonProperty("logChannel")]
        public ulong LogChannel { get; set; }

        [JsonProperty("ownerID")]
        public ulong OwnerID { get; set; }

        #endregion Implementation of IConfig
    }
}