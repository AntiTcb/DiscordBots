// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/29/2016 11:34 PM
// Last Revised: 10/30/2016 1:53 PM
// Last Revised by: Alex Gravely

namespace OrgBot.Modules.YGOWikia.Entities
{
    using Discord;
    using System;
    using System.Linq;

    public class YGOWikiaCard
    {
        public string ATK { get; set; }
        public string Attribute { get; set; }

        public CardCategory Category 
            => string.IsNullOrEmpty(Types) ? CardCategory.None : PendulumScales != null 
                ? Rank != null 
                ? CardCategory.PendulumXyz : CardCategory.Pendulum : Types.Contains("Spell Card") || Types == "Spell" 
                ? CardCategory.Spell : Types.Contains("Trap Card") || Types == "Trap" 
                ? CardCategory.Trap : Rank != null 
                ? CardCategory.Xyz : Types.Contains("Link") 
                ? CardCategory.Link : CardCategory.Monster;

        public string DEF { get; set; }
        public string LinkNumber { get; set; }
        public string Effect { get; set; }
        public string ImageUrl { get; set; }
        public string Level { get; set; }
        public string LinkArrows { get; set; }
        public string Name { get; set; }
        public string PendulumScales { get; set; }
        public string Property { get; set; }
        public string Rank { get; set; }
        public string Types { get; set; }
        public string Url { get; set; }

        public static YGOWikiaCard Parse(ILookup<string, string> things, string effect, string url, string imgUrl)
        {
            if (things == null) { return null; }
            var newCard = new YGOWikiaCard
            {
                Name = things["English"].ElementAtOrDefault(0) ?? "Parse failed",
                Types = things["Types"].ElementAtOrDefault(0)?.Trim()
                    ?? things["Type"].ElementAtOrDefault(0)?.Trim()
                    ?? things["Card type"].ElementAtOrDefault(0)?.Trim(),

                Attribute = things["Attribute"].ElementAtOrDefault(0),
                ATK = things["ATK / LINK"].ElementAtOrDefault(0)?.Split('/')?.ElementAtOrDefault(0).Trim() 
                    ?? things["ATK / DEF"].ElementAtOrDefault(0)?.Split('/')?.ElementAtOrDefault(0)?.Trim() 
                    ?? things["ATK/DEF"].ElementAtOrDefault(0)?.Split('/')?.ElementAtOrDefault(0)?.Trim(),

                DEF = things["ATK / DEF"].ElementAtOrDefault(0)?.Split('/')?.ElementAtOrDefault(1)?.Trim() 
                    ?? things["ATK/DEF"].ElementAtOrDefault(0)?.Split('/')?.ElementAtOrDefault(1)?.Trim(),

                LinkNumber = things["ATK / LINK"].ElementAtOrDefault(0)?.Split('/')?.ElementAtOrDefault(1)?.Trim(),
                LinkArrows = things["Link Arrows"].ElementAtOrDefault(0),
                Level = things["Level"].ElementAtOrDefault(0),
                Rank = things["Rank"].ElementAtOrDefault(0),
                PendulumScales = things["Pendulum Scale"].ElementAtOrDefault(0)?.Trim(),
                Property = things["Property"].ElementAtOrDefault(0),
                Effect = effect.Trim(),
                Url = url,
                ImageUrl = imgUrl
            };
            return newCard;
        }

        public EmbedBuilder ToDiscordEmbed()
        {
            var isPend = Category == CardCategory.Pendulum || Category == CardCategory.PendulumXyz;
            var isXyz = Category == CardCategory.Xyz || Category == CardCategory.PendulumXyz;
            var isSpellOrTrap = Category == CardCategory.Spell || Category == CardCategory.Trap;
            var description = isSpellOrTrap ? $"{Property?.TrimEnd()} {Types}" : $"{(isXyz ? $"Rank {Rank}" : (Category == CardCategory.Link ? "" : $"Level {Level} | "))}{Attribute?.Trim()} | {Types}";
            var statLine = Category == CardCategory.Link ? $"**ATK** / {ATK} \t **LINK** / {LinkNumber}\n **Link Arrows:** {LinkArrows}" : $"**ATK** / {ATK} \t**DEF** / {DEF}";
            var scaleLine = $"{CustomEmoji.LeftScale}{PendulumScales} / {PendulumScales}{CustomEmoji.RightScale}";
            var em = new EmbedBuilder()
                .WithTitle($"{Name} - {Url}")
                .WithUrl(Url)
                .WithDescription($"{description}{(isSpellOrTrap ? "" : $"\n{statLine}")}{(isPend ? $"\n{scaleLine}" : "")}")
                .WithThumbnailUrl(ImageUrl)
                .WithAuthor((a) =>
                    a.WithName("Yu-Gi-Oh! Wikia")
                     .WithIconUrl("http://img3.wikia.nocookie.net/__cb15/yugioh/images/8/89/Wiki-wordmark.png")
                     .WithUrl("http://yugioh.wikia.com/wiki/Yu-Gi-Oh!_Wikia")
                )
                .AddField((f) =>
                    f.WithName("Effects:")
                    .WithValue(Effect));
            return em;
        }

        public string ToDiscordMessage()
        {
            string returnString = $"{Name} | {Types} ";
            switch (Category)
            {
                case CardCategory.Monster:
                    returnString += $"| {Attribute}\nLevel: {Level} | ATK / {ATK} DEF /{DEF}";
                    break;

                case CardCategory.Pendulum:
                    returnString += $"| {Attribute}\nLevel: {Level} | Scales: {PendulumScales} | ATK / {ATK}  DEF / {DEF}";
                    break;

                case CardCategory.Xyz:
                    returnString += $"| {Attribute}\nRank: {Rank} | ATK / {ATK} DEF / {DEF}";
                    break;

                case CardCategory.PendulumXyz:
                    returnString += $"| {Attribute}\nRank: {Rank} | Scales: {PendulumScales} | ATK / {ATK } DEF / {DEF}";
                    break;

                case CardCategory.Spell:
                case CardCategory.Trap:
                    var stReturnString = Format.Code($"{Name} | {Property.TrimEnd()} {Types}\n{Effect}", "elm");
                    return stReturnString;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Format.Code($"{returnString}\n{Effect}\n\n", "elm") + $"<{Url}>";
        }
    }
}