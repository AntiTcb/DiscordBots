#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 10/17/2016 6:05 PM
// Last Revised: 10/17/2016 6:05 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.Modules.OSRS { 
    public static class SkillTypeExtensions {
        public static string GetFullSkillName(this SkillType skill) {
            switch ((int)skill) {
                case 0:
                    return "Total";
                case 1:
                    return "Attack";
                case 2:
                    return "Defense";
                case 3:
                    return "Strength";
                case 4:
                    return "Hitpoints";
                case 5:
                    return "Ranged";
                case 6:
                    return "Prayer";
                case 7:
                    return "Magic";
                case 8:
                    return "Cooking";
                case 9:
                    return "Woodcutting";
                case 10:
                    return "Fletching";
                case 11:
                    return "Fishing";
                case 12:
                    return "Firemaking";
                case 13:
                    return "Crafting";
                case 14:
                    return "Smithing";
                case 15:
                    return "Mining";
                case 16:
                    return "Herblore";
                case 17:
                    return "Agility";
                case 18:
                    return "Thieving";
                case 19:
                    return "Slayer";
                case 20:
                    return "Farming";
                case 21:
                    return "Runecrafting";
                case 22:
                    return "Hunter";
                case 23:
                    return "Construction";
                case 100:
                    return "All";
                default:
                    return null;
            }
        }
    }

    public enum SkillType {
        Agility = 17,
        Attack = 1,
        Construction = 23,
        Cooking = 8,
        Crafting = 13,
        Defense = 2,
        Farming = 20,
        Firemaking = 12,
        Fishing = 11,
        Fletching = 10,
        Herblore = 16,
        Hitpoints = 4,
        Hunter = 22,
        Magic = 7,
        Mining = 15,
        Prayer = 6,
        Ranged = 5,
        Runecrafting = 21,
        Slayer = 19,
        Smithing = 14,
        Strength = 3,
        Thieving = 18,
        Woodcutting = 9,
        Total = 0,
        All = 100,
        Combat = 200,
        Agi = Agility,
        Att = Attack,
        Atk = Attack,
        Con = Construction,
        Cook = Cooking,
        Craft = Crafting,
        Def = Defense,
        Defence = Defense,
        Farm = Farming,
        Fire = Firemaking,
        FM = Firemaking,
        Fletch = Fletching,
        Herb = Herblore,
        HP = Hitpoints,
        Health = Hitpoints, 
        Constitution = Hitpoints,
        Hunt = Hunter,
        Mage = Magic,
        Mine = Mining,
        Pray = Prayer,
        Range = Ranged,
        RC = Runecrafting,
        Rune = Runecrafting,
        Slay = Slayer,
        SM = Smithing,
        Smith = Smithing,
        T = Thieving,
        TH = Thieving,
        Thiev = Thieving,
        Thief = Thieving,
        Theiving = Thieving,
        Thiefing = Thieving,
        WC = Woodcutting,
        Wood = Woodcutting,
        CB = Combat,
        CLvl = Combat
    }
}