// Description:
//
// Solution: DiscordBots
// Project: BCL
//
// Created: 10/27/2016 10:48 PM
// Last Revised: 11/03/2016 9:19 PM
// Last Revised by: Alex Gravely

namespace BCL.Modules.Owner {

    using Discord.Commands;
    using Interfaces;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class OwnerModule {

        public sealed class EvalGlobals {
            internal IBotConfig BC { get; set; }
            internal CommandContext CC { get; set; }
            internal IServerConfig SC { get; set; }
        }

        [Name("Eval"), Group("eval"), RequireOwner]
        public class EvalGroup : ModuleBase {

            [Command("add"),
                Summary("Adds a namespace import to the eval namespace imports list."),
                Remarks("eval add System.Math")]
            public async Task AddImportAsync([Summary("Namespace name")]string import) {
                Globals.EvalImports.Add(import);
                await ReplyAsync($"Added {import}").ConfigureAwait(false);
            }

            [Command("execute"),
                Alias("run", "exec", "=>"), Summary("Executes a valid C# expression"), Remarks("eval execute 1+1")]
            public async Task EvaluateAsync([Remainder, Summary("C# expression to evaluate")] string expression) {
                await EvalService.EvaluateAsync(Context, expression).ConfigureAwait(false);
            }

            [Command("list"), Alias("l"), Summary("Lists all the current namespace imports."), Remarks("eval list")]
            public async Task ListImportsAsync() {
                await ReplyAsync(string.Join(", ", Globals.EvalImports.Select(x => x))).ConfigureAwait(false);
            }

            [Command("remove"),
                Alias("delete"),
                Summary("Removes a namespace import from the eval namespace imports list."),
                Remarks("eval remove System.Math")]
            public async Task RemoveImportAsync([Summary("Namespace name")]string import) {
                Globals.EvalImports.Remove(import);
                await ReplyAsync($"Removed {import}").ConfigureAwait(false);
            }
        }
    }
}