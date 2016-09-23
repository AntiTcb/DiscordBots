#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/20/2016 11:48 AM
// Last Revised: 09/23/2016 6:00 AM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.Entities {
    using System;

    public enum HighScoreType {
        Regular,
        Ironman,
        Ultimate,
        Deadman,
        Seasonal
    }

    public enum Skill {
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

    public class RuneScapeStats {
        #region Public Structs + Classes

        public sealed class Stat {
            #region Internal Fields + Properties

            internal ulong Experience { get; set; }
            internal int Level { get; set; }
            internal ulong Rank { get; set; }

            #endregion Internal Fields + Properties

            #region Public Methods

            public string ToDiscordMessage() => "```xl" + $"{Level}\t\t" + $"{Rank:N0}\t\t\t" + $"{Experience}";

            #endregion Public Methods

            public static int operator +(Stat a, Stat b) => a.Level + b.Level;
            public static int operator +(Stat a, int b) => a.Level + b;
            public static int operator +(int a, Stat b) => a + b.Level;
            public static int operator -(Stat a, Stat b) => a.Level - b.Level;
            public static int operator *(Stat a, Stat b) => a.Level * b.Level;
            public static int operator *(Stat a, int b) => a.Level * b;
            public static int operator *(int a, Stat b) => a * b.Level;
            public static double operator *(Stat a, double b) => a.Level * b;
            public static double operator *(double a, Stat b) => a * b.Level;
            public static int operator /(Stat a, Stat b) => a.Level / b.Level;
            public static decimal operator /(Stat a, decimal b) => a.Level / b;
            public static decimal operator /(decimal a, Stat b) => a / b.Level;
            public static int operator /(Stat a, int b) => a.Level / b;
            public static int operator /(int a, Stat b) => a / b.Level;
        }

        #endregion Public Structs + Classes

        #region Public Fields + Properties

        public Stat Agility { get; }
        public Stat Attack { get; }
        public Stat Construction { get; }
        public Stat Cooking { get; }
        public Stat Crafting { get; }
        public Stat Defense { get; }
        public Stat Farming { get; }
        public Stat Firemaking { get; }
        public Stat Fishing { get; }
        public Stat Fletching { get; }
        public Stat Herblore { get; }
        public Stat Hitpoints { get; }
        public Stat Hunter { get; }
        public Stat Magic { get; }
        public Stat Mining { get; }
        public Stat Prayer { get; }
        public Stat Ranged { get; }
        public Stat Runecrafting { get; }
        public Stat Slayer { get; }
        public Stat Smithing { get; }
        public HighScoreType StatsSource { get; }
        public Stat Strength { get; }
        public Stat Thieving { get; }
        public Stat Total { get; }
        public string Username { get; }
        public Stat Woodcutting { get; }

        public decimal Combat
            =>
            Math.Round
                ((((Defense + Hitpoints + Math.Floor((decimal) (Prayer / 2))) * (decimal) 0.25) +
                 (decimal)
                 Math.Max((Attack + Strength) * 0.325,
                      Math.Max(Math.Floor(Magic * 1.5) * 0.325, Math.Floor(Ranged * 1.5) * 0.325))) * 100) / 100;

        #endregion Public Fields + Properties

        #region Public Methods

        public string ToDiscordMessage() {
            return $"```xl" +
                ".------------------------------------------------------------.";
        }

        #endregion Public Methods
    }
}