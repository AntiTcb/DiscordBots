﻿#region Header

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
    using Extensions;
    using Newtonsoft.Json;

    #endregion

    public class YGOCard {
        #region Public Fields + Properties

        [JsonProperty("id")]
        public uint Id { get; }

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
                        $"```xl\n{Name} \t\t {Attribute.ToUpper()} \t {Type}\n" +
                        $"Level: {Level} \t ATK/DEF: {Attack}/{Defence}\n{Description}\n```";

                case YGOCardType.Xyz:
                    return
                        $"```xl\n{Name} \t\t {Attribute.ToUpper()} \t {Type}\n" +
                        $"Rank: {Level} \t ATK/DEF: {Attack}/{Defence}\n{Description}\n```";

                case YGOCardType.P_Normal:
                case YGOCardType.P_Effect:
                    return
                        $"```xl\n{Name} \t\t {Attribute.ToUpper()} \t {Type}\n" +
                        $"Level: {Level} \t Scales: {LeftScale}/{RightScale} \t ATK/DEF: {Attack}/{Defence}\n" +
                        $"Pendulum Effect:\n{PendulumEffect}\n\nMonster Effect:\n{Description}\n```";

                case YGOCardType.P_Synchro:
                    return $"```xl\n{Name} \t\t {Attribute.ToUpper()} \t {Type}\n" +
                           $"Level: {Level} \t Scales {LeftScale}/{RightScale} \t ATK/DEF: {Attack}/{Defence}\n" +
                           $"Pendulum Effect: \n{PendulumEffect}\n\nMonster Effect:\n{Description}\n```";

                case YGOCardType.P_Xyz:
                    return
                        $"```xl\n**{Name} \t\t {Attribute.ToUpper()} \t {Type}\n" +
                        $"Rank: {Level} \t Scales: {LeftScale}/{RightScale} \t ATK/DEF: {Attack}/{Defence}\n" +
                        $"Pendulum Effect:\n{PendulumEffect}\n\nMonster Effect:\n{Description}\n```";

                case YGOCardType.Spell:
                case YGOCardType.Trap:
                    return
                        $"```xl\n{Name} \t\t {Type} {CardType}\n{Description}\n```";

                default:
                    return "Invalid Card Name";
            };
        }
    }
}