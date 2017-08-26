using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Humanizer;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBCL.Services
{
    public sealed class CommandHandlingService 
    {
        private readonly DiscordShardedClient _discord;
        private readonly CommandService _cmdService;
        private readonly GuildConfigService _gConfigService;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services, DiscordShardedClient discord, CommandService cmdService, GuildConfigService gConfigService)
        {
            _services = services;
            _discord = discord;
            _cmdService = cmdService;
            _gConfigService = gConfigService;
            _discord.MessageReceived += MessageReceived;
            _cmdService.Log += OnCommandLogAsync;
        }

        private async Task OnCommandLogAsync(LogMessage arg) {
            string msg = arg.Exception?.ToString() ?? arg.Message;
            if (arg.Exception is CommandException cmdEx)
            {
                msg = $"{cmdEx.InnerException.GetType().Name}: {cmdEx.InnerException.Message}";
                var eb = new EmbedBuilder
                {
                    Title = msg,
                    Color = Color.DarkRed,
                    Description = Format.Sanitize(cmdEx.InnerException.StackTrace).Truncate(5500)
                };
                await cmdEx.Context.Channel.SendMessageAsync("", embed: eb).ConfigureAwait(false);
            }
            await PrettyConsole.LogAsync(arg.Severity, arg.Source, msg).ConfigureAwait(false);
        }

        public async Task InitializeAsync()
        {
            PrettyConsole.WriteLine("Loading Command Handling Service...");
            _cmdService.AddTypeReader<CommandInfo>(new CommandInfoTypeReader());
            _cmdService.AddTypeReader<ModuleInfo>(new ModuleInfoTypeReader());
            _cmdService.AddTypeReader<SocketGuild>(new GuildTypeReader<SocketGuild>());
            _cmdService.AddTypeReader<RestGuild>(new GuildTypeReader<RestGuild>());

            await _cmdService.AddModulesAsync(Assembly.GetEntryAssembly()).ConfigureAwait(false);
            await _cmdService.AddModulesAsync(typeof(BotBase).GetTypeInfo().Assembly).ConfigureAwait(false);
        }

        private async Task MessageReceived(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage usrMsg && usrMsg.Source == MessageSource.User)) return;

            int argPos = 0;

            if (!HasPrefix(usrMsg, ref argPos)) return;

            var context = new ShardedCommandContext(_discord, usrMsg);
            var result = await _cmdService.ExecuteAsync(context, argPos, _services).ConfigureAwait(false);

            if (result.IsSuccess) return;

            switch (result)
            {
                case ParseResult parse:
                    await usrMsg.Channel.SendMessageAsync($"**Parse Error:** {parse.Error.Humanize()} {parse.ErrorReason}").ConfigureAwait(false); 
                    break;
                case TypeReaderResult typeRead:
                    await usrMsg.Channel.SendMessageAsync($"**Typereader Error:** {typeRead.ErrorReason}").ConfigureAwait(false);
                    break;
                case PreconditionGroupResult preconGroup:
                    await usrMsg.Channel.SendMessageAsync($"**Precondition Group Error:** {preconGroup.ErrorReason}").ConfigureAwait(false);
                    break;
                case PreconditionResult precon:
                    await usrMsg.Channel.SendMessageAsync($"**Precondition Error:** {precon.ErrorReason}").ConfigureAwait(false);
                break;
                case SearchResult search:
                    await usrMsg.Channel.SendMessageAsync($"**Search Error:** {search.ErrorReason}").ConfigureAwait(false);
                    break;
                case ExecuteResult execute:
                    await usrMsg.Channel.SendMessageAsync($"**Execute Result:** {execute.ErrorReason}").ConfigureAwait(false);
                    break;
            }
        }

        private bool HasPrefix(IUserMessage msg, ref int argPos)
        {
            //Mention prefixes are valid in all contexts, check it first
            if (msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
                return true;

            if (msg is IGuildChannel gChan)                                                        
                return msg.HasStringPrefix(_gConfigService.GetConfig(gChan.Id).Prefix, ref argPos);
            else
                return false;
        }
    }
}
