#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/18/2016 7:22 PM
// Last Revised: 11/05/2016 2:26 PM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOCard.Entities {
    #region Using

    using System.Threading.Tasks;
    using Discord;
    using Extensions;
    using Newtonsoft.Json;

    #endregion

    public class YGOCard {
        #region Public Constructors

        public YGOCard() { }

        public YGOCard(YGOCard card) {
            Attack = card.Attack;
            Attribute = card.Attribute;
            CardType = card.CardType;
            Defence = card.Defence;
            Description = card.Description;
            Id = card.Id;
            LeftScale = card.LeftScale;
            RightScale = card.RightScale;
            Level = card.Level;
            Name = card.Name;
            PendulumEffect = card.PendulumEffect;
            Type = card.Type;
        }

        #endregion Public Constructors

        #region Public Fields + Properties

        [JsonProperty("atk")]
        public uint Attack { get; set; }

        [JsonProperty("attribute")]
        public string Attribute {
            get {
                return attribute;
            }
            set {
                attribute = value?.ToTitleCase() ?? string.Empty;
            }
        }

        [JsonProperty("card")]
        public YGOCardType CardType { get; set; }

        [JsonProperty("def")]
        public uint Defence { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("scale_left")]
        public uint LeftScale { get; set; }

        [JsonProperty("level")]
        public uint Level { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pendulum_effect")]
        public string PendulumEffect {
            get {
                return pendulumEffect;
            }
            set {
                pendulumEffect = value ?? string.Empty;
            }
        }

        [JsonProperty("scale_right")]
        public uint RightScale { get; set; }

        [JsonProperty("type")]
        public string Type {
            get {
                return type;
            }
            set {
                type = value?.ToTitleCase() ?? string.Empty;
            }
        }

        #endregion Public Fields + Properties

        #region Private Fields + Properties

        string attribute;
        string pendulumEffect;
        string type;

        #endregion Private Fields + Properties

        #region Public Methods

        public string ToDiscordMessage() {
            string returnString;
            switch (CardType) {
                case YGOCardType.Effect:
                case YGOCardType.Fusion:
                case YGOCardType.Normal:
                case YGOCardType.Ritual:
                case YGOCardType.Synchro:
                case YGOCardType.Monster:
                    returnString = $"{Name} | {Attribute.ToUpper()} | {Type}\n" +
                                   $"Level: {Level} | ATK/DEF: {Attack}/{Defence}\n{Description}";
                    break;

                case YGOCardType.Xyz:
                    returnString = $"{Name} | {Attribute.ToUpper()} | {Type}\n" +
                                   $"Rank: {Level} | ATK/DEF: {Attack}/{Defence}\n{Description}";
                    break;

                case YGOCardType.P_Normal:
                case YGOCardType.P_Effect:
                    returnString = $"{Name} | {Attribute.ToUpper()} | {Type}\n" +
                                   $"Level: {Level} | Scales: {LeftScale}/{RightScale} | ATK/DEF: {Attack}/{Defence}\n" +
                                   $"Pendulum Effect:\n{PendulumEffect}\n\nMonster Effect:\n{Description}";
                    break;

                case YGOCardType.P_Synchro:
                    returnString = $"{Name} | {Attribute.ToUpper()} | {Type}\n" +
                                   $"Level: {Level} | Scales {LeftScale}/{RightScale} | ATK/DEF: {Attack}/{Defence}\n" +
                                   $"Pendulum Effect:\n{PendulumEffect}\n\nMonster Effect:\n{Description}";
                    break;

                case YGOCardType.P_Xyz:
                    returnString = $"{Name} | {Attribute.ToUpper()} | {Type}\n" +
                                   $"Rank: {Level} | Scales {LeftScale}/{RightScale} | ATK/DEF: {Attack}/{Defence}\n" +
                                   $"Pendulum Effect:\n{PendulumEffect}\n\nMonster Effect:\n{Description}";
                    break;

                case YGOCardType.Spell:
                case YGOCardType.Trap:
                    returnString = $"{Name} | {Type} {CardType}\n{Description}";
                    break;

                default:
                    return "Invalid Card Name.";
            }
            return Format.Code(returnString, "elm");
        }

        public async Task UpdateAsync() => Update(await YGOCardAPIClient.GetCardAsync(Id));

        #endregion Public Methods

        #region Private Methods

        void Update(YGOCard updatedCard) {
            if (updatedCard.Id != Id) {
                return;
            }
            Attack = updatedCard.Attack;
            Attribute = updatedCard.Attribute;
            CardType = updatedCard.CardType;
            Defence = updatedCard.Defence;
            Description = updatedCard.Description;
            LeftScale = updatedCard.LeftScale;
            Level = updatedCard.Level;
            Name = updatedCard.Name;
            PendulumEffect = updatedCard.PendulumEffect;
            RightScale = updatedCard.RightScale;
            Type = updatedCard.Type;
        }

        #endregion Private Methods

        #region Overrides of Object

        public override string ToString() => $"{Name}";

        #endregion Overrides of Object
    }
}