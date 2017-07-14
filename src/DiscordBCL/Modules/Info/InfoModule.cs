using System;                                                                             
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBCL.Modules
{
    [Name("Info")]
    public class InfoModule : ModuleBase<ShardedCommandContext>
    {
        private CommandService _cmdService;
        private IServiceProvider _provider;
        private string _prefix => _provider.GetRequiredService<IConfiguration>()["prefix"];

        public InfoModule(CommandService cmdService, IServiceProvider provider)
        {
            _cmdService = cmdService;
            _provider = provider;                                                                
        }

        [Command("help", RunMode = RunMode.Async)]
        [Alias("commands")]
        [Remarks("help")]
        public async Task HelpAsync()
        {
            var modules = _cmdService.Modules.Where(m => 
                m.CanExecute(Context, _provider) && !m.Attributes.Any(a => a is HiddenAttribute)).OrderBy(m => m.Name);
            var sentMessage = await ReplyAsync("", embed: modules.GetEmbed(Context, _provider)).ConfigureAwait(false);
            await Task.Delay(30000).ConfigureAwait(false);
            await sentMessage.DeleteAsync().ConfigureAwait(false);
        }

        [Command("help", RunMode = RunMode.Async), Priority(1)]
        [Alias("help:command")]
        [Summary("Information about a specific command.")]
        [Remarks("help help")]
        public async Task HelpAsync([Remainder]CommandInfo commandName)
        {
            if (!commandName.CanExecute(Context, _provider))                        
                await ReplyAsync("You do not have permission to run this command.");
            else 
                await ReplyAsync("", embed: commandName.GetEmbed(Context)).ConfigureAwait(false);
        }

        [Command("help", RunMode = RunMode.Async), Priority(2)]
        [Alias("help:module")]
        [Summary("Information about a specific module.")]
        [Remarks("help info")]
        public async Task HelpAsync([Remainder]ModuleInfo moduleName)
        {
            if (!moduleName.CanExecute(Context, _provider))
                await ReplyAsync("You do not have permission to view this module.");
            else
                await ReplyAsync("", embed: moduleName.GetEmbed(Context, _provider));
        }                                                                  

        [Command("info")]
        [Summary("Gets general information about the bot.")]
        [Remarks("info")]
        public async Task InfoAsync()
        {
            var app = await Context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
            int channelCount = Context.Client.Guilds.Sum(g => g.Channels.Count);
            int memberCount = Context.Client.Guilds.Sum(g => g.MemberCount);
            var assemblyName = Assembly.GetEntryAssembly().GetName();

            var eb = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = app.Owner.ToString(),
                    IconUrl = app.Owner.GetAvatarUrl(),
                    Url = "https://github.com/AntiTcb"
                },                                                              
                Description = "Check out the source code on GitHub!",
                Url = "https://github.com/AntiTcb/DiscordBots/", // TODO
                Title = $"{assemblyName.Name} {assemblyName.Version}" 
            };                                                                             
            eb.AddField("Library", $"[Discord.Net v{DiscordConfig.Version}](https://github.com/RogueException/Discord.Net)");
            eb.AddField("Runtime", $"{AppContext.TargetFrameworkName} {RuntimeInformation.ProcessArchitecture} " +
                    $"({RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture})");
            eb.AddInlineField("Uptime", GetUptime());
            eb.AddInlineField("Heap", GetHeapSize());
            eb.AddInlineField("Latency", Context.Client.Latency);
            eb.AddInlineField("Guilds", Context.Client.Guilds.Count);
            eb.AddInlineField("Channels", channelCount);
            eb.AddInlineField("Users", memberCount);   

            await ReplyAsync("", embed: eb);
        }

        [Command("latency", RunMode = RunMode.Async)]
        [Alias("ping", "pong", "rtt")]
        [Summary("Returns the current estimated round-trip latency over the websocket gateway.")]
        [Remarks("latency")]
        public async Task Latency()
        {
            ulong target = 0;
            var cts = new CancellationTokenSource();

            Task WaitTarget(SocketMessage message)
            {
                if (message.Id != target) return Task.CompletedTask;
                cts.Cancel();
                return Task.CompletedTask;
            }

            int latency = Context.Client.Latency;
            var s = Stopwatch.StartNew();
            var m = await ReplyAsync($"Heartbeat: {latency}ms, Init: ---, Round-Trip Time: ---");
            long init = s.ElapsedMilliseconds;
            target = m.Id;
            s.Restart();
            Context.Client.MessageReceived += WaitTarget;

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(30), cts.Token);
            }
            catch (TaskCanceledException)
            {
                long rtt = s.ElapsedMilliseconds;
                s.Stop();
                await m.ModifyAsync(x => x.Content = $"Heartbeat: {latency}ms, Init: {init}ms, Round-Trip Time: {rtt}ms");
                return;
            }
            finally
            {
                Context.Client.MessageReceived -= WaitTarget;
            }
            s.Stop();
            await m.ModifyAsync(x => x.Content = $"Heartbeat: {latency}ms, Init: {init}ms, Round-Trip Time: timeout");
        }

        [Command("invite"), Alias("join")]
        [Summary("Allows you to add the bot to your guilds!")]
        [Remarks("invite")]
        public async Task InviteAsync()
        {
            var app = await Context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
            var eb = new EmbedBuilder
            {
                Title = "Click here to add me to your guild!",
                Description = "**Note:** You must have the __Manage Server__ permission in the guild you want to add me to.",
                Url = $"https://discordapp.com/oauth2/authorize?permissions=67496960&client_id={app.Id}&scope=bot",
                Author = new EmbedAuthorBuilder
                {
                    Name = Context.Guild.CurrentUser.Nickname ?? Context.Client.CurrentUser.Username
                },
                ThumbnailUrl = app.IconUrl
            };
            await ReplyAsync("", embed:eb).ConfigureAwait(false);
        }

        private static string GetUptime()
            => (DateTimeOffset.UtcNow - Process.GetCurrentProcess().StartTime).Humanize(5, true, maxUnit: TimeUnit.Month);
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).Megabytes().ToString();

    }
}
