namespace WiseOldBot.Modules.OSRS
{
    using System;
    using System.Threading.Tasks;
    using Discord.Commands;

    [Name("OSRS")]
    public partial class OSRSModule : ModuleBase<SocketCommandContext>
    {
        [Command("lvl2exp"), Alias("l2e")]
        [Summary("Gets the minimum experience required for the input level.")]
        [Remarks("lvl2exp 95")]
        public async Task CalculateExperienceAsync([Summary("Level")] uint level)
        {
            double exp = LevelToExperience(level);
            if (level > 99)
                await ReplyAsync($"Minimum exp: {exp}.");
            else
            {
                double nextLevelExp = LevelToExperience(level + 1);
                await ReplyAsync($"Minimum exp: {exp}. Next level: {nextLevelExp}. Difference:{nextLevelExp - exp}");
            }
        }

        [Command("exp2lvl"), Alias("e2l")]
        [Summary("Gets the level the input experience amount would equate to."), Remarks("exp2lvl 100000")]
        public async Task CalculateLevelAsync([Summary("Experience amount")]uint exp)
            => await ReplyAsync($"Level: {ExperienceToLevel(exp)}");

        

        private uint ExperienceToLevel(double exp)
        {
            double pts = 0;
            for (int lvl = 1; lvl < 99; ++lvl)
            {
                pts += Math.Floor(lvl + (300 * Math.Pow(2, lvl / 7D)));
                double output = pts / 4D;
                if (!((exp - output) < 0))
                    continue;

                double l_out = (pts - Math.Floor(lvl + (300 * Math.Pow(2, lvl / 7D)))) / 4;
                return (uint)(lvl + ((exp - l_out) / (output - l_out)));
            }
            return 99;
        }

        private double LevelToExperience(uint level)
        {
            double total = 0;
            for (int i = 1; i < level; i++)
                total += Math.Floor(i + (300 * Math.Pow(2, i / 7.0)));

            return Math.Floor(total / 4);
        }
    }
}