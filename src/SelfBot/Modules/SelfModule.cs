#region Header
// Description:
// 
// Solution: DiscordBots
// Project: SelfBot
// 
// Created: 10/03/2016 3:41 PM
// Last Revised: 10/03/2016 3:41 PM
// Last Revised by: Alex Gravely
#endregion
namespace SelfBot.Modules {
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using System.Reflection;
    using BCL;
    using BCL.Interfaces;
    using BCL.Preconditions;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;

    public class SelfModule : ModuleBase {

        public class RoslynGlobals {
            public DiscordSocketClient Client { get; set; }

            public RoslynGlobals(DiscordSocketClient c) {
                Client = c;
            }
        }

        [Group("import")]
        public class ImportGroup : ModuleBase {
            [Command("add")]
            public async Task AddImport([Remainder] string import) {
                (Globals.BotConfig as SelfConfig).EvalImports.Add(import);
                await ConfigHandler.SaveAsync(Globals.CONFIG_PATH, Globals.BotConfig);
                await Context.Message.ModifyAsync(m => m.Content = $"Added import: {import}");
            }

            [Command("remove"), Alias("del")]
            public async Task RemoveImport([Remainder] string import) {
                (Globals.BotConfig as SelfConfig).EvalImports.Remove(import);
                await ConfigHandler.SaveAsync(Globals.CONFIG_PATH, Globals.BotConfig);
                await Context.Message.ModifyAsync(m => m.Content = $"Removed import: {import}");
            }

            [Command("list")]
            public async Task ListImports() {
                await Context.Message.ModifyAsync(m => m.Content = string.Join("\n", (Globals.BotConfig as SelfConfig).EvalImports.Select(x => x)));
            }
        }

        [Command("eval")]
        public async Task Eval([Remainder] string script) {
            try {
                var opts = ScriptOptions.Default
                    .WithReferences(
                        typeof(object).GetTypeInfo().Assembly,
                        typeof(Enumerable).GetTypeInfo().Assembly,
                        typeof(DiscordSocketClient).GetTypeInfo().Assembly,
                        typeof(CommandContext).GetTypeInfo().Assembly,
                        typeof(IMessage).GetTypeInfo().Assembly)
                    .WithImports((Globals.BotConfig as SelfConfig).EvalImports);
                var globals = new RoslynGlobals(Context.Client as DiscordSocketClient);
                var result = await CSharpScript.EvaluateAsync(script, opts, globals);
                await Context.Message.ModifyAsync(m => m.Content = $"Eval: {Format.Code(result?.ToString())}");
            }
            catch (Exception ex) {
                await Context.Message.ModifyAsync(m => m.Content = $"Exception! -- {Format.Code($"{ex}", "cs")}");
            }
        }
    }
}