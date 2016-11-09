#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 11/03/2016 8:33 PM
// Last Revised: 11/03/2016 9:32 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Modules.Owner.Services {
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;

    #endregion

    public static class EvalService {
        #region Private Fields + Properties

        public static IEnumerable<Assembly> Assemblies => GetAssemblies();
        public static IEnumerable<string> Imports => Globals.EvalImports;

        #endregion Private Fields + Properties   

        #region Public Methods

        public static async Task EvaluateAsync(CommandContext context, string script) {
            using (context.Channel.EnterTypingState()) {
                var options = ScriptOptions.Default.AddReferences(Assemblies).AddImports(Imports);
                var working = await context.Channel.SendMessageAsync("**Evaluating**, just a sec...");
                var _globals = new ScriptGlobals { client = context.Client as DiscordSocketClient, context = context };
                script = script.Trim('`');
                try {
                    var eval = await CSharpScript.EvaluateAsync(script, options, _globals, typeof(ScriptGlobals));
                    await context.Channel.SendMessageAsync(eval.ToString());
                }
                catch (Exception e) {
                    await context.Channel.SendMessageAsync($"**Script Failed**\n{e.Message}");
                }
                finally {
                    await working.DeleteAsync();
                }
            }
        }

        public static IEnumerable<Assembly> GetAssemblies() {
            var Assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            foreach (var a in Assemblies) {
                var asm = Assembly.Load(a);
                yield return asm;
            }
            yield return Assembly.GetEntryAssembly();
            yield return typeof(ILookup<string, string>).GetTypeInfo().Assembly;
        }

        #endregion Public Methods
    }

    public class ScriptGlobals {
        #region Public Fields + Properties

        public SocketGuildChannel channel => context.Channel as SocketGuildChannel;
        public DiscordSocketClient client { get; internal set; }
        public CommandContext context { get; internal set; }
        public SocketGuild guild => context.Guild as SocketGuild;
        public SocketMessage msg => context.Message as SocketMessage;

        #endregion Public Fields + Properties
    }
}