// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 10/17/2016 6:05 PM
// Last Revised: 10/17/2016 6:06 PM
// Last Revised by: Alex Gravely

namespace WiseOldBot.Modules.OSRS
{
    using Discord;

    public struct Skill
    {
        public long Experience { get; set; }
        public uint Level { get; set; }
        public int Rank { get; set; }

        public Skill(string skill) : this(skill.Split(',')) { }

        public Skill(long experience, uint level, int rank, SkillType skillName)
        {
            Experience = experience;
            Level = level;
            Rank = rank;
        }

        public Skill(string[] skillArray)
        {
            Experience = long.Parse(skillArray[2]);
            Level = uint.Parse(skillArray[1]);
            Rank = int.Parse(skillArray[0]);
        }

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

        public EmbedBuilder ToDiscordEmbed()
        {
            var level = Level.ToString();
            var exp = $"{Experience:N0}";
            var rank = $"{(Rank == -1 ? "Unranked" : $"{Rank:N0}")}";

            var em = new EmbedBuilder()
                .AddField("Level:", level, true)
                .AddField("Experience:", exp, true)
                .AddField("Rank:", rank, true);
            return em;
        }

        public string ToDiscordMessage() => Format.Code($"Level:{Level}\tExp: {Experience:N0}\tRank: {(Rank == -1 ? "Unranked" : $"{Rank:N0}")}");

        public override string ToString() => $"Level: {Level} \nExp: {Experience:N0} \nRank: {(Rank == -1 ? "Unranked" : $"{Rank:N0}")}";
    }
}