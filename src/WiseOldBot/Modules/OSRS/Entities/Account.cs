#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/25/2016 6:48 AM
// Last Revised: 10/19/2016 6:08 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.Modules.OSRS {
    #region Using

    using System;

    #endregion

    public class Account {
        #region Public Structs + Classes

        public Account(string username, string hsType, string[] skills) {
            Username = username;

            switch (hsType) {
                case "hiscore_oldschool":
                    StatsSource = HighScoreType.Regular;
                    break;

                case "hiscore_oldschool_ironman":
                    StatsSource = HighScoreType.Ironman;
                    break;

                case "hiscore_oldschool_deadman":
                    StatsSource = HighScoreType.Deadman;
                    break;

                case "hiscore_oldschool_seasonal":
                    StatsSource = HighScoreType.Seasonal;
                    break;

                case "hiscore_oldschool_ultimate":
                    StatsSource = HighScoreType.Ultimate;
                    break;
            }

            Total = new Skill(skills[(int) SkillType.Total]);
            Attack = new Skill(skills[(int) SkillType.Attack]);
            Defense = new Skill(skills[(int) SkillType.Defense]);
            Strength = new Skill(skills[(int) SkillType.Strength]);
            Hitpoints = new Skill(skills[(int) SkillType.Hitpoints]);
            Ranged = new Skill(skills[(int) SkillType.Ranged]);
            Prayer = new Skill(skills[(int) SkillType.Prayer]);
            Magic = new Skill(skills[(int) SkillType.Magic]);
            Agility = new Skill(skills[(int) SkillType.Agility]);
            Construction = new Skill(skills[(int) SkillType.Construction]);
            Cooking = new Skill(skills[(int) SkillType.Cooking]);
            Woodcutting = new Skill(skills[(int) SkillType.Woodcutting]);
            Fletching = new Skill(skills[(int) SkillType.Fletching]);
            Crafting = new Skill(skills[(int) SkillType.Crafting]);
            Farming = new Skill(skills[(int) SkillType.Farming]);
            Firemaking = new Skill(skills[(int) SkillType.Firemaking]);
            Fishing = new Skill(skills[(int) SkillType.Fishing]);
            Smithing = new Skill(skills[(int) SkillType.Smithing]);
            Mining = new Skill(skills[(int) SkillType.Mining]);
            Herblore = new Skill(skills[(int) SkillType.Herblore]);
            Thieving = new Skill(skills[(int) SkillType.Thieving]);
            Slayer = new Skill(skills[(int) SkillType.Slayer]);
            Runecrafting = new Skill(skills[(int) SkillType.Runecrafting]);
            Hunter = new Skill(skills[(int) SkillType.Hunter]);
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
            $"```xl\n{Username} / {StatsSource} / CB: {Combat} \nSKILL / LEVEL / EXPERIENCE / RANK\n\n" +
            $"TOTAL / {Total.Level} / {Total.Experience:N0} / {Total.Rank:N0}\n" +
            $"AGILITY / {Agility.Level} / {Agility.Experience:N0} / {Agility.Rank:N0}\n" +
            $"ATTACK / {Attack.Level} / {Attack.Experience:N0} / {Attack.Rank:N0}\n" +
            $"CONSTRUCTION / {Construction.Level} / {Construction.Experience:N0} / {Construction.Rank:N0}\n" +
            $"COOKING / {Cooking.Level} / {Cooking.Experience:N0} / {Cooking.Rank:N0}\n" +
            $"CRAFTING / {Crafting.Level} / {Crafting.Experience:N0} / {Crafting.Rank:N0}\n" +
            $"DEFENSE / {Defense.Level} / {Defense.Experience:N0} / {Defense.Rank:N0}\n" +
            $"FARMING / {Farming.Level} / {Farming.Experience:N0} / {Farming.Rank:N0}\n" +
            $"FIREMAKING / {Firemaking.Level} / {Firemaking.Experience:N0} / {Firemaking.Rank:N0}\n" +
            $"FISHING / {Fishing.Level} / {Fishing.Experience:N0} / {Fishing.Rank:N0}\n" +
            $"FLETCHING / {Fletching.Level} / {Fletching.Experience:N0} / {Fletching.Rank:N0}\n" +
            $"HERBLORE / {Herblore.Level} / {Herblore.Experience:N0} / {Herblore.Rank:N0}\n" +
            $"HITPOINTS / {Hitpoints.Level} / {Hitpoints.Experience:N0} / {Hitpoints.Rank:N0}\n" +
            $"HUNTER / {Hunter.Level} / {Hunter.Experience:N0} / {Hunter.Rank:N0}\n" +
            $"MAGIC / {Magic.Level} / {Magic.Experience:N0} / {Magic.Rank:N0}\n" +
            $"MINING / {Mining.Level} / {Mining.Experience:N0} / {Mining.Rank:N0}\n" +
            $"PRAYER / {Prayer.Level} / {Prayer.Experience:N0} / {Prayer.Rank:N0}\n" +
            $"RANGED / {Ranged.Level} / {Ranged.Experience:N0} / {Ranged.Rank:N0}\n" +
            $"RUNECRAFTING / {Runecrafting.Level} / {Runecrafting.Experience:N0} / {Runecrafting.Rank:N0}\n" +
            $"SLAYER / {Slayer.Level} / {Slayer.Experience:N0} / {Slayer.Rank:N0}\n" +
            $"SMITHING / {Smithing.Level} / {Smithing.Experience:N0} / {Smithing.Rank:N0}\n" +
            $"STRENGTH / {Strength.Level} / {Strength.Experience:N0} / {Strength.Rank:N0}\n" +
            $"THIEVING / {Thieving.Level} / {Thieving.Experience:N0} / {Thieving.Rank:N0}\n" +
            $"WOODCUTTING / {Woodcutting.Level} / {Woodcutting.Experience:N0} / {Woodcutting.Rank:N0}\n```";
    }

    #endregion Public Methods
}