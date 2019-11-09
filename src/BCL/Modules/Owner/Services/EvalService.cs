namespace BCL.Modules.Owner.Services {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;

    public static class EvalService {

        public static IEnumerable<Assembly> Assemblies => GetAssemblies();
        public static IEnumerable<string> Imports => Globals.EvalImports;

        public static async Task EvaluateAsync(SocketCommandContext context, string script) {
            using (context.Channel.EnterTypingState()) {
                var options = ScriptOptions.Default.AddReferences(Assemblies).AddImports(Imports);
                var working = await context.Channel.SendMessageAsync("**Evaluating**, just a sec...");
                var _globals = new ScriptGlobals { client = context.Client as DiscordSocketClient, context = context };
                script = script.Trim('`');
                try {
                    object eval = await CSharpScript.EvaluateAsync(script, options, _globals, typeof(ScriptGlobals));
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

    }

    public class ScriptGlobals {

        public ISocketMessageChannel channel => context.Channel;
        public DiscordSocketClient client { get; internal set; }
        public SocketCommandContext context { get; internal set; }
        public SocketGuild guild => context.Guild;
        public SocketUserMessage msg => context.Message;
        public SocketUser user => context.User;
        public SocketGuildUser guser => context.User as SocketGuildUser;
        public SocketTextChannel gChannel => context.Channel as SocketTextChannel;
    }
}