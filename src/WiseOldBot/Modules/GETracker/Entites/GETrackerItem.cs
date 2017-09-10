// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 09/20/2016 11:48 AM
// Last Revised: 09/22/2016 6:22 AM
// Last Revised by: Alex Gravely

namespace WiseOldBot.Modules.GETracker.Entities {

    using Humanizer;
    using Humanizer.Localisation;
    using Discord;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class GETrackerItem {

        public class Wrapper {

            [JsonProperty("data")]
            public GETrackerItem Item { get; set; }
        }

        [JsonProperty("approxProfit")]
        public int? ApproximateProfit { get; set; }
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
        public DateTime? LastBuy { get; set; }   
        [JsonProperty("lastKnownSellTime")]
        public DateTime? LastSell { get; set; }   
        [JsonProperty("lowAlch")]
        public int LowAlchemy { get; set; }  
        [JsonProperty("name")]
        public string Name { get; set; }  
        [JsonProperty("selling")]
        public double SellingPrice { get; set; }    
        [JsonProperty("sellingQuantity")]
        public int SellingQuantity { get; set; }  
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }    
        [JsonProperty("url")]
        public Uri URL { get; set; }

        public Embed ToDiscordEmbed() {
            var eb = new EmbedBuilder()
            {
                Title = $"{Name} (ID: {ItemID})",
                Description = $"Updated: {(DateTime.UtcNow - UpdatedAt.Value).Humanize(1, minUnit: TimeUnit.Second, maxUnit: TimeUnit.Minute)} ago",
                Url = URL.ToString(),
                ThumbnailUrl = Icon
            }
            .WithAuthor("GE-Tracker", Icon, URL.ToString())
            .WithFooter($"Cached for the next: {(CachedUntil - DateTime.UtcNow).Humanize()}")
            //.WithFooter($"Updated at: {(UpdatedAt?.ToString() ?? "Never"):r} || Cached Until: {CachedUntil:r}\nCurrent time: {DateTime.UtcNow:r}")
            .AddField("Buying", $"Price: {$"{BuyingPrice:N0}"}{CustomEmoji.Gold}" + $"\nQuantity: {$"{BuyingQuantity:N0}"}", true)
            .AddField("Average", $"Price: {$"{AveragePrice:N0}"}{CustomEmoji.Gold}", true)
            .AddField("Selling", $"Price: {$"{SellingPrice:N0}"}{CustomEmoji.Gold}" + $"\nQuantity: {$"{SellingQuantity:N0}"}", true);
            if (ApproximateProfit.HasValue)
                eb.AddField("Approx. Profit", $"{$"{ApproximateProfit:N0}"}{CustomEmoji.Gold}", true);
            eb.AddField("Misc", $"Low Alch: {$"{LowAlchemy:N0}"}{CustomEmoji.Gold}\t" +
                                $"High Alch: {$"{HighAlchemy:N0}"}{CustomEmoji.Gold}\t" +
                                $"Buy Limit: {$"{BuyingLimit}"}");

            return eb.Build();
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
            if (updatedItem.ItemID != ItemID) 
                return;
           
            AveragePrice = updatedItem.AveragePrice;
            BuyingLimit = updatedItem.BuyingLimit;
            BuyingPrice = updatedItem.BuyingPrice;
            BuyingQuantity = updatedItem.BuyingQuantity;
            CachedUntil = updatedItem.CachedUntil;
            SellingPrice = updatedItem.SellingPrice;
            SellingQuantity = updatedItem.SellingQuantity;
            ApproximateProfit = updatedItem.ApproximateProfit;
        }

        public async Task UpdateAsync() {
            Update((await GETrackerAPIClient.DownloadItemAsync(ItemID)).Item);
        }

        public override bool Equals(object item) => ItemID == (item as GETrackerItem)?.ItemID;

        public override int GetHashCode() => ItemID;
    }
}