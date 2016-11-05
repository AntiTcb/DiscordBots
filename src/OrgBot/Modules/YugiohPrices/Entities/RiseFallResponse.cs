#region Header

// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 11/04/2016 5:57 PM
// Last Revised: 11/04/2016 5:58 PM
// Last Revised by: Alex Gravely

#endregion Header

namespace OrgBot.Modules.YugiohPrices.Entities
{
    #region Using

    using Newtonsoft.Json;
    using System.Collections.Generic;

    #endregion Using

    public class RiseFallResponse {

        #region Public Structs + Classes

        public class CardData {

            #region Public Fields + Properties

            [JsonProperty("card_number")]
            public string CardNumber { get; set; }

            [JsonProperty("card_set")]
            public string CardSet { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("price")]
            public double Price { get; set; }

            [JsonProperty("price_shift")]
            public double PriceShift { get; set; }

            // TODO: Enum
            [JsonProperty("rarity")]
            public string Rarity { get; set; }

            #endregion Public Fields + Properties
        }

        public class Data {

            #region Public Fields + Properties

            [JsonProperty("falling")]
            public List<CardData> Falling { get; set; }

            [JsonProperty("rising")]
            public List<CardData> Rising { get; set; }

            #endregion Public Fields + Properties
        }

        #endregion Public Structs + Classes
    }
}