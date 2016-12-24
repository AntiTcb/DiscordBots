// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 09/20/2016 11:48 AM
// Last Revised: 09/22/2016 6:22 AM
// Last Revised by: Alex Gravely

namespace WiseOldBot.Modules.GETracker.Entities {

    using Discord;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class GETrackerItem {

        public class Wrapper {

            [JsonProperty("data")]
            public GETrackerItem Item { get; set; }
        }

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
        public string Icon => $"https://www.ge-tracker.com/assets/images/icons/{ItemID}.gif";

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

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("url")]
        public Uri URL { get; set; }

        public EmbedBuilder ToDiscordEmbed() {
            return new EmbedBuilder()
                .WithTitle($"{Name} - {URL}")
                .WithDescription($"Item ID: {ItemID}")
                .WithUrl($"{URL}")
                .WithThumbnailUrl(Icon)
                .WithAuthor((a) =>
                    a.WithIconUrl(Icon)
                     .WithName("GE-Tracker")
                     .WithUrl(URL.ToString()))
                .WithFooter((f) =>
                    f.WithText($"Updated at: {UpdatedAt:r} || Cached Until: {CachedUntil:r}\nCurrent time: {DateTime.UtcNow:r}"))
                .AddField((f) =>
                    f.WithName("Buying")
                     .WithValue($"Price: {Format.Code($"{BuyingPrice:N0}")}{CustomEmoji.Gold}" +
                                $"\nQuantity: {Format.Code($"{BuyingQuantity:N0}")}")
                     .WithIsInline(true))
                .AddField((f) =>
                    f.WithName("Average")
                     .WithValue($"Price: {Format.Code($"{AveragePrice:N0}")}{CustomEmoji.Gold}")
                     .WithIsInline(true))
                .AddField((f) =>
                    f.WithName("Selling")
                     .WithValue($"Price: {Format.Code($"{SellingPrice:N0}")}{CustomEmoji.Gold}" +
                                $"\nQuantity: {Format.Code($"{SellingQuantity:N0}")}")
                     .WithIsInline(true))
                .AddField((f) =>
                    f.WithName("Misc")
                     .WithValue($"Low Alch: {Format.Code($"{LowAlchemy:N0}")}{CustomEmoji.Gold}\t" +
                                $"High Alch: {Format.Code($"{HighAlchemy:N0}")}{CustomEmoji.Gold}\t" +
                                $"Buy Limit: {Format.Code($"{BuyingLimit}")}"));
        }

        public string ToDiscordMessage()
            =>
            $"{Format.Bold(Name)} ({Format.Italics("ID:" + ItemID)}) <{URL}>\n" + $"{Format.Underline("Prices:")}\n" +
            $"\t\tBuying: {Format.Code($"{BuyingPrice:N0}gp")}\t" + $"Average: {Format.Code($"{AveragePrice:N0}gp")}\t" +
            $"Selling: {Format.Code($"{SellingPrice:N0}gp")}\n" + $"{Format.Underline("Volume:")}\n" +
            $"\t\tBuying Qty: {Format.Code($"{BuyingQuantity:N0}")}\t" +
            $"Selling Qty: {Format.Code($"{SellingQuantity:N0}")}\n" + $"{Format.Underline("Misc:")}\n" +
            $"\t\tLow Alch: {Format.Code($"{LowAlchemy:N0}gp")}\t" + $"High Alch: {Format.Code($"{HighAlchemy:N0}gp")}\t" +
            $"Buy Limit: {Format.Code($"{BuyingLimit:N0}")}\n";

        public void Update(GETrackerItem updatedItem) {
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

        public async Task UpdateAsync() {
            //if (CachedUntil > DateTime.UtcNow) {
            //    return;
            //}
            Update((await GETrackerAPIClient.DownloadItemAsync(ItemID)).Item);
        }
    }
}