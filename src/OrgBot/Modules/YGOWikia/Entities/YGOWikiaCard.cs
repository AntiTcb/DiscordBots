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

    #endregion

    public class YGOWikiaCard {
        #region Public Fields + Properties

        public string Attribute { get; set; }

        public CardCategory? Category
            =>
            PendulumScales != null
                ? Rank != null ? CardCategory.PendulumXyzMonster : CardCategory.PendulumMonster
                : Types.Contains("Spell")
                    ? CardCategory.Spell : Types.Contains("Trap") ? CardCategory.Trap : CardCategory.Monster;

        public string Effect { get; set; }
        public string Level { get; set; }
        public string Name { get; set; }
        public string PendulumScales { get; set; }
        public string Rank { get; set; }
        public string Stats { get; set; }
        public string Types { get; set; }

        #endregion Public Fields + Properties

        #region Public Methods

        public static YGOWikiaCard Parse(ILookup<string, string> things, string effect) {
            var newCard = new YGOWikiaCard {
                Name = things["English"].ElementAtOrDefault(0) ?? "Parse failed",
                Types = things["Types"].ElementAtOrDefault(0) ?? things["Type"].ElementAtOrDefault(0),
                Attribute = things["Attribute"].ElementAtOrDefault(0),
                Stats = things["ATK / DEF"].ElementAtOrDefault(0) ?? things["ATK/DEF"].ElementAtOrDefault(0),
                Level = things["Level"].ElementAtOrDefault(0),
                Rank = things["Rank"].ElementAtOrDefault(0),
                PendulumScales = things["Pendulum Scale"].ElementAtOrDefault(0),
                Effect = effect.Trim('\n')
            };
            return newCard;
        }

        public string ToDiscordMessage() {
            string returnString = $"{Name} | {Types}";
            if (Category == null) {
                return "Invalid card";
            }
            switch (Category) {
                case CardCategory.Monster:
                    returnString += $"| {Attribute}\nLevel: {Level} | {Stats}";
                    break;

                case CardCategory.PendulumMonster:
                    returnString += $"| {Attribute}\nLevel: {Level} | Scales: {PendulumScales} | {Stats}";
                    break;

                case CardCategory.XyzMonster:
                    returnString += $"| {Attribute}\nRank: {Rank} | {Stats}";
                    break;

                case CardCategory.PendulumXyzMonster:
                    returnString += $"| {Attribute}\nRank: {Rank} | Scales: {PendulumScales} | {Stats}";
                    break;

                case CardCategory.Spell:
                    break;

                case CardCategory.Trap:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Format.Code($"{returnString}\n{Effect}", "elm");
        }

        #endregion Public Methods
    }
}