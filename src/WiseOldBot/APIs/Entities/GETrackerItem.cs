#region Header

// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 09/15/2016 2:13 PM
// Last Revised: 09/15/2016 2:13 PM
// Last Revised by: Alex Gravely

#endregion Header

namespace WiseOldBot.APIs.Entities
{
    using Newtonsoft.Json;

    public class GETrackerItem
    {
        #region Public Fields + Properties

        [JsonProperty("overall")]
        public int AveragePrice { get; set; }

        [JsonProperty("buyLimit")]
        public double BuyingLimit { get; set; }

        [JsonProperty("buying")]
        public double BuyingPrice { get; set; }

        [JsonProperty("buyingQuantity")]
        public int BuyingQuantity { get; set; }

        [JsonProperty("highAlch")]
        public object highAlch { get; set; }

        [JsonProperty("itemId")]
        public int ItemID { get; set; }

        [JsonProperty("lowAlch")]
        public object lowAlch { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("selling")]
        public double SellingPrice { get; set; }

        [JsonProperty("sellingQuantity")]
        public int SellingQuantity { get; set; }

        #endregion Public Fields + Properties

        public string ToDiscordMessage() {
            return "";
        }
    }
}