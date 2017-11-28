namespace WiseOldBot.Modules.OSRS
{
    using Discord;
    using Entities;
    using System;
    using System.Text.RegularExpressions;
    using System.Linq;
    using System.Collections.Generic;

    public class Account
    {
        public Skill Agility { get; set; }
        public Skill Attack { get; set; }
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
        public GameMode StatsSource { get; set; }
        public Skill Strength { get; set; }
        public Skill Thieving { get; set; }
        public Skill Total { get; set; }
        public string Username { get; set; }
        public Skill Woodcutting { get; set; }

        public decimal Combat
            =>
            Math.Round((((Defense + Hitpoints + Math.Floor((decimal)(Prayer / 2))) * (decimal)0.25) + (decimal) Math.Max((Attack + Strength) * 0.325,
                Math.Max(Math.Floor(Magic * 1.5) * 0.325, Math.Floor(Ranged * 1.5) * 0.325))) * 100) / 100;

        public string Url => Uri.EscapeUriString($"http://services.runescape.com/m={_hsType}/hiscorepersonal.ws?user1={Username}");

        string _hsType;

        static Regex UrlParser = new Regex("(hiscore_oldschool.*?)/", RegexOptions.Compiled);

        public Account(string username, string uri, string[] skills)
        {
            Username = username;
            _hsType = UrlParser.Match(uri).Groups[1].Value;
            switch (_hsType)
            {
                case "hiscore_oldschool":
                    StatsSource = GameMode.Regular;
                    break;

                case "hiscore_oldschool_ironman":
                    StatsSource = GameMode.Ironman;
                    break;

                case "hiscore_oldschool_deadman":
                    StatsSource = GameMode.Deadman;
                    break;

                case "hiscore_oldschool_seasonal":
                    StatsSource = GameMode.DeadmanSeasonal;
                    break;

                case "hiscore_oldschool_ultimate":
                    StatsSource = GameMode.Ultimate;
                    break;
                case "hiscore_oldschool_hardcore_ironman":
                    StatsSource = GameMode.HardcoreIronman;
                    break;
            }

            Total = new Skill(skills[SkillType.Total.GetIndex().Value]);
            Attack = new Skill(skills[SkillType.Attack.GetIndex().Value]);
            Defense = new Skill(skills[SkillType.Defense.GetIndex().Value]);
            Strength = new Skill(skills[SkillType.Strength.GetIndex().Value]);
            Hitpoints = new Skill(skills[SkillType.Hitpoints.GetIndex().Value]);
            Ranged = new Skill(skills[SkillType.Ranged.GetIndex().Value]);
            Prayer = new Skill(skills[SkillType.Prayer.GetIndex().Value]);
            Magic = new Skill(skills[SkillType.Magic.GetIndex().Value]);
            Agility = new Skill(skills[SkillType.Agility.GetIndex().Value]);
            Construction = new Skill(skills[SkillType.Construction.GetIndex().Value]);
            Cooking = new Skill(skills[SkillType.Cooking.GetIndex().Value]);
            Woodcutting = new Skill(skills[SkillType.Woodcutting.GetIndex().Value]);
            Fletching = new Skill(skills[SkillType.Fletching.GetIndex().Value]);
            Crafting = new Skill(skills[SkillType.Crafting.GetIndex().Value]);
            Farming = new Skill(skills[SkillType.Farming.GetIndex().Value]);
            Firemaking = new Skill(skills[SkillType.Firemaking.GetIndex().Value]);
            Fishing = new Skill(skills[SkillType.Fishing.GetIndex().Value]);
            Smithing = new Skill(skills[SkillType.Smithing.GetIndex().Value]);
            Mining = new Skill(skills[SkillType.Mining.GetIndex().Value]);
            Herblore = new Skill(skills[SkillType.Herblore.GetIndex().Value]);
            Thieving = new Skill(skills[SkillType.Thieving.GetIndex().Value]);
            Slayer = new Skill(skills[SkillType.Slayer.GetIndex().Value]);
            Runecrafting = new Skill(skills[SkillType.Runecrafting.GetIndex().Value]);
            Hunter = new Skill(skills[SkillType.Hunter.GetIndex().Value]);
        }

        public EmbedBuilder SkillToDiscordEmbed(SkillType skill)
        {
            if (skill == SkillType.All)
                return ToDiscordEmbed(); 
            else if (skill == SkillType.Combat)
            {
                return new EmbedBuilder()
                {
                    Title = Username,
                    Description = $"Combat - {StatsSource.GetGameModeName()}",
                    Url = Url
                }
                .AddField("Combat", Combat)
                .AddField($"{CustomEmoji.Attack} Attack", Attack.ToString(), true)
                .AddField($"{CustomEmoji.Strength} Strength", Strength.ToString(), true)
                .AddField($"{CustomEmoji.Defense}Defense", Defense.ToString(), true)
                .AddField($"{CustomEmoji.Hitpoints} Hitpoints", Hitpoints.ToString(), true)
                .AddField($"{CustomEmoji.Ranged} Ranged", Ranged.ToString(), true)
                .AddField($"{CustomEmoji.Magic} Magic", Magic.ToString(), true)
                .AddField($"{CustomEmoji.Prayer} Prayer", Prayer.ToString(), true)
                .AddField($"{CustomEmoji.Slayer} Slayer", Slayer.ToString(), true)
                .AddField("Total", Total.ToString(), true);
            }
            var skillFlags = skill.GetUniqueFlags<SkillType>();
            if (skillFlags.Count() > 1)
            {
                var eb = new EmbedBuilder
                {
                    Title = Username,
                    Description = StatsSource.GetGameModeName(),
                    Url = Url
                };
                foreach (var s in skill.GetUniqueFlags<SkillType>())
                    eb.AddField(s.GetSkillNameAndIcon(), GetSkillBySkillType(s).ToString(), true);

                return eb;
            }
            return GetSkillBySkillType(skill).ToDiscordEmbed()
                .WithTitle(Username)
                .WithDescription($"{skill.GetSkillNameAndIcon()} - {StatsSource.GetGameModeName()}")
                .WithUrl(Url);
        }

        public string SkillToDiscordMessage(SkillType skill)
        {
            string returnString = $"{Format.Bold($"{Username}: {skill}")}\n";
            if (skill == SkillType.All)
                return ToDiscordMessage();
            else if (skill == SkillType.Combat)
                return returnString + Format.Code($"{Combat}");
            else
                return returnString + GetSkillBySkillType(skill).ToDiscordMessage();
        }

        public EmbedBuilder ToDiscordCombatEmbed() 
            => new EmbedBuilder()
        {
            Title = Username,
            Description = StatsSource.ToString(),
            Url = Url
        }.AddField(nameof(Combat), Combat.ToString());

        public EmbedBuilder ToDiscordEmbed() 
            => new EmbedBuilder()
        {
            Title = Username,
            Description = StatsSource.GetGameModeName(),
            Url = Url
        }
                .AddField("Combat", $"{Combat}")
                .AddField($"{CustomEmoji.Attack} {nameof(Attack)}", Attack.ToString(), true)
                .AddField($"{CustomEmoji.Hitpoints} {nameof(Hitpoints)}", Hitpoints.ToString(), true)
                .AddField($"{CustomEmoji.Mining} {nameof(Mining)}", Mining.ToString(), true)
                .AddField($"{CustomEmoji.Strength} {nameof(Strength)}", Strength.ToString(), true)
                .AddField($"{CustomEmoji.Agility} {nameof(Agility)}", Agility.ToString(), true)
                .AddField($"{CustomEmoji.Smithing} {nameof(Smithing)}", Smithing.ToString(), true)
                .AddField($"{CustomEmoji.Defense} {nameof(Defense)}", Defense.ToString(), true)
                .AddField($"{CustomEmoji.Herblore} {nameof(Herblore)}", Herblore.ToString(), true)
                .AddField($"{CustomEmoji.Fishing} {nameof(Fishing)}", Fishing.ToString(), true)
                .AddField($"{CustomEmoji.Ranged} {nameof(Ranged)}", Ranged.ToString(), true)
                .AddField($"{CustomEmoji.Thieving} {nameof(Thieving)}", Thieving.ToString(), true)
                .AddField($"{CustomEmoji.Cooking} {nameof(Cooking)}", Cooking.ToString(), true)
                .AddField($"{CustomEmoji.Prayer} {nameof(Prayer)}", Prayer.ToString(), true)
                .AddField($"{CustomEmoji.Crafting} {nameof(Crafting)}", Crafting.ToString(), true)
                .AddField($"{CustomEmoji.Firemaking} {nameof(Firemaking)}", Firemaking.ToString(), true)
                .AddField($"{CustomEmoji.Magic} {nameof(Magic)}", Magic.ToString(), true)
                .AddField($"{CustomEmoji.Fletching} {nameof(Fletching)}", Fletching.ToString(), true)
                .AddField($"{CustomEmoji.Woodcutting} {nameof(Woodcutting)}", Woodcutting.ToString(), true)
                .AddField($"{CustomEmoji.Runecrafting} {nameof(Runecrafting)}", Runecrafting.ToString(), true)
                .AddField($"{CustomEmoji.Slayer} {nameof(Slayer)}", Slayer.ToString(), true)
                .AddField($"{CustomEmoji.Farming} {nameof(Farming)}", Farming.ToString(), true)
                .AddField($"{CustomEmoji.Construction} {nameof(Construction)}", Construction.ToString(), true)
                .AddField($"{CustomEmoji.Hunter} {nameof(Hunter)}", Hunter.ToString(), true)
                .AddField(nameof(Total), Total.ToString(), true);

        public string ToDiscordMessage()
            =>
            $"```elm\n{Username} / {StatsSource} / CB: {Combat} \nSKILL / LEVEL / EXPERIENCE / RANK\n\n" +
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

        Skill GetSkillBySkillType(SkillType skill)
        {
            switch (skill)
            {
                case SkillType.Agility:
                    return Agility;
                case SkillType.Attack:
                    return Attack;
                case SkillType.Construction:
                    return Construction;
                case SkillType.Cooking:
                    return Cooking;
                case SkillType.Crafting:
                    return Crafting;
                case SkillType.Defense:
                    return Defense;
                case SkillType.Farming:
                    return Farming;
                case SkillType.Firemaking:
                    return Firemaking;
                case SkillType.Fishing:
                    return Fishing;
                case SkillType.Fletching:
                    return Fletching;
                case SkillType.Herblore:
                    return Herblore;
                case SkillType.Hitpoints:
                    return Hitpoints;
                case SkillType.Hunter:
                    return Hunter;
                case SkillType.Magic:
                    return Magic;
                case SkillType.Mining:
                    return Mining;
                case SkillType.Prayer:
                    return Prayer;
                case SkillType.Ranged:
                    return Ranged;
                case SkillType.Runecrafting:
                    return Runecrafting;
                case SkillType.Slayer:
                    return Slayer;
                case SkillType.Smithing:
                    return Smithing;
                case SkillType.Strength:
                    return Strength;
                case SkillType.Thieving:
                    return Thieving;
                case SkillType.Total:
                    return Total;
                case SkillType.Woodcutting:
                    return Woodcutting;
                default:
                    throw new ArgumentOutOfRangeException(nameof(skill), skill, null);
            }
        }
    }
}