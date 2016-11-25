#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/29/2016 11:34 PM
// Last Revised: 10/30/2016 1:53 PM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOWikia.Entities {
    #region Using

    using System;
    using System.Linq;
    using Discord;
    using YGOCard.Entities;

    #endregion

    public class YGOWikiaCard {
        #region Public Fields + Properties

        public string Attribute { get; set; }

        public CardCategory Category
            =>
            string.IsNullOrEmpty(Types) ? CardCategory.None :
            PendulumScales != null
                ? Rank != null ? CardCategory.PendulumXyzMonster : CardCategory.PendulumMonster
                : Types.Contains("Spell Card")
                    ? CardCategory.Spell : Types.Contains("Trap Card") ? CardCategory.Trap : Rank != null ? CardCategory.XyzMonster : CardCategory.Monster;

        public string Effect { get; set; }
        public string Level { get; set; }
        public string Name { get; set; }
        public string PendulumScales { get; set; }
        public string Rank { get; set; }
        public string ATK { get; set; }
        public string DEF { get; set; }
        public string Types { get; set; }
        public string Property { get; set; }
        public string Url { get; set; }

        #endregion Public Fields + Properties

        #region Public Methods

        public static YGOWikiaCard Parse(ILookup<string, string> things, string effect, string url) {
            var newCard = new YGOWikiaCard {
                Name = things["English"].ElementAtOrDefault(0) ?? "Parse failed",
                Types = things["Types"].ElementAtOrDefault(0) ?? things["Type"].ElementAtOrDefault(0),
                Attribute = things["Attribute"].ElementAtOrDefault(0),
                ATK = things["ATK / DEF"].ElementAtOrDefault(0)?.Split('/')?.ElementAtOrDefault(0)?.Trim() ?? things["ATK/DEF"].ElementAtOrDefault(0)?.Split('/')?.ElementAtOrDefault(0)?.Trim(),
                DEF = things["ATK / DEF"].ElementAtOrDefault(0)?.Split('/')?.ElementAtOrDefault(1)?.Trim() ?? things["ATK/DEF"].ElementAtOrDefault(0)?.Split('/')?.ElementAtOrDefault(1)?.Trim(),
                Level = things["Level"].ElementAtOrDefault(0),
                Rank = things["Rank"].ElementAtOrDefault(0),
                PendulumScales = things["Pendulum Scale"].ElementAtOrDefault(0)?.Trim(),
                Property = things["Property"].ElementAtOrDefault(0),
                Effect = effect.Trim(),
                Url = url
            };
            return (newCard.Name == "Parse failed" ? null : newCard);
        }

        public EmbedBuilder ToDiscordEmbed() {
            var isPend = Category == CardCategory.PendulumMonster || Category == CardCategory.PendulumXyzMonster;
            var isXyz = Category == CardCategory.XyzMonster || Category == CardCategory.PendulumXyzMonster;
            var isSpellOrTrap = Category == CardCategory.Spell || Category == CardCategory.Trap;
            var description = isSpellOrTrap ? $"{Property?.TrimEnd()} {Types}" : $"{(isXyz ? $"Rank {Rank}" : $"Level {Level}")} | {Attribute?.Trim()} | {Types}";
            var atkDefLine = $"ATK / {ATK} \tDEF / {DEF}";
            var scaleLine = $"<:leftscale:251264662572761089>{PendulumScales} / {PendulumScales}<:rightscale:251264701730652161>";
            var em = new EmbedBuilder()
                .WithTitle($"{Name} - {Url}")
                .WithUrl(Url)
                .WithDescription($"{description}{(isSpellOrTrap ? "" : $"\n{atkDefLine}")}{(isPend ? $"\n{scaleLine}" : "")}")
                .AddField((f) =>
                    f.WithName("Effects:")
                    .WithValue(Effect));
            return em;
        }

        public string ToDiscordMessage() {
            string returnString = $"{Name} | {Types} ";
            switch (Category) {
                case CardCategory.Monster:
                    returnString += $"| {Attribute}\nLevel: {Level} | ATK / {ATK} DEF /{DEF}";
                    break;

                case CardCategory.PendulumMonster:
                    returnString += $"| {Attribute}\nLevel: {Level} | Scales: {PendulumScales} | ATK / {ATK}  DEF / {DEF}";
                    break;

                case CardCategory.XyzMonster:
                    returnString += $"| {Attribute}\nRank: {Rank} | ATK / {ATK} DEF / {DEF}";
                    break;

                case CardCategory.PendulumXyzMonster:
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

        #endregion Public Methods
    }
}