using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WiseOldBot.Modules.OSRS
{
    [Flags]
    public enum SkillType
    {
        [Info(17)]
        Agility = 1 << 0,
        [Info(1)]
        Attack = 1 << 1,
        [Info(23)]
        Construction = 1 << 2,
        [Info(8)]
        Cooking = 1 << 3,
        [Info(13)]
        Crafting = 1 << 4,
        [Info(2)]
        Defence = 1 << 5,
        [Info(20)]
        Farming = 1 << 6,
        [Info(12)]
        Firemaking = 1 << 7,
        [Info(11)]
        Fishing = 1 << 8,
        [Info(10)]
        Fletching = 1 << 9,
        [Info(16)]
        Herblore = 1 << 10,
        [Info(4)]
        Hitpoints = 1 << 11,
        [Info(22)]
        Hunter = 1 << 12,
        [Info(7)]
        Magic = 1 << 13,
        [Info(15)]
        Mining = 1 << 14,
        [Info(6)]
        Prayer = 1 << 15,
        [Info(5)]
        Ranged = 1 << 16,
        [Info(21)]
        Runecrafting = 1 << 17,
        [Info(19)]
        Slayer = 1 << 18,
        [Info(14)]
        Smithing = 1 << 19,
        [Info(3)]
        Strength = 1 << 20,
        [Info(18)]
        Thieving = 1 << 21,
        [Info(9)]
        Woodcutting = 1 << 22,
        [Info(0)]
        Total = 1 << 23,

        // Special
        Combat = Attack | Strength | Defence | Hitpoints | Magic | Ranged | Prayer,
        All = Combat | Agility | Construction | Cooking | Crafting | Farming | Firemaking | Fishing | Fletching | Herblore | Hunter
            | Mining | Runecrafting | Slayer | Smithing | Thieving | Woodcutting,

        // Aliases
        [Info(1)]
        Att = Attack,
        [Info(1)]
        Atk = Attack,
        [Info(23)]
        Con = Construction,
        [Info(8)]
        Cook = Cooking,
        [Info(13)]
        Craft = Crafting,
        [Info(2)]
        Def = Defence,
        [Info(2)]
        Defense = Defence,
        [Info(20)]
        Farm = Farming,
        [Info(12)]
        Fire = Firemaking,
        [Info(12)]
        FM = Firemaking,
        [Info(10)]
        Fletch = Fletching,
        [Info(16)]
        Herb = Herblore,
        [Info(4)]
        HP = Hitpoints,
        [Info(4)]
        Health = Hitpoints,
        [Info(4)]
        Constitution = Hitpoints,
        [Info(22)]
        Hunt = Hunter,
        [Info(7)]
        Mage = Magic,
        [Info(15)]
        Mine = Mining,
        [Info(6)]
        Pray = Prayer,
        [Info(5)]
        Range = Ranged,
        [Info(21)]
        RC = Runecrafting,
        [Info(21)]
        Rune = Runecrafting,
        [Info(19)]
        Slay = Slayer,
        [Info(14)]
        SM = Smithing,
        [Info(14)]
        Smith = Smithing,
        [Info(0)]
        Overall = Total,
        [Info(18)]
        TH = Thieving,
        [Info(18)]
        Thiev = Thieving,
        [Info(18)]
        Thief = Thieving,
        [Info(18)]
        Theiving = Thieving,
        [Info(18)]
        Thiefing = Thieving,
        [Info(9)]
        WC = Woodcutting,
        [Info(9)]
        Wood = Woodcutting,
        CB = Combat,
        CLvl = Combat
    }

    //public enum SkillType
    //{
    //    Agility = 17,
    //    Attack = 1,
    //    Construction = 23,
    //    Cooking = 8,
    //    Crafting = 13,
    //    Defense = 2,
    //    Farming = 20,
    //    Firemaking = 12,
    //    Fishing = 11,
    //    Fletching = 10,
    //    Herblore = 16,
    //    Hitpoints = 4,
    //    Hunter = 22,
    //    Magic = 7,
    //    Mining = 15,
    //    Prayer = 6,
    //    Ranged = 5,
    //    Runecrafting = 21,
    //    Slayer = 19,
    //    Smithing = 14,
    //    Strength = 3,
    //    Thieving = 18,
    //    Woodcutting = 9,
    //    Total = 0,
    //    All = 100,
    //    Combat = 200,
    //    Agi = Agility,
    //    Att = Attack,
    //    Atk = Attack,
    //    Con = Construction,
    //    Cook = Cooking,
    //    Craft = Crafting,
    //    Def = Defense,
    //    Defence = Defense,
    //    Farm = Farming,
    //    Fire = Firemaking,
    //    FM = Firemaking,
    //    Fletch = Fletching,
    //    Herb = Herblore,
    //    HP = Hitpoints,
    //    Health = Hitpoints,
    //    Constitution = Hitpoints,
    //    Hunt = Hunter,
    //    Mage = Magic,
    //    Mine = Mining,
    //    Pray = Prayer,
    //    Range = Ranged,
    //    RC = Runecrafting,
    //    Rune = Runecrafting,
    //    Slay = Slayer,
    //    SM = Smithing,
    //    Smith = Smithing,
    //    T = Thieving,
    //    TH = Thieving,
    //    Thiev = Thieving,
    //    Thief = Thieving,
    //    Theiving = Thieving,
    //    Thiefing = Thieving,
    //    WC = Woodcutting,
    //    Wood = Woodcutting,
    //    CB = Combat,
    //    CLvl = Combat
    //}

    public static class SkillTypeExtensions
    {
        public static TAttr GetAttribute<TVal, TAttr>(this TVal value) where TVal : struct where TAttr : Attribute
        {
            if (!typeof(TVal).GetTypeInfo().IsEnum)
                throw new ArgumentException($"{nameof(TVal)} must be an enumerated type.");

            return value.GetType().GetRuntimeField(value.ToString()).GetCustomAttribute<TAttr>();
        }
        public static InfoAttribute GetInfo<T>(this T value) where T : struct
        {
            var info = value.GetAttribute<T, InfoAttribute>();
            return info;
        }
        public static IEnumerable<T> GetUniqueFlags<T>(this Enum flags)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("The generic type parameter must be an Enum.");

            if (!(flags is T))
                throw new ArgumentException("The generic type parameter does not match the target type.");

            ulong flag = 1;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<T>().Distinct())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && flags.HasFlag(value as Enum))
                {
                    yield return value;
                }
            }
        }

        public static int? GetIndex(this SkillType skill)
            => skill.GetInfo()?.Index;

        public static string GetSkillNameAndIcon(this SkillType skill)
        {
            if (!skill.GetIndex().HasValue) return skill.GetFullSkillName();
            if (skill == SkillType.All) return "All";

            switch (skill.GetIndex().Value)
            {
                case 0:
                    return "Total";
                case 1:
                    return $"{CustomEmoji.Attack} Attack";
                case 2:
                    return $"{CustomEmoji.Defense} Defense";
                case 3:
                    return $"{CustomEmoji.Strength} Strength";
                case 4:
                    return $"{CustomEmoji.Hitpoints} Hitpoints";
                case 5:
                    return $"{CustomEmoji.Ranged} Ranged";
                case 6:
                    return $"{CustomEmoji.Prayer} Prayer";
                case 7:
                    return $"{CustomEmoji.Magic} Magic";
                case 8:
                    return $"{CustomEmoji.Cooking} Cooking";
                case 9:
                    return $"{CustomEmoji.Woodcutting} Woodcutting";
                case 10:
                    return $"{CustomEmoji.Fletching} Fletching";
                case 11:
                    return $"{CustomEmoji.Fishing} Fishing";
                case 12:
                    return $"{CustomEmoji.Firemaking} Firemaking";
                case 13:
                    return $"{CustomEmoji.Crafting} Crafting";
                case 14:
                    return $"{CustomEmoji.Smithing} Smithing";
                case 15:
                    return $"{CustomEmoji.Mining} Mining";
                case 16:
                    return $"{CustomEmoji.Herblore} Herblore";
                case 17:
                    return $"{CustomEmoji.Agility} Agility";
                case 18:
                    return $"{CustomEmoji.Thieving} Thieving";
                case 19:
                    return $"{CustomEmoji.Slayer} Slayer";
                case 20:
                    return $"{CustomEmoji.Farming} Farming";
                case 21:
                    return $"{CustomEmoji.Runecrafting} Runecrafting";
                case 22:
                    return $"{CustomEmoji.Hunter} Hunter";
                case 23:
                    return $"{CustomEmoji.Construction} Construction";
                default:
                    return null;
            }
        }

        public static string GetFullSkillName(this SkillType skill)
        {
            if (!skill.GetIndex().HasValue) return null;
            if (skill == SkillType.All) return "All";
            switch (skill.GetIndex().Value)
            {
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
                default:
                    return null;
            }
        }
    }
}