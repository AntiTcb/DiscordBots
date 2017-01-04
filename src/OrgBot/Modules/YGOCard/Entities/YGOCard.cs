// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/18/2016 7:22 PM
// Last Revised: 11/05/2016 2:26 PM
// Last Revised by: Alex Gravely

namespace OrgBot.Modules.YGOCard.Entities
{
    using BCL.Extensions;
    using Discord;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class YGOCard
    {
        public struct CardColor
        {
            public static Color Effect = new Color(255, 139, 33);
            public static Color Fusion = new Color(160, 134, 183);
            public static Color Normal = new Color(253, 230, 138);
            public static Color Ritual = new Color(157, 181, 204);
            public static Color Spell = new Color(29, 158, 116);
            public static Color Synchro = new Color(204, 204, 204);
            public static Color Trap = new Color(188, 90, 132);
            public static Color Xyz = new Color(0, 0, 0);

            internal static Color GetColor(YGOCardType type)
            {
                switch (type)
                {
                    case YGOCardType.Monster:
                    case YGOCardType.Normal:
                    case YGOCardType.P_Normal:
                        return Normal;

                    case YGOCardType.Effect:
                    case YGOCardType.P_Effect:
                        return Effect;

                    case YGOCardType.Fusion:
                        return Fusion;

                    case YGOCardType.Ritual:
                        return Ritual;

                    case YGOCardType.Spell:
                        return Spell;

                    case YGOCardType.Trap:
                        return Trap;

                    case YGOCardType.Synchro:
                    case YGOCardType.P_Synchro:
                        return Synchro;

                    case YGOCardType.Xyz:
                    case YGOCardType.P_Xyz:
                        return Xyz;

                    default:
                        return Color.Default;
                }
            }
        }

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
                pendulumEffect = value ?? "\u200B";
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
                type = value?.ToTitleCase() ?? "\u200B";
            }
        }

        public YGOCard() { }

        public YGOCard(YGOCard card)
        {
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
            ImageUrl = card.ImageUrl;
        }

        public EmbedBuilder ToDiscordEmbed()
        {
            var em = new EmbedBuilder()
                .WithTitle(Format.Bold(Name))
                .WithColor(CardColor.GetColor(CardType))
                .WithAuthor((a) =>
                    a.WithName("YGOrganization.com")
                     .WithIconUrl("https://ygorganization.com/wp-content/uploads/2014/09/cropped-TheOrgLogo.png")
                     .WithUrl("https://ygorganization.com"))
                .WithThumbnailUrl((string.IsNullOrEmpty(ImageUrl) ? "" : $"{YGORG_PIC_BASE_URI}{ImageUrl}"));

            if (CardType == YGOCardType.Spell || CardType == YGOCardType.Trap)
            {
                em.WithDescription($"{Type} {CardType}")
                        .AddField((f) =>
                            f.WithName("Effect:")
                             .WithValue(Format.Code(Description, "elm"))
                             .WithIsInline(false));
            }
            else
            {
                var isXyz = CardType == YGOCardType.Xyz || CardType == YGOCardType.P_Xyz;
                var isPend = (int)CardType >= 9;
                var descriptionFirstLine = $"{(isXyz ? "Rank:" : "Level:")} {Level} | {Attribute.ToUpper()} | {Type}";
                var descriptionSecondLine = $"\nATK / {Attack} \tDEF / {Defence}";
                var descriptionThirdLine = $"\n{CustomEmoji.LeftScale}{LeftScale} / {RightScale}{CustomEmoji.RightScale}";
                em.WithDescription(descriptionFirstLine + descriptionSecondLine + (isPend ? descriptionThirdLine : ""));

                if (isPend)
                {
                    em.AddField((f) =>
                        f.WithName("Pendulum Effect:")
                         .WithValue(PendulumEffect)
                         .WithIsInline(false));
                }
                em.AddField((f) =>
                        f.WithName(CardType == YGOCardType.Normal || CardType == YGOCardType.P_Normal ? "Flavor Text:" : "Effect:")
                            .WithValue(Description)
                            .WithIsInline(false));
            }

            return em;
        }

        public string ToDiscordMessage()
        {
            string returnString;
            switch (CardType)
            {
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

        public override string ToString() => $"{Name}";

        public async Task UpdateAsync() => Update(await YGOCardAPIClient.GetCardAsync(Id));

        const string YGORG_PIC_BASE_URI = "https://ygorganization.com/cardart/";

        string attribute;
        string pendulumEffect;
        string type;

        void Update(YGOCard updatedCard)
        {
            if (updatedCard.Id != Id)
            {
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
            ImageUrl = updatedCard.ImageUrl;
        }
    }
}