using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBCL.Modules
{
    [Hidden]
    [Name("Owner")]
    [RequireOwner]
    public partial class OwnerModule : ModuleBase<ShardedCommandContext>
    {
        private readonly CommandService _service;

        public OwnerModule(CommandService service)
            => _service = service;

        [Command("echo")]
        [Summary("Echoes the input string.")]
        [Remarks("echo potato")]
        public async Task EchoAsync([Remainder]string input)
            => await ReplyAsync(input).ConfigureAwait(false);

        [Command("shutdown", RunMode = RunMode.Async)]
        [Alias("pd", "off")]
        [Summary("Terminates the application.")]
        [Remarks("shutdown")]
        public async Task ShutdownAsync()
        {
            await ReplyAsync("Shutting down.").ConfigureAwait(false);
            await Task.Delay(1500).ConfigureAwait(false);
            await Context.Client.StopAsync().ConfigureAwait(false);
            await Task.Delay(10000);
            Environment.Exit(0);          
        }                                  
    }
}
