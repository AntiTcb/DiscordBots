#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/27/2016 10:48 PM
// Last Revised: 11/03/2016 9:19 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Modules.Owner {
    #region Using

    using System.Linq;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Interfaces;
    using Services;

    #endregion

    public partial class OwnerModule {
        #region Public Structs + Classes

        public sealed class EvalGlobals {
            #region Internal Fields + Properties

            internal IBotConfig BC { get; set; }
            internal CommandContext CC { get; set; }
            internal IServerConfig SC { get; set; }

            #endregion Internal Fields + Properties
        }

        [Group("eval")]
        public class EvalGroup : ModuleBase {
            #region Public Methods

            [Command("add")]
            public async Task AddImportAsync(string import) {
                Globals.EvalImports.Add(import);
                await ReplyAsync($"Added {import}").ConfigureAwait(false);
            }

            [Command("execute"), Alias("run", "exec", "=>")]
            public async Task EvaluateAsync([Remainder] string expression) {
                await EvalService.EvaluateAsync(Context, expression).ConfigureAwait(false);
            }

            [Command("list"), Alias("l")]
            public async Task ListImportsAsync() {
                await ReplyAsync(string.Join(", ", Globals.EvalImports.Select(x => x))).ConfigureAwait(false);
            }

            [Command("remove"), Alias("delete")]
            public async Task RemoveImportAsync(string import) {
                Globals.EvalImports.Remove(import);
                await ReplyAsync($"Removed {import}").ConfigureAwait(false);
            }

            #endregion Public Methods
        }

        #endregion Public Structs + Classes
    }
}