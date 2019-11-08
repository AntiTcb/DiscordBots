using System;
using System.Linq;
using System.Text;
using BCL.Extensions;
using Discord;
using Newtonsoft.Json;
using static Discord.Format;
using WikiFormat = OrgBot.Util.Format;

namespace OrgBot
{
    public class YugipediaCard
    {
        private string _attribute;
        private static readonly string ZERO_WIDTH_SPACE = "\u200B";

        public string Attribute
        {
            get => _attribute;
            set => _attribute = value?.ToTitleCase() ?? string.Empty;
        }
        [JsonProperty("atk")]
        public int? Attack { get; set; }
        [JsonProperty("card_type")]
        public string CardType { get; set; }
        [JsonProperty("database_id")]
        public string DatabaseId { get; set; }
        [JsonProperty("def")]
        public int? Defense { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("link_arrows")]
        public string LinkArrows { get; set; }
        [JsonProperty("lore")]
        public string DescriptionRaw { get; set; }
        [JsonProperty("level")]
        public int Level { get; set; }
        [JsonProperty("materials")]
        public string Materials { get; set; }
        [JsonProperty("en_name")]
        public string Name { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("pendulum_effect")]
        public string PendulumEffectRaw { get; set; }
        [JsonProperty("pendulum_scale")]
        public int? PendulumScale { get; set; }
        [JsonProperty("property")]
        public string Property { get; set; }
        [JsonProperty("rank")]
        public int? Rank { get; set; }
        [JsonProperty("tcg_status")]
        public string TcgStatus { get; set; }
        [JsonProperty("type")]
        public string Type1 { get; set; }
        [JsonProperty("type2")]
        public string Type2 { get; set; }
        [JsonProperty("type3")]
        public string Type3 { get; set; }
        [JsonProperty("type4")]
        public string Type4 { get; set; }

        private string[] Types => new[] { Type1, Type2, Type3, Type4 };

        public int? LinkRating => LinkArrows?.Split(',')?.Count();
        public string Description => WikiFormat.ResolveMarkup(DescriptionRaw);
        public string ImageUrl => $"https://yugipedia.com/wiki/File:{Image}";
        public string PendulumEffect => WikiFormat.ResolveMarkup(PendulumEffectRaw);
        public string Type => string.Join("/", Types.Where(t => !string.IsNullOrEmpty(t)));

        public Embed ToEmbed()
        {
            bool isPend = Type.Contains("Pendulum", StringComparison.OrdinalIgnoreCase);
            bool isXyz = Type.Contains("Xyz", StringComparison.OrdinalIgnoreCase);
            bool isLink = Type.Contains("Link", StringComparison.OrdinalIgnoreCase);
            bool isSpellOrTrap = !string.IsNullOrWhiteSpace(CardType);

            var descriptionBuilder = new StringBuilder(ZERO_WIDTH_SPACE);

            if (isSpellOrTrap)
            {
                descriptionBuilder.AppendLine($"{Property} {CardType}");
            }
            else
            {
                if (isXyz)
                    descriptionBuilder.Append($"Rank {Rank} | ");
                else if (!isLink)
                    descriptionBuilder.Append($"Level {Level} | ");

                descriptionBuilder.AppendLine($"{Attribute?.Trim()} | {Type}");

                descriptionBuilder.Append($"{Bold("ATK")} / {Attack} \t ");

                if (isLink)
                    descriptionBuilder.AppendLine($"{Bold("LINK")} / {LinkRating}\n{Bold("Link Arrows:")} {LinkArrows}");
                else
                    descriptionBuilder.AppendLine($"{Bold("DEF")} / {Defense}");

                if (isPend)
                {
                    descriptionBuilder.AppendLine($"{CustomEmoji.LeftScale}{PendulumScale} / {CustomEmoji.RightScale}");
                    descriptionBuilder.AppendLine($"{Bold("Pendulum Effect:")}\n{PendulumEffect}\n");
                }
            }

            descriptionBuilder.AppendLine($"{Bold("Description")}\n{Description}");


            var em = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Yugipedia",
                    IconUrl = "https://ms.yugipedia.com//b/bc/Wiki.png",
                    Url = "https://yugipedia.com/wiki/Yugipedia"
                },
                Description = descriptionBuilder.ToString(),
                Title = Name,
                ThumbnailUrl = ImageUrl,
                Url = Uri.EscapeUriString($"https://yugipedia.com/wiki/{Name}")
            };

            return em.Build();
        }
    }
}
