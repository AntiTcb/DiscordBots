﻿#region Header

// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/18/2016 7:22 PM
// Last Revised: 11/05/2016 2:26 PM
// Last Revised by: Alex Gravely

#endregion Header

namespace OrgBot.Modules.YGOCard.Entities {

    #region Using

    using Discord;
    using BCL.Extensions;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    #endregion Using

    public class YGOCard {

        #region Public Constructors

        internal struct CardColor {

            #region Private Fields + Properties

            static Color _effect = new Color(255, 139, 33);
            static Color _fusion = new Color(160, 134, 183);
            static Color _normal = new Color(253, 230, 138);
            static Color _ritual = new Color(157, 181, 204);
            static Color _spell = new Color(29, 158, 116);
            static Color _synchro = new Color(204, 204, 204);
            static Color _trap = new Color(188, 90, 132);
            static Color _xyz = new Color(0, 0, 0);

            #endregion Private Fields + Properties

            #region Internal Methods

            internal static Color GetColor(YGOCardType type) {
                switch (type) {
                    case YGOCardType.Monster:
                    case YGOCardType.Normal:
                    case YGOCardType.P_Normal:
                        return _normal;

                    case YGOCardType.Effect:
                    case YGOCardType.P_Effect:
                        return _effect;

                    case YGOCardType.Fusion:
                        return _fusion;

                    case YGOCardType.Ritual:
                        return _ritual;

                    case YGOCardType.Spell:
                        return _spell;

                    case YGOCardType.Trap:
                        return _trap;

                    case YGOCardType.Synchro:
                    case YGOCardType.P_Synchro:
                        return _synchro;

                    case YGOCardType.Xyz:
                    case YGOCardType.P_Xyz:
                        return _xyz;

                    default:
                        return Color.Default;
                }
            }

            #endregion Internal Methods
        }

        public YGOCard() {
        }

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

        [JsonProperty("picture")]
        public string ImageUrl { get; set; }

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

        public EmbedBuilder ToDiscordEmbed() {
            var em = new EmbedBuilder()
                .WithTitle(Format.Bold(Name))
                .WithColor(CardColor.GetColor(CardType));

            if (CardType == YGOCardType.Spell || CardType == YGOCardType.Trap) {
                em.WithDescription($"{Type} {CardType}")
                        .AddField((f) =>
                            f.WithName("Effect:")
                             .WithValue(Format.Code(Description, "elm"))
                             .WithIsInline(false));
            }
            else {
                em.WithDescription($"{Type} | {Attribute.ToUpper()}");
                var isPend = (int)CardType >= 9;
                if (isPend) {
                    em.AddField((f) =>
                        f.WithName("Pendulum Effect:")
                         .WithValue(Format.Code(PendulumEffect, "elm"))
                         .WithIsInline(false));
                }
                em.AddField((f) =>
                        f.WithName(CardType == YGOCardType.Normal ? "Flavor Text:" : "Effect:")
                            .WithValue(CardType == YGOCardType.Normal ? Description : Format.Code(Description, "elm"))
                            .WithIsInline(false));
                if (isPend) {
                    em.AddField((f) =>
                        f.WithName("Scales")
                         .WithValue($"{LeftScale} / {RightScale}")
                         .WithIsInline(true));
                }
                em.AddField((f) =>
                    f.WithName(CardType == YGOCardType.Xyz || CardType == YGOCardType.P_Xyz ? "Rank:" : "Level:")
                     .WithValue($"{Level}")
                     .WithIsInline(true))
                .AddField((f) =>
                    f.WithName("ATK/DEF:")
                     .WithValue($"{Attack}/{Defence}")
                     .WithIsInline(true));
            }

            return em;
        }

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