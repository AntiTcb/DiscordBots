namespace WiseOldBot.Modules.GETracker.Entities
{
    using Discord;
    using Humanizer;
    using Humanizer.Localisation;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class GETrackerItem
    {
        public class Wrapper
        {
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
        public string Icon => $"https://www.ge-tracker.com/assets/images/icons/{ItemId}.gif";
        [JsonProperty("itemId")]
        public int ItemId { get; set; }
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
        public Uri Url { get; set; }

        public string OSRSExchange => $"http://services.runescape.com/m=itemdb_oldschool/Runescape/viewitem?obj={ItemId}";
        public string RSBExchange => $"https://rsbuddy.com/exchange?id={ItemId}&";
        public string Wikia => $"http://os.rs.wiki/w/{Name.Replace(" ", "_")}";
        public string RSIconUrl => $"http://services.runescape.com/m=itemdb_oldschool/1509361245218_obj_big.gif?id={ItemId}";
        public string RSIconSpriteUrl => $"http://services.runescape.com/m=itemdb_oldschool/1509361245218_obj_sprite.gif?id={ItemId}";

        public override bool Equals(object item) => ItemId == (item as GETrackerItem)?.ItemId;
        public override int GetHashCode() => ItemId;

        public Embed ToDiscordEmbed()
        {
            var eb = new EmbedBuilder()
            {
                Title = $"{Name} (ID: {ItemId})",
                Description = $"[GE-Tracker]({Url}) | [OSRS Exchange]({OSRSExchange}) | [RSB Exchange]({RSBExchange}) | [2007 Wiki]({Wikia})\nUpdated: {(DateTime.UtcNow - UpdatedAt.Value).Humanize(3, minUnit: TimeUnit.Second, maxUnit: TimeUnit.Minute)} ago.",
                Url = Url.ToString(),
                ThumbnailUrl = RSIconUrl
            }
            .WithAuthor("GE-Tracker", "https://cdn.discordapp.com/avatars/372857710229848064/f997ce46943f41a18bb089f6b41954af.png?size=128", Url.ToString())
            .WithFooter($"Cached for the next: {(CachedUntil - DateTime.UtcNow).Humanize(3, minUnit: TimeUnit.Second, maxUnit: TimeUnit.Minute)}")
            .AddField("Buying", $"Price: {$"{BuyingPrice:N0}"}{CustomEmoji.Gold}" + $"\nQuantity: {$"{BuyingQuantity:N0}"}", true)
            .AddField("Average", $"Price: {$"{AveragePrice:N0}"}{CustomEmoji.Gold}", true)
            .AddField("Selling", $"Price: {$"{SellingPrice:N0}"}{CustomEmoji.Gold}" + $"\nQuantity: {$"{SellingQuantity:N0}"}", true);

            if (ApproximateProfit.HasValue)
                eb.AddField("Approx. Profit", $"{$"{ApproximateProfit:N0}"}{CustomEmoji.Gold}", true);

            eb.AddField("Alch", $"Low: {$"{LowAlchemy:N0}"}{CustomEmoji.Gold}\nHigh: {$"{HighAlchemy:N0}"}{CustomEmoji.Gold}", true)
              .AddField("Buy Limit", BuyingLimit.ToString("N0"), true);

            return eb.Build();
        }

        public string ToDiscordMessage()
            =>
            $"{Format.Bold(Name)} ({Format.Italics("ID:" + ItemId)}) <{Url}>\n" + $"{Format.Underline("Prices:")}\n" +
            $"\t\tBuying: {Format.Code($"{BuyingPrice:N0}gp")}\t" + $"Average: {Format.Code($"{AveragePrice:N0}gp")}\t" +
            $"Selling: {Format.Code($"{SellingPrice:N0}gp")}\n" + $"{Format.Underline("Volume:")}\n" +
            $"\t\tBuying Qty: {Format.Code($"{BuyingQuantity:N0}")}\t" +
            $"Selling Qty: {Format.Code($"{SellingQuantity:N0}")}\n" + $"{Format.Underline("Misc:")}\n" +
            $"\t\tLow Alch: {Format.Code($"{LowAlchemy:N0}gp")}\t" + $"High Alch: {Format.Code($"{HighAlchemy:N0}gp")}\t" +
            $"Buy Limit: {Format.Code($"{BuyingLimit:N0}")}\n";

        public void Update(GETrackerItem updatedItem)
        {
            if (updatedItem.ItemId != ItemId)
                return;

            AveragePrice = updatedItem.AveragePrice;
            BuyingLimit = updatedItem.BuyingLimit;
            BuyingPrice = updatedItem.BuyingPrice;
            BuyingQuantity = updatedItem.BuyingQuantity;
            CachedUntil = updatedItem.CachedUntil;
            SellingPrice = updatedItem.SellingPrice;
            SellingQuantity = updatedItem.SellingQuantity;
            ApproximateProfit = updatedItem.ApproximateProfit;
            UpdatedAt = updatedItem.UpdatedAt;
        }

        public async Task UpdateAsync()
        {
            var model = await GETrackerAPIClient.DownloadItemAsync(ItemId);
            Update(model.Item);
        }
    }
}