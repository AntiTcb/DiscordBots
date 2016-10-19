#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/16/2016 11:42 PM
// Last Revised: 10/19/2016 6:17 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.TypeReaders {
    #region Using

    using System.Threading.Tasks;
    using Discord.Commands;
    using Modules.OSRS;

    #endregion

    public class HighScoreTypeReader : TypeReader {
        #region Overrides of TypeReader

        public override Task<TypeReaderResult> Read(CommandContext context, string input) {
            if (input.Split(' ').Length > 1) {
                return Task.FromResult
                    (TypeReaderResult.FromError(CommandError.BadArgCount, "Invalid highscore type argument count"));
            }
            switch (input.ToLower()) {
                case "":
                case "r":
                case "normal":
                case "regular":
                case "norm":
                case "reg":
                    return Task.FromResult(TypeReaderResult.FromSuccess(HighScoreType.Regular));

                case "ironman":
                case "i":
                case "iron":
                    return Task.FromResult(TypeReaderResult.FromSuccess(HighScoreType.Ironman));

                case "ui":
                case "ultimate":
                case "u":
                case "ulti":
                    return Task.FromResult(TypeReaderResult.FromSuccess(HighScoreType.Ultimate));

                case "dmm":
                case "dm":
                case "deadman":
                    return Task.FromResult(TypeReaderResult.FromSuccess(HighScoreType.Deadman));

                case "dms":
                case "seasonal":
                case "dmms":
                case "dmseasonal":
                    return Task.FromResult(TypeReaderResult.FromSuccess(HighScoreType.Seasonal));

                default:
                    return Task.FromResult
                        (TypeReaderResult.FromError(CommandError.ParseFailed, "Could not parse input highscore type."));
            }
        }

        #endregion Overrides of TypeReader
    }
}