#region Header
// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/23/2016 6:01 AM
// Last Revised: 09/23/2016 6:01 AM
// Last Revised by: Alex Gravely
#endregion
namespace WiseOldBot.TypeReaders {
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Entities;

    public class SkillTypeReader : TypeReader {
        #region Overrides of TypeReader

        public override Task<TypeReaderResult> Read(IUserMessage context, string input) {
            switch (input.ToLower()) {
                case "agility":
                case "agi":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Agility));

                case "attack":
                case "att":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Attack));

                case "construction":
                case "con":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Construction));

                case "cooking":
                case "cook":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Cooking));

                case "crafting":
                case "craft":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Crafting));

                case "defense":
                case "def":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Defense));

                case "farming":
                case "farm":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Farming));

                case "firemaking":
                case "fire":
                case "fm":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Firemaking));

                case "fletching":
                case "fletch":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Fletching));

                case "herblore":
                case "herb":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Herblore));

                case "hitpoints":
                case "hp":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Hitpoints));

                case "hunter":
                case "hunt":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Hunter));

                case "magic":
                case "mage":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Magic));

                case "mining":
                case "mine":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Mining));

                case "prayer":
                case "pray":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Prayer));

                case "ranged":
                case "range":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Ranged));

                case "runecrafting":
                case "rc":
                case "rune":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Runecrafting));

                case "slayer":
                case "slay":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Slayer));

                case "smithing":
                case "smith":
                case "sm":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Smithing));

                case "strength":
                case "str":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Strength));

                case "thieving":
                case "theiving":
                case "thief":
                case "thiev":
                case "theiv":
                case "t":
                case "th":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Thieving));

                case "woodcutting":
                case "wood":
                case "wc":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Woodcutting));

                case "total":
                case "all":
                case "":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.All));

                case "cb":
                case "combatlevel":
                case "combat":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.Combat));

                case "noncombat":
                case "ncm":
                case "skiller":
                    return Task.FromResult(TypeReaderResult.FromSuccess(Skill.NonCombat));

                default:
                    return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Could not parse skill declaration."));
            }
        }

        #endregion
    }
}