#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/20/2016 11:48 AM
// Last Revised: 09/25/2016 6:22 AM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.OSRS {
    #region Using

    using System;

    #endregion

    public enum HighScoreType {
        Regular,
        Ironman,
        Ultimate,
        Deadman,
        Seasonal
    }

    public enum SkillType {
        Agility,
        Attack,
        Construction,
        Cooking,
        Crafting,
        Defense,
        Farming,
        Firemaking,
        Fishing,
        Fletching,
        Herblore,
        Hitpoints,
        Hunter,
        Magic,
        Mining,
        Prayer,
        Ranged,
        Runecrafting,
        Slayer,
        Smithing,
        Strength,
        Thieving,
        Woodcutting,
        Total,
        All,
        Combat,
        NonCombat
    }

    public class Account {
        #region Public Structs + Classes

        public struct Skill {
            #region Internal Fields + Properties

            internal ulong Experience { get; set; }
            internal int Level { get; set; }
            internal ulong Rank { get; set; }

            #endregion Internal Fields + Properties

            #region Public Methods

            public string ToDiscordMessage() => "```xl" + $"{Level}\t\t" + $"{Rank:N0}\t\t\t" + $"{Experience}";

            #endregion Public Methods

            public static int operator -(Skill a, Skill b) => a.Level - b.Level;

            public static int operator *(Skill a, Skill b) => a.Level * b.Level;

            public static int operator *(Skill a, int b) => a.Level * b;

            public static int operator *(int a, Skill b) => a * b.Level;

            public static double operator *(Skill a, double b) => a.Level * b;

            public static double operator *(double a, Skill b) => a * b.Level;

            public static int operator /(Skill a, Skill b) => a.Level / b.Level;

            public static decimal operator /(Skill a, decimal b) => a.Level / b;

            public static decimal operator /(decimal a, Skill b) => a / b.Level;

            public static int operator /(Skill a, int b) => a.Level / b;

            public static int operator /(int a, Skill b) => a / b.Level;

            public static int operator +(Skill a, Skill b) => a.Level + b.Level;

            public static int operator +(Skill a, int b) => a.Level + b;

            public static int operator +(int a, Skill b) => a + b.Level;
        }

        #endregion Public Structs + Classes

        #region Public Fields + Properties

        public Skill Agility { get; set; }
        public Skill Attack { get; set; }

        public decimal Combat
            =>
            Math.Round
                ((((Defense + Hitpoints + Math.Floor((decimal) (Prayer / 2))) * (decimal) 0.25) +
                  (decimal)
                  Math.Max
                      ((Attack + Strength) * 0.325,
                       Math.Max(Math.Floor(Magic * 1.5) * 0.325, Math.Floor(Ranged * 1.5) * 0.325))) * 100) / 100;

        public Skill Construction { get; set; }
        public Skill Cooking { get; set; }
        public Skill Crafting { get; set; }
        public Skill Defense { get; set; }
        public Skill Farming { get; set; }
        public Skill Firemaking { get; set; }
        public Skill Fishing { get; set; }
        public Skill Fletching { get; set; }
        public Skill Herblore { get; set; }
        public Skill Hitpoints { get; set; }
        public Skill Hunter { get; set; }
        public Skill Magic { get; set; }
        public Skill Mining { get; set; }
        public Skill Prayer { get; set; }
        public Skill Ranged { get; set; }
        public Skill Runecrafting { get; set; }
        public Skill Slayer { get; set; }
        public Skill Smithing { get; set; }
        public HighScoreType StatsSource { get; set; }
        public Skill Strength { get; set; }
        public Skill Thieving { get; set; }
        public Skill Total { get; set; }
        public string Username { get; set; }
        public Skill Woodcutting { get; set; }

        #endregion Public Fields + Properties

        #region Public Methods

        public string ToDiscordMessage()
            =>
            $"```xl\n{Username} / {StatsSource} / CB: {Combat} \nSKILL / LEVEL / EXPERIENCE / RANK\n" +
            $"TOTAL / {Total.Level} / {Total.Experience} / {Total.Rank}\n" +
            $"AGILITY / {Agility.Level} / {Agility.Experience} / {Agility.Rank}\n" +
            $"ATTACK / {Attack.Level} / {Attack.Experience} / {Attack.Rank}\n" +
            $"CONSTRUCTION / {Construction.Level} / {Construction.Experience} / {Construction.Rank}\n" +
            $"COOKING / {Cooking.Level} / {Cooking.Experience} / {Cooking.Rank}\n" +
            $"CRAFTING / {Crafting.Level} / {Crafting.Experience} / {Crafting.Rank}\n" +
            $"DEFENSE / {Defense.Level} / {Defense.Experience} / {Defense.Rank}\n" +
            $"FARMING / {Farming.Level} / {Farming.Experience} / {Farming.Rank}\n" +
            $"FIREMAKING / {Firemaking.Level} / {Firemaking.Experience} / {Firemaking.Rank}\n" +
            $"FISHING / {Fishing.Level} / {Fishing.Experience} / {Fishing.Rank}\n" +
            $"FLETCHING / {Fletching.Level} / {Fletching.Experience} / {Fletching.Rank}\n" +
            $"HERBLORE / {Herblore.Level} / {Herblore.Experience} / {Herblore.Rank}\n" +
            $"HITPOINTS / {Hitpoints.Level} / {Hitpoints.Experience} / {Hitpoints.Rank}\n" +
            $"HUNTER / {Hunter.Level} / {Hunter.Experience} / {Hunter.Rank}\n" +
            $"MAGIC / {Magic.Level} / {Magic.Experience} / {Magic.Rank}\n" +
            $"MINING / {Mining.Level} / {Mining.Experience} / {Mining.Rank}\n" +
            $"PRAYER / {Prayer.Level} / {Prayer.Experience} / {Prayer.Rank}\n" +
            $"RANGED / {Ranged.Level} / {Ranged.Experience} / {Ranged.Rank}\n" +
            $"RUNECRAFTING / {Runecrafting.Level} / {Runecrafting.Experience} / {Runecrafting.Rank}\n" +
            $"SLAYER / {Slayer.Level} / {Slayer.Experience} / {Slayer.Rank}\n" +
            $"SMITHING / {Smithing.Level} / {Smithing.Experience} / {Smithing.Rank}\n" +
            $"STRENGTH / {Strength.Level} / {Strength.Experience} / {Strength.Rank}\n" +
            $"THIEVING / {Thieving.Level} / {Thieving.Experience} / {Thieving.Rank}\n" +
            $"WOODCUTTING / {Woodcutting.Level} / {Woodcutting.Experience} / {Woodcutting.Rank}\n```";
    }

    #endregion Public Methods
}