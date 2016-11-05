#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 11/04/2016 5:55 PM
// Last Revised: 11/04/2016 5:58 PM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YugiohPrices.Entities {
    #region Using

    using System.Collections.Generic;
    using Newtonsoft.Json;

    #endregion

    public class CardPriceResponse {
        #region Public Structs + Classes

        public class Data {
            #region Public Fields + Properties

            [JsonProperty("listings")]
            public List<object> Listings { get; set; }

            // TODO: Dict<string, int>
            [JsonProperty("prices")]
            public Prices Prices { get; set; }

            #endregion Public Fields + Properties
        }

        public class Datum {
            #region Public Fields + Properties

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("price_data")]
            public PriceData PriceData { get; set; }

            [JsonProperty("print_tag")]
            public string PrintTag { get; set; }

            // TODO: Enum
            [JsonProperty("rarity")]
            public string Rarity { get; set; }

            #endregion Public Fields + Properties
        }

        public class Example {
            #region Public Fields + Properties

            [JsonProperty("data")]
            public List<Datum> Data { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            #endregion Public Fields + Properties
        }

        public class PriceData {
            #region Public Fields + Properties

            [JsonProperty("data")]
            public Data Data { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            #endregion Public Fields + Properties
        }

        public class Prices {
            #region Public Fields + Properties

            [JsonProperty("average")]
            public double Average { get; set; }

            [JsonProperty("high")]
            public double High { get; set; }

            [JsonProperty("low")]
            public double Low { get; set; }

            [JsonProperty("shift")]
            public double Shift { get; set; }

            [JsonProperty("shift_180")]
            public double Shift180 { get; set; }

            [JsonProperty("shift_21")]
            public double Shift21 { get; set; }

            [JsonProperty("shift_3")]
            public double Shift3 { get; set; }

            [JsonProperty("shift_30")]
            public double Shift30 { get; set; }

            [JsonProperty("shift_365")]
            public double Shift365 { get; set; }

            [JsonProperty("shift_7")]
            public double Shift7 { get; set; }

            [JsonProperty("shift_90")]
            public double Shift90 { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            #endregion Public Fields + Properties
        }

        #endregion Public Structs + Classes
    }
}