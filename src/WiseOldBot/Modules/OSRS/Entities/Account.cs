namespace WiseOldBot.Modules.OSRS
{
    using Discord;
    using Entities;
    using System;
    using System.Text.RegularExpressions;

    public class Account
    {
        public Skill Agility { get; set; }

        public Skill Attack { get; set; }

        public decimal Combat
            =>
            Math.Round
                ((((Defense + Hitpoints + Math.Floor((decimal)(Prayer / 2))) * (decimal)0.25) +
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
        public GameMode StatsSource { get; set; }    
        public Skill Strength { get; set; }          
        public Skill Thieving { get; set; }          
        public Skill Total { get; set; }             
        public string Url => Uri.EscapeUriString($"http://services.runescape.com/m={_hsType}/hiscorepersonal.ws?user1={Username}");        
        public string Username { get; set; }                                                                                               
        public Skill Woodcutting { get; set; }

        string _hsType;

        public Account(string username, string uri, string[] skills)
        {
            Username = username;
            _hsType = Regex.Match(uri, "(hiscore_oldschool.*?)/").Groups[1].Value;
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

            Total = new Skill(skills[(int)SkillType.Total]);
            Attack = new Skill(skills[(int)SkillType.Attack]);
            Defense = new Skill(skills[(int)SkillType.Defense]);
            Strength = new Skill(skills[(int)SkillType.Strength]);
            Hitpoints = new Skill(skills[(int)SkillType.Hitpoints]);
            Ranged = new Skill(skills[(int)SkillType.Ranged]);
            Prayer = new Skill(skills[(int)SkillType.Prayer]);
            Magic = new Skill(skills[(int)SkillType.Magic]);
            Agility = new Skill(skills[(int)SkillType.Agility]);
            Construction = new Skill(skills[(int)SkillType.Construction]);
            Cooking = new Skill(skills[(int)SkillType.Cooking]);
            Woodcutting = new Skill(skills[(int)SkillType.Woodcutting]);
            Fletching = new Skill(skills[(int)SkillType.Fletching]);
            Crafting = new Skill(skills[(int)SkillType.Crafting]);
            Farming = new Skill(skills[(int)SkillType.Farming]);
            Firemaking = new Skill(skills[(int)SkillType.Firemaking]);
            Fishing = new Skill(skills[(int)SkillType.Fishing]);
            Smithing = new Skill(skills[(int)SkillType.Smithing]);
            Mining = new Skill(skills[(int)SkillType.Mining]);
            Herblore = new Skill(skills[(int)SkillType.Herblore]);
            Thieving = new Skill(skills[(int)SkillType.Thieving]);
            Slayer = new Skill(skills[(int)SkillType.Slayer]);
            Runecrafting = new Skill(skills[(int)SkillType.Runecrafting]);
            Hunter = new Skill(skills[(int)SkillType.Hunter]);
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
                .AddField("Attack", Attack.ToString(), true)
                .AddField("Strength", Strength.ToString(), true)
                .AddField("Defense", Defense.ToString(), true)
                .AddField("Ranged", Ranged.ToString(), true)
                .AddField("Magic", Magic.ToString(), true)
                .AddField("Prayer", Prayer.ToString(), true);
            }
            return GetSkillBySkillType(skill).ToDiscordEmbed()
                .WithTitle(Username)
                .WithDescription($"{skill.GetFullSkillName()} - {StatsSource.GetGameModeName()}")
                .WithUrl(Url);
        }

        public string SkillToDiscordMessage(SkillType skill)
        {
            var returnString = $"{Format.Bold($"{Username}: {skill}")}\n";
            if (skill == SkillType.All)
                return ToDiscordMessage();
            else if (skill == SkillType.Combat)
                return returnString + Format.Code($"{Combat}");
            else
                return returnString + GetSkillBySkillType(skill).ToDiscordMessage();
        }

        public EmbedBuilder ToDiscordCombatEmbed()
        {
            return new EmbedBuilder()
            {
                Title = Username,
                Description = StatsSource.ToString(),
                Url = Url
            }.AddField(nameof(Combat), $"{Combat}");
        }

        public EmbedBuilder ToDiscordEmbed()
        {
            return new EmbedBuilder()
            {
                Title = Username,
                Description = StatsSource.GetGameModeName(),
                Url = Url
            }
                .AddField("Combat", $"{Combat}")
                .AddField($"{nameof(Attack)}{CustomEmoji.Attack}", Attack.ToString(), true)
                .AddField($"{nameof(Hitpoints)}{CustomEmoji.Hitpoints}", Hitpoints.ToString(), true)
                .AddField($"{nameof(Mining)}{CustomEmoji.Mining}", Mining.ToString(), true)
                .AddField($"{nameof(Strength)}{CustomEmoji.Strength}", Strength.ToString(), true)
                .AddField($"{nameof(Agility)}{CustomEmoji.Agility}", Agility.ToString(), true)
                .AddField($"{nameof(Smithing)}{CustomEmoji.Smithing}", Smithing.ToString(), true)
                .AddField($"{nameof(Defense)}{CustomEmoji.Defense}", Defense.ToString(), true)
                .AddField($"{nameof(Herblore)}{CustomEmoji.Herblore}", Herblore.ToString(), true)
                .AddField($"{nameof(Fishing)}{CustomEmoji.Fishing}", Fishing.ToString(), true)
                .AddField($"{nameof(Ranged)}{CustomEmoji.Ranged}", Ranged.ToString(), true)
                .AddField($"{nameof(Thieving)}{CustomEmoji.Thieving}", Thieving.ToString(), true)
                .AddField($"{nameof(Cooking)}{CustomEmoji.Cooking}", Cooking.ToString(), true)
                .AddField($"{nameof(Prayer)}{CustomEmoji.Prayer}", Prayer.ToString(), true)
                .AddField($"{nameof(Crafting)}{CustomEmoji.Crafting}", Crafting.ToString(), true)
                .AddField($"{nameof(Firemaking)}{CustomEmoji.Firemaking}", Firemaking.ToString(), true)
                .AddField($"{nameof(Magic)}{CustomEmoji.Magic}", Magic.ToString(), true)
                .AddField($"{nameof(Fletching)}{CustomEmoji.Fletching}", Fletching.ToString(), true)
                .AddField($"{nameof(Woodcutting)}{CustomEmoji.Woodcutting}", Woodcutting.ToString(), true)
                .AddField($"{nameof(Runecrafting)}{CustomEmoji.Runecrafting}", Runecrafting.ToString(), true)
                .AddField($"{nameof(Slayer)}{CustomEmoji.Slayer}", Slayer.ToString(), true)
                .AddField($"{nameof(Farming)}{CustomEmoji.Farming}", Farming.ToString(), true)
                .AddField($"{nameof(Construction)}{CustomEmoji.Construction}", Construction.ToString(), true)
                .AddField($"{nameof(Hunter)}{CustomEmoji.Hunter}", Hunter.ToString(), true)
                .AddField(nameof(Total), Total.ToString(), true);
        }

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