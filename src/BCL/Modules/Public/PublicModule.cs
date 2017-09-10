// Description:
//
// Solution: DiscordBots
// Project: BCL
//
// Created: 10/15/2016 11:36 PM
// Last Revised: 10/15/2016 11:36 PM
// Last Revised by: Alex Gravely

namespace BCL.Modules.Public
{
    using BCL.Comparers;
    using BCL.Extensions;
    using BCL.Modules.Public.Services;
    using Discord;
    using Discord.Commands;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    [Name("Public")]
    public partial class PublicModule : ModuleBase<SocketCommandContext>
    {
        public CommandService CommandService { get; set; }

        [Command("help"),
            Alias("commands", "command", "cmds", "cmd"),
            Summary("Information about the bot's commands.")]
        public async Task HelpAsync([Remainder, Summary("Command/Module name to search for")]string name = "")
        {
            var modules = CommandService.Modules.OrderBy(x => x.Name);
            var commands = modules.SelectMany(m => m.Commands.Select(x => x).Distinct(new CommandNameComparer()));

            var cmd = commands.FirstOrDefault(x => x.Aliases.Contains(name.ToLower()));
            var module = modules.FirstOrDefault(x => x.Name.ToLower().Contains(name.ToLower()));
            var helpMode = name == "" ? HelpMode.All : cmd != null ? HelpMode.Command : module != null ? HelpMode.Module : HelpMode.All;

            switch (helpMode)
            {
                case HelpMode.All:
                    var errMsg = name == "" ? "" :
                        module == null && cmd == null ? "Module/Command not found, showing generic help instead." : "";
                    await ReplyAsync(errMsg, embed: HelpService.GetGenericHelpEmbed(modules, Context).WithAuthor(Context.Client.CurrentUser).Build());
                    break;

                case HelpMode.Module:
                    if (!module.CanExecute(Context))
                    {
                        await ReplyAsync("You do not have permission to see information for this module.");
                        return;
                    }
                    await ReplyAsync("", embed: HelpService.GetModuleHelpEmbed(module, Context).WithAuthor(Context.Client.CurrentUser).Build());
                    break;

                case HelpMode.Command:
                    if (!cmd.CanExecute(Context))
                    {
                        await ReplyAsync("You do not have permission to see information for this command.");
                        return;
                    }
                    await ReplyAsync("", embed: HelpService.GetCommandHelpEmbed(cmd, Context).WithAuthor(Context.Client.CurrentUser).Build());
                    break;
            }
        }

        [Command("info"), Summary("Information about the bot.")]
        public async virtual Task InfoAsync()
        {
            var app = await Context.Client.GetApplicationInfoAsync().ConfigureAwait(false);

            await ReplyAsync($"{Format.Bold("Info")}\n" + $"- Author: {app.Owner} (ID: {app.Owner.Id})\n" +
                     $"- Repo: <https://github.com/AntiTcb/DiscordBots/tree/master/src/{app.Name}>\n" +
                     $"- Assembly: {Assembly.GetEntryAssembly().GetName().Name} {Assembly.GetEntryAssembly().GetName().Version}\n" +
                     $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                     $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}\n" +
                     $"- Uptime: {GetUptime()}\n\n" + $"{Format.Bold("Stats")}\n" + $"- Heap Size: {GetHeapSize()} MB\n" +
                     $"- Guilds: {Context.Client.Guilds.Count}\n" +
                     $"- Channels: {Context.Client.Guilds.Sum(g => g.Channels.Count)} " +
                     $"- Users: Online - {Context.Client.Guilds.Sum(g => g.Users.Count)} | Total - {Context.Client.Guilds.Sum(g => g.MemberCount)}").ConfigureAwait(false);
        }

        [Command("join"), Alias("invite"), Summary("Returns the Invite URL of the bot.")]
        public async virtual Task JoinAsync()
        {
            var app = await Context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
            await ReplyAsync($"<https://discordapp.com/oauth2/authorize?permissions=67496960&client_id={app.Id}&scope=bot>").ConfigureAwait(false);
        }                         

        static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString(CultureInfo.InvariantCulture);

        static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\ hh\:mm\:ss");
    }
}