#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/20/2016 11:48 AM
// Last Revised: 09/22/2016 6:22 AM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.GETracker {
    #region Using

    using System;
    using System.Threading.Tasks;
    using Discord;
    using Newtonsoft.Json;

    #endregion

    public class GETrackerItem {
        #region Public Structs + Classes

        public class Wrapper {
            #region Public Fields + Properties

            [JsonProperty("data")]
            public GETrackerItem Item { get; set; }

            #endregion Public Fields + Properties
        }

        #endregion Public Structs + Classes

        #region Public Fields + Properties

        [JsonProperty("overall")]
        public int AveragePrice { get; set; }

        [JsonProperty("buyLimit")]
        public double BuyingLimit { get; set; }

        [JsonProperty("buying")]
        public double BuyingPrice { get; set; }

        [JsonProperty("buyingQuantity")]
        public int BuyingQuantity { get; set; }

        [JsonProperty("cachedUntil")]
        public DateTime CachedUntil { get; set; }

        [JsonProperty("highAlch")]
        public int HighAlchemy { get; set; }

        [JsonProperty("icon")]
        public Uri Icon { get; set; }

        [JsonProperty("itemId")]
        public int ItemID { get; set; }

        [JsonProperty("lastKnownBuyTime")]
        public DateTime LastBuy { get; set; }

        [JsonProperty("lastKnownSellTime")]
        public DateTime LastSell { get; set; }

        [JsonProperty("lowAlch")]
        public int LowAlchemy { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("selling")]
        public double SellingPrice { get; set; }

        [JsonProperty("sellingQuantity")]
        public int SellingQuantity { get; set; }

        #endregion Public Fields + Properties

        #region Public Methods

        public string ToDiscordMessage()
            =>
            $"{Format.Bold(Name)} (ID:{Format.Italics(ItemID.ToString())})\n" + $"{Format.Underline("Prices:")}\n" +
            $"\t\tBuying: {Format.Code($"{BuyingPrice:N0}gp")}\t" + $"Average: {Format.Code($"{AveragePrice:N0}gp")}\t" +
            $"Selling: {Format.Code($"{SellingPrice:N0}gp")}\n" + $"{Format.Underline("Volume:")}\n" +
            $"\t\tBuying Qty: {Format.Code($"{BuyingQuantity:N0}")}\t" +
            $"Selling Qty: {Format.Code($"{SellingQuantity:N0}")}\n" + $"{Format.Underline("Misc:")}\n" +
            $"\t\tLow Alch: {Format.Code($"{LowAlchemy:N0}gp")}\t" + $"High Alch: {Format.Code($"{HighAlchemy:N0}gp")}\t" +
            $"Buy Limit: {Format.Code($"{BuyingLimit:N0}")}\n";

        public async Task UpdateAsync() {
            if (CachedUntil > DateTime.UtcNow) {
                return;
            }
            Update((await GETrackerAPIClient.GetItemAsync(ItemID)).Item);
        }

        internal void Update(GETrackerItem updatedItem) {
            if (updatedItem.ItemID != ItemID) {
                return;
            }
            AveragePrice = updatedItem.AveragePrice;
            BuyingLimit = updatedItem.BuyingLimit;
            BuyingPrice = updatedItem.BuyingPrice;
            BuyingQuantity = updatedItem.BuyingQuantity;
            CachedUntil = updatedItem.CachedUntil;
            SellingPrice = updatedItem.SellingPrice;
            SellingQuantity = updatedItem.SellingQuantity;
        }

        #endregion Public Methods
    }
}