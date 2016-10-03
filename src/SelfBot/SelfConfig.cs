using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SelfBot
{
    using BCL.Interfaces;
    using Newtonsoft.Json;

    public class SelfConfig : IConfig
    {
        #region Implementation of IConfig
        [JsonProperty("botToken")]
        public string BotToken { get; set; }
        [JsonProperty("prefix")]
        public char CommandPrefix { get; set; }

        #endregion
    }
}
