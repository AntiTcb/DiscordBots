#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/05/2016 7:20 PM
// Last Revised: 10/05/2016 7:59 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using Interfaces;
    using Newtonsoft.Json;

    #endregion

    public struct ServerConfig : IServerConfig {

        public static char DefaultPrefix = ';';

        #region Implementation of IServerConfig

        [JsonProperty("commandPrefix")]
        public char CommandPrefix { get; set; }

        #endregion Implementation of IServerConfig

        public ServerConfig(char commandPrefix) {
            CommandPrefix = commandPrefix;
        }
    }
}