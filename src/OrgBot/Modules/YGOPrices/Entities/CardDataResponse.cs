namespace OrgBot.Modules.YGOPrices.Entities
{
    using BCL.Extensions;
    using Newtonsoft.Json;

    public class CardData
    {
        #region Public Fields + Properties

        [JsonProperty("atk")]
        public int? Attack { get; set; }

        [JsonProperty("family")]
        public string Attribute { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("def")]
        public int? Defense { get; set; }

        [JsonProperty("level")]
        public int? LevelOrRank { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("property")]
        public string Property { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type {
            get { return _type; }
            set { _type = value?.ToTitleCase(); }
        }

        #endregion Public Fields + Properties

        #region Private Fields + Properties

        string _type;

        #endregion Private Fields + Properties
    }

    public class CardDataResponse
    {
        #region Public Fields + Properties

        [JsonProperty("data")]
        public CardData CardInfo { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        #endregion Public Fields + Properties
    }
}