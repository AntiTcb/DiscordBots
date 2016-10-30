#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/27/2016 10:48 PM
// Last Revised: 10/27/2016 10:54 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Modules.Owner {
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Interfaces;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;

    #endregion

    public partial class OwnerModule {
        #region Public Structs + Classes

        public sealed class EvalGlobals
        {
            #region Internal Fields + Properties

            internal IBotConfig BC { get; set; }
            internal CommandContext CC { get; set; }
            internal IServerConfig SC { get; set; }

            #endregion Internal Fields + Properties
        }

        [Group("eval")]
        public class EvalGroup : ModuleBase {
            #region Public Structs + Classes

            [Group("imports")]
            public class ImportGroup : ModuleBase {
                #region Public Methods

                [Command("add")]
                public async Task AddImportAsync(string import) {
                    Globals.EvalImports.Add(import);
                    await ReplyAsync($"Added {import}").ConfigureAwait(false);
                }

                [Command("list"), Alias("l")]
                public async Task ListImportsAsync()
                    => await ReplyAsync(string.Join(", ", Globals.EvalImports.Select(x => x))).ConfigureAwait(false);

                [Command("remove"), Alias("delete")]
                public async Task RemoveImportAsync(string import)
                {
                    Globals.EvalImports.Remove(import);
                    await ReplyAsync($"Removed {import}").ConfigureAwait(false);
                }
                #endregion Public Methods
            }

            #endregion Public Structs + Classes     

            #region Public Methods

            [Command("execute"), Alias("run", "exec", "=>")]
            public async Task EvaluateAsync([Remainder] string expression) {
                await Context.Channel.TriggerTypingAsync().ConfigureAwait(false);
                var options =
                    ScriptOptions.Default.AddReferences
                                  (typeof(object).GetTypeInfo().Assembly.Location,
                                   typeof(Enumerable).GetTypeInfo().Assembly.Location,
                                   typeof(IUser).GetTypeInfo().Assembly.Location,
                                   typeof(DiscordSocketClient).GetTypeInfo().Assembly.Location,
                                   typeof(CommandContext).GetTypeInfo().Assembly.Location,
                                   typeof(Assembly).GetTypeInfo().Assembly.Location).AddImports(Globals.EvalImports);

                var guildID = Context.Guild?.Id;
                var globalObjects = guildID != null
                                        ? new EvalGlobals {
                                            CC = Context,
                                            BC = Globals.BotConfig,
                                            SC = Globals.ServerConfigs[guildID.Value]
                                        }
                                        : new EvalGlobals { CC = Context, BC = Globals.BotConfig };
                var result = await CSharpScript.EvaluateAsync(expression, globals: globalObjects, globalsType: typeof(EvalGlobals)).ConfigureAwait(false);
                await ReplyAsync($"Eval: {result}").ConfigureAwait(false);
            }

            #endregion Public Methods
        }

        #endregion Public Structs + Classes
    }
}