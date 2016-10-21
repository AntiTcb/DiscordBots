#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/18/2016 7:22 PM
// Last Revised: 10/18/2016 7:24 PM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOCard.Entities {
    #region Using

    using System.Globalization;
    using System.Threading.Tasks;
    using Discord;
    using Extensions;
    using Newtonsoft.Json;

    #endregion

    public class YGOCard {
        #region Public Fields + Properties

        [JsonProperty("id")]
        public uint Id { get; set; }

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

        public async Task UpdateAsync() => Update(await YGOCardAPIClient.GetCardAsync(Id));

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

        #region Overrides of Object

        public override string ToString() => $"{Name}";

    #endregion

        public string ToDiscordMessage() {
            switch (CardType)
            {
                case YGOCardType.Effect:
                case YGOCardType.Fusion:
                case YGOCardType.Normal:
                case YGOCardType.Ritual:
                case YGOCardType.Synchro:
                case YGOCardType.Monster:
                    return
                        $"{Format.Code($"{Name} | {Attribute.ToUpper()} | {Type}", "elm")}" +
                        $"{Format.Code($"Level: {Level} | ATK/DEF: {Attack}/{Defence}\n{Description}\n", "elm")}";

                case YGOCardType.Xyz:
                    return
                        $"{Format.Code($"{Name} | {Attribute.ToUpper()} | {Type}", "elm")}"+
                        $"{Format.Code($"Rank: {Level} | ATK/DEF: {Attack}/{Defence}\n{Description}\n", "elm")}";

                case YGOCardType.P_Normal:
                case YGOCardType.P_Effect:
                    return $"{Format.Code($"{Name} | {Attribute.ToUpper()} | {Type}", "elm")}" +
                           $"{Format.Code($"Level: {Level} | Scales: {LeftScale}/{RightScale} | ATK/DEF: {Attack}/{Defence}", "elm")}" +
                           $"{Format.Code($"Pendulum Effect:\n{PendulumEffect}\n\nMonster Effect:{Description}", "elm")}";

                case YGOCardType.P_Synchro:
                    return $"{Format.Code($"{Name} | {Attribute.ToUpper()} | {Type}", "elm")}" +
                           $"{Format.Code($"Level: {Level} | Scales {LeftScale}/{RightScale} | ATK/DEF: {Attack}/{Defence}", "elm")}" +
                           $"{Format.Code($"Pendulum Effect:\n{PendulumEffect}\n\nMonster Effect:{Description}", "elm")}";

                case YGOCardType.P_Xyz:
                    return
                        $"{Format.Code($"{Name} | {Attribute.ToUpper()} | {Type}", "elm")}" +
                        $"{Format.Code($"Rank: {Level} | Scales {LeftScale}/{RightScale} | ATK/DEF: {Attack}/{Defence}", "elm")}" +
                        $"{Format.Code($"Pendulum Effect:\n{PendulumEffect}\n\nMonster Effect:{Description}", "elm")}";

                case YGOCardType.Spell:
                case YGOCardType.Trap:
                    return $"{Format.Code($"{Name} | {Type} {CardType}\n{Description}","elm")}";

                default:
                    return "Invalid Card Name.";
            };
        }
    }
}