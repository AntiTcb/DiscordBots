#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/16/2016 5:11 PM
// Last Revised: 10/16/2016 9:41 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Modules.Owner {
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord.WebSocket;
    using Interfaces;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;
    using Preconditions;

    #endregion

    [RequireOwner]
    public class OwnerModule : ModuleBase {

        [Command("powerdown"), Alias("pd")]
        public async Task PowerdownAsync() {
            await ReplyAsync("Powering down!").ConfigureAwait(false);
            await Context.Client.DisconnectAsync().ConfigureAwait(false);
        }

        #region Private Structs + Classes

        sealed class EvalGlobals {
            #region Internal Fields + Properties

            internal IBotConfig BotConfig { get; set; }
            internal CommandContext CommandContext { get; set; }
            internal IServerConfig ServerConfig { get; set; }

            #endregion Internal Fields + Properties
        }

        static List<string> EvalImports { get; } = new List<string> {
            "Discord",
            "Discord.Commands",
            "Discord.WebSocket",
            "System",
            "System.Collections",
            "System.Collections.Generic",
            "System.Linq",
            "System.Reflection",
            "System.Threading.Tasks"
        };

        #endregion Private Structs + Classes

        #region Public Methods

        [Command("addimport"), Alias("add")]
        public async Task AddImportAsync(string import) {
            EvalImports.Add(import);
            await ReplyAsync($"Added {import}").ConfigureAwait(false);
        }

        [Command("evaluate"), Alias("eval")]
        public async Task EvaluateAsync([Remainder] string expression) {
            var options =
                ScriptOptions.Default.AddReferences
                              (typeof(object).GetTypeInfo().Assembly.Location,
                               typeof(Enumerable).GetTypeInfo().Assembly.Location,
                               typeof(DiscordSocketClient).GetTypeInfo().Assembly.Location,
                               typeof(CommandContext).GetTypeInfo().Assembly.Location,
                               typeof(Assembly).GetTypeInfo().Assembly.Location).AddImports(EvalImports);

            var guildID = Context.Guild?.Id;
            var globalObjects = guildID != null
                                    ? new EvalGlobals {
                                        CommandContext = Context,
                                        BotConfig = Globals.BotConfig,
                                        ServerConfig = Globals.ServerConfigs[guildID.Value]
                                    }
                                    : new EvalGlobals { CommandContext = Context, BotConfig = Globals.BotConfig };
            var result = await CSharpScript.EvaluateAsync(expression, options, globalObjects).ConfigureAwait(false);
            await ReplyAsync($"Eval: {result}").ConfigureAwait(false);
        }

        [Command("listimports"), Alias("list")]
        public async Task ListImportsAsync()
            => await ReplyAsync(string.Join(", ", EvalImports.Select(x => x))).ConfigureAwait(false);

        [Command("removeimport"), Alias("remove")]
        public async Task RemoveImportAsync(string import) {
            EvalImports.Remove(import);
            await ReplyAsync($"Removed {import}").ConfigureAwait(false);
        }

        #endregion Public Methods
    }
}