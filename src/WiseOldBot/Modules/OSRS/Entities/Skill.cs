#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 10/17/2016 6:05 PM
// Last Revised: 10/17/2016 6:06 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.Modules.OSRS {
    using Discord;

    public struct Skill {
        #region Internal Fields + Properties

        internal ulong Experience { get; set; }
        internal uint Level { get; set; }
        internal uint Rank { get; set; }

        #endregion Internal Fields + Properties

        #region Public Constructors

        public Skill(string skill) : this(skill.Split(',')) { }

        public Skill(ulong experience, uint level, uint rank) {
            Experience = experience;
            Level = level;
            Rank = rank;
        }

        public Skill(string[] skillArray) {
            Experience = ulong.Parse(skillArray[0]);
            Level = uint.Parse(skillArray[1]);
            Rank = uint.Parse(skillArray[2]);
        }

        #endregion Public Constructors

        #region Public Methods

        public string ToDiscordMessage() => Format.Code($"Level:{Level}\tExp: {Experience:N0}\tRank: {Rank:N0}");

        public override string ToString() => $"Exp: {Experience}, Level: {Level}, Rank: {Rank}";

        #endregion Public Methods

        public static uint operator -(Skill a, Skill b) => a.Level - b.Level;

        public static uint operator *(Skill a, Skill b) => a.Level * b.Level;

        public static uint operator *(Skill a, uint b) => a.Level * b;

        public static uint operator *(uint a, Skill b) => a * b.Level;

        public static double operator *(Skill a, double b) => a.Level * b;

        public static double operator *(double a, Skill b) => a * b.Level;

        public static uint operator /(Skill a, Skill b) => a.Level / b.Level;

        public static decimal operator /(Skill a, decimal b) => a.Level / b;

        public static decimal operator /(decimal a, Skill b) => a / b.Level;

        public static uint operator /(Skill a, uint b) => a.Level / b;

        public static uint operator /(uint a, Skill b) => a / b.Level;

        public static uint operator +(Skill a, Skill b) => a.Level + b.Level;

        public static uint operator +(Skill a, uint b) => a.Level + b;

        public static uint operator +(uint a, Skill b) => a + b.Level;
    }
}