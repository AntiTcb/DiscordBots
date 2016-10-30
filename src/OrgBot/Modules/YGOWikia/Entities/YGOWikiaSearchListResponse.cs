#region Header
// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/30/2016 1:07 PM
// Last Revised: 10/30/2016 1:07 PM
// Last Revised by: Alex Gravely
#endregion
namespace OrgBot.Modules.YGOWikia.Entities {
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class YGOWikiaSearchListResponse {
        [JsonProperty("items")]
        public List<YGOWikiaSearchListItem> Items { get; set; }
    }

    public class YGOWikiaSearchListItem {
        [JsonProperty("url")]
        public string URL { get; set; }
    }
}