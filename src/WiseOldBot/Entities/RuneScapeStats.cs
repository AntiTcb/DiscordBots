#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/15/2016 10:01 PM
// Last Revised: 09/15/2016 10:05 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.Entities {
    public enum HighScoreType {
        Regular,
        Ironman,
        Ultimate,
        Deadman,
        Seasonal
    }

    public class RuneScapeStats {
        #region Public Structs + Classes

        public sealed class Stat {
            #region Internal Fields + Properties

            internal ulong Experience { get; set; }
            internal int Level { get; set; }
            internal ulong Rank { get; set; }

            #endregion Internal Fields + Properties

            public string ToDiscordMessage() => "```xl" +
                $"{Level}\t\t" +
                $"{Rank:N0}\t\t\t" +
                $"{Experience}";
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
        public Stat Strength { get; }
        public Stat Thieving { get; }
        public Stat Total { get; }
        public string Username { get; }
        public Stat Woodcutting { get; }
        public HighScoreType StatsSource { get; }

        #endregion Public Fields + Properties

        public string ToDiscordMessage() {
            return "";
        }
    }
}