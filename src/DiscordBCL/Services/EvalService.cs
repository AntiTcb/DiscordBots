using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Humanizer;

namespace DiscordBCL.Services
{
    public class EvalService
    {
        private readonly IEnumerable<Assembly> _assemblies;
        private readonly IEnumerable<string> _namespaceImports;
        private readonly ScriptOptions _options;
        private readonly CommandService _commands;

        public EvalService(CommandService commands)
        {
            _commands = commands;
            _assemblies = GetAssemblies();
            _namespaceImports = GetCustomNamespaces();
            _options = ScriptOptions.Default.AddReferences(_assemblies).AddImports(_namespaceImports);
            PrettyConsole.Log(LogSeverity.Info, "Services", "Loaded EvalService.");
        }

        public async Task EvaluteAsync(ShardedCommandContext ctx, string expr)
        {
            var sw = Stopwatch.StartNew();
            expr = expr.Trim('`');
            var globals = new ScriptGlobals(ctx, _commands);
            var working = await ctx.Channel.SendMessageAsync("**Evaluating...**");
            try
            {                                                                                 
                object eval = await CSharpScript.EvaluateAsync(expr, _options, globals, typeof(ScriptGlobals));
                sw.Stop();
                var eb = new EmbedBuilder
                {
                    Title = "Result:",
                    Description = eval.ToString(),
                    Color = Color.Green,
                    Footer = new EmbedFooterBuilder { Text = $"Elapsed: {sw.Elapsed.Humanize()}" }
                };

                await working.ModifyAsync(x => { x.Content = "Done!"; x.Embed = eb.Build(); });
            }
            catch (Exception e)
            {
                await working.ModifyAsync(x => x.Content = Format.Code(e.ToString()));
            }                                                            
        }

        private IEnumerable<string> GetCustomNamespaces()
        {
            var assemblies = new[] 
            {
                Assembly.GetEntryAssembly(),
                typeof(BotBase).GetTypeInfo().Assembly,
                typeof(Discord.Webhook.DiscordWebhookClient).GetTypeInfo().Assembly,
                typeof(To).GetTypeInfo().Assembly
            };
            var manualNamespaces = new[]
            {
                "System", "System.Linq", "System.Threading.Tasks", "System.Reflection", "System.Collections", "System.Collections.Generic",
                "System.IO", "System.Math", "System.Diagnostics",

                "Discord", "Discord.Commands", "Discord.Rest", "Discord.WebSocket", "Discord.Webhook",
            };                   
            var customNamespaces = new HashSet<string>(assemblies.SelectMany(a => a.GetTypes().Select(t => t.Namespace)).Where(c => !string.IsNullOrEmpty(c)));
            return manualNamespaces.Concat(customNamespaces);
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            foreach (var a in assemblies)
            {
                var asm = Assembly.Load(a);
                yield return asm;
            }
            yield return Assembly.GetEntryAssembly();
            yield return typeof(ILookup<string, string>).GetTypeInfo().Assembly;
        }
    }

    public class ScriptGlobals
    {                                                     
        public ShardedCommandContext Context { get; set; }
        public CommandService Commands { get; set; }

        public DiscordShardedClient Client => Context.Client;
        public ISocketMessageChannel Channel => Context.Channel;
        public SocketGuild Guild => Context.Guild;
        public SocketTextChannel GuildChannel => Context.Channel as SocketTextChannel;
        public SocketUserMessage Message => Context.Message;
        public DiscordSocketClient Shard => Client.GetShardFor(Guild);
        public SocketUser User => Context.User;
        public SocketGuildUser GuildUser => Context.User as SocketGuildUser;


        internal ScriptGlobals(ShardedCommandContext ctx, CommandService cmds)
        {
            Context = ctx;
            Commands = cmds;
        }
    }
}
