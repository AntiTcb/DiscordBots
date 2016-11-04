#region Header

// Description:
// 
// Solution: DiscordBots
// Project: SelfBot
// 
// Created: 10/11/2016 6:18 PM
// Last Revised: 10/30/2016 6:07 PM
// Last Revised by: Alex Gravely

#endregion

namespace SelfBot {
    #region Using

    using System.Collections.Generic;
    using BCL.Interfaces;
    using Newtonsoft.Json;

    #endregion

    public class SelfConfig : IBotConfig {
        #region Implementation of IConfig

        [JsonProperty("botToken")]
        public string BotToken { get; set; }

        [JsonProperty("evalImports")]
        public HashSet<string> EvalImports { get; set; } = new HashSet<string>();

        [JsonProperty("logChannel")]
        public ulong LogChannel { get; set; }

        #endregion Implementation of IConfig
    }
}