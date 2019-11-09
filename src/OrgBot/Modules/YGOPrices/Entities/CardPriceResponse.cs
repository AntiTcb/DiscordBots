#region Header

// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 11/04/2016 5:55 PM
// Last Revised: 11/04/2016 5:58 PM
// Last Revised by: Alex Gravely

#endregion Header

namespace OrgBot.Modules.YGOPrices.Entities
{
    #region Using

    using Discord;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    #endregion Using

    public struct CardPriceData
    {
        #region Public Fields + Properties

        [JsonProperty("data")]
        public CardPriceDataDatum Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        #endregion Public Fields + Properties
    }

    public struct CardPriceDataDatum
    {
        #region Public Fields + Properties

        [JsonProperty("listings")]
        public string[] Listings { get; set; }

        [JsonProperty("prices")]
        public CardPrices Prices { get; set; }

        #endregion Public Fields + Properties
    }

    public struct CardPriceResponse
    {
        #region Public Fields + Properties

        [JsonProperty("data")]
        public List<SetData> Sets { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public EmbedBuilder ToDiscordEmbed()
        {
            var eb = new EmbedBuilder();
            foreach (var set in Sets)
            {
                string fieldName = $"{set.SetName} - {set.Rarity} - {set.PrintTag}";
                string fieldValue;
                if (set.PriceInfo.Status == "success")
                {
                    fieldValue = set.PriceInfo.Data.Prices.ToPriceString();
                }
                else if (set.PriceInfo.Status == "fail")
                {
                    fieldValue = $"{set.PriceInfo.Message}";
                }
                else
                {
                    fieldValue = $"Unknown status: {set.PriceInfo.Status} -- {set.PriceInfo.Message}";
                }
                eb.AddField((f) => f.WithName(fieldName).WithValue(fieldValue).WithIsInline(false));
            }
            return eb;
        }

        #endregion Public Fields + Properties
    }

    public struct CardPrices
    {
        #region Public Fields + Properties

        [JsonProperty("average")]
        public decimal Average { get; set; }

        [JsonProperty("high")]
        public decimal High { get; set; }

        [JsonProperty("low")]
        public decimal Low { get; set; }

        [JsonProperty("shift_180")]
        public decimal Shift180Days { get; set; }

        [JsonProperty("shift")]
        public decimal Shift1Day { get; set; }

        [JsonProperty("shift_21")]
        public decimal Shift21Days { get; set; }

        [JsonProperty("shift_30")]
        public decimal Shift30Days { get; set; }

        [JsonProperty("shift_365")]
        public decimal Shift365Days { get; set; }

        [JsonProperty("shift_3")]
        public decimal Shift3Days { get; set; }

        [JsonProperty("shift_7")]
        public decimal Shift7Days { get; set; }

        [JsonProperty("shift_90")]
        public decimal Shift90Days { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public string ToPriceString() => $"Low: {Low:C} | Avg: {Average:C} | High: {High:C}";

        #endregion Public Fields + Properties
    }

    public struct SetData
    {
        #region Public Fields + Properties

        [JsonProperty("price_data")]
        public CardPriceData PriceInfo { get; set; }

        [JsonProperty("print_tag")]
        public string PrintTag { get; set; }

        [JsonProperty("rarity")]
        public string Rarity { get; set; }

        [JsonProperty("name")]
        public string SetName { get; set; }

        #endregion Public Fields + Properties
    }
}