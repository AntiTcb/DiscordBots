using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SelfBot
{
    using BCL.Interfaces;
    using Newtonsoft.Json;

    public class SelfBotConfig : IBotConfig
    {
        #region Implementation of IBotConfig
        [JsonProperty("botToken")]
        public string BotToken { get; set; }

        [JsonProperty("prefix")]
        public char CommandPrefix { get; set; } = ';';

        public ulong LogChannel { get; set; }

        [JsonProperty("evalImports")]
        public HashSet<string> EvalImports { get; set; } = new HashSet<string>();

        #endregion
    }
}
