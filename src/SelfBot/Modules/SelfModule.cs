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

    [Module, RequireOwner(89613772372574208)]
    public class SelfModule {
        DiscordSocketClient _client;
        IConfig _config;

        public SelfModule(DiscordSocketClient c, IConfig con) {
            _client = c;
            _config = con;
        }

        public class RoslynGlobals {
            public DiscordSocketClient Client { get; set; }

            public RoslynGlobals(DiscordSocketClient c) {
                Client = c;
            }
        }

        [Group("import")]
        public class ImportGroup {
            [Command("add")]
            public async Task AddImport(IUserMessage msg, [Remainder] string import) {
                var _config = await ConfigHandler.LoadAsync<SelfConfig>(Program.CONFIG_PATH);
                if (_config.EvalImports.Contains(import)) {
                    return;
                }
                _config.EvalImports.Add(import);
                await ConfigHandler.SaveAsync(Program.CONFIG_PATH, _config);
                await msg.ModifyAsync(m => m.Content = $"Added import: {import}");
            }

            [Command("remove"), Alias("del")]
            public async Task RemoveImport(IUserMessage msg, [Remainder] string import) {
                var _config = await ConfigHandler.LoadAsync<SelfConfig>(Program.CONFIG_PATH);
                if (!_config.EvalImports.Contains(import)) {
                    return;
                }
                _config.EvalImports.Remove(import);
                await ConfigHandler.SaveAsync(Program.CONFIG_PATH, _config);
                await msg.ModifyAsync(m => m.Content = $"Removed import: {import}");
            }

            [Command("list")]
            public async Task ListImports(IUserMessage msg) {
                var _config = await ConfigHandler.LoadAsync<SelfConfig>(Program.CONFIG_PATH);
                await msg.ModifyAsync(m => m.Content = string.Join("\n", _config.EvalImports.Select(x => x)));
            }
        }

        [Command("eval")]
        public async Task Eval(IUserMessage msg, [Remainder] string script) {
            _config = await ConfigHandler.LoadAsync<SelfConfig>(Program.CONFIG_PATH);
            try {
                var opts = ScriptOptions.Default
                    .WithReferences(
                        typeof(object).GetTypeInfo().Assembly,
                        typeof(Enumerable).GetTypeInfo().Assembly,
                        typeof(DiscordSocketClient).GetTypeInfo().Assembly,
                        typeof(Command).GetTypeInfo().Assembly,
                        typeof(IMessage).GetTypeInfo().Assembly)
                    .WithImports((_config as SelfConfig).EvalImports);
                var globals = new RoslynGlobals(_client);
                var result = await CSharpScript.EvaluateAsync(script, opts, globals);
                await msg.ModifyAsync(m => m.Content = $"{Format.Code(result?.ToString())}");
            }
            catch (Exception ex) {
                await msg.ModifyAsync(m => m.Content = $"Exception! -- {Format.Code($"{ex}", "cs")}");
            }
        }
    }
}