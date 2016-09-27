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
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Agility));

                case "attack":
                case "att":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Attack));

                case "construction":
                case "con":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Construction));

                case "cooking":
                case "cook":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Cooking));

                case "crafting":
                case "craft":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Crafting));

                case "defense":
                case "def":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Defense));

                case "farming":
                case "farm":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Farming));

                case "firemaking":
                case "fire":
                case "fm":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Firemaking));

                case "fletching":
                case "fletch":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Fletching));

                case "herblore":
                case "herb":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Herblore));

                case "hitpoints":
                case "hp":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Hitpoints));

                case "hunter":
                case "hunt":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Hunter));

                case "magic":
                case "mage":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Magic));

                case "mining":
                case "mine":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Mining));

                case "prayer":
                case "pray":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Prayer));

                case "ranged":
                case "range":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Ranged));

                case "runecrafting":
                case "rc":
                case "rune":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Runecrafting));

                case "slayer":
                case "slay":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Slayer));

                case "smithing":
                case "smith":
                case "sm":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Smithing));

                case "strength":
                case "str":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Strength));

                case "thieving":
                case "theiving":
                case "thief":
                case "thiev":
                case "theiv":
                case "t":
                case "th":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Thieving));

                case "woodcutting":
                case "wood":
                case "wc":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Woodcutting));

                case "total":
                case "all":
                case "":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.All));

                case "cb":
                case "combatlevel":
                case "combat":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.Combat));

                case "noncombat":
                case "ncm":
                case "skiller":
                    return Task.FromResult(TypeReaderResult.FromSuccess(SkillType.NonCombat));

                default:
                    return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Could not parse skill declaration."));
            }
        }

        #endregion
    }
}