﻿#region Header

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

        public CardCategory Category
            =>
            string.IsNullOrEmpty(Types) ? CardCategory.None :
            PendulumScales != null
                ? Rank != null ? CardCategory.PendulumXyzMonster : CardCategory.PendulumMonster
                : Types.Contains("Spell Card")
                    ? CardCategory.Spell : Types.Contains("Trap Card") ? CardCategory.Trap : CardCategory.Monster;

        public string Effect { get; set; }
        public string Level { get; set; }
        public string Name { get; set; }
        public string PendulumScales { get; set; }
        public string Rank { get; set; }
        public string Stats { get; set; }
        public string Types { get; set; }
        public string Property { get; set; }
        public string Url { get; set; }

        #endregion Public Fields + Properties

        #region Public Methods

        public static YGOWikiaCard Parse(ILookup<string, string> things, string effect, string url) {
            var newCard = new YGOWikiaCard {
                Name = things["English"].ElementAtOrDefault(0) ?? "Parse failed.",
                Types = things["Types"].ElementAtOrDefault(0) ?? things["Type"].ElementAtOrDefault(0),
                Attribute = things["Attribute"].ElementAtOrDefault(0),
                Stats = things["ATK / DEF"].ElementAtOrDefault(0) ?? things["ATK/DEF"].ElementAtOrDefault(0),
                Level = things["Level"].ElementAtOrDefault(0),
                Rank = things["Rank"].ElementAtOrDefault(0),
                PendulumScales = things["Pendulum Scale"].ElementAtOrDefault(0),
                Property = things["Property"].ElementAtOrDefault(0),
                Effect = effect.Trim('\n'),
                Url = url
            };
            return newCard;
        }

        public string ToDiscordMessage() {
            string returnString = $"{Name} | {Types} ";
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