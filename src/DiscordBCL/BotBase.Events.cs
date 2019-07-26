using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordBCL
{
    public partial class BotBase
    {
        protected virtual Task LogMessageAsync(LogMessage logMsg)
            => PrettyConsole.LogAsync(logMsg.Severity, logMsg.Source, logMsg.Exception?.ToString() ?? logMsg.Message);

        protected virtual Task CreateGuildConfigAsync(SocketGuild guild)
        {
            _guildConfigService.AddConfig(guild.Id, "!");
            return Task.CompletedTask;
        }
        protected virtual Task ValidateGuildConfigAsync(SocketGuild guild)
        {
            if (_guildConfigService.GetConfig(guild.Id) == null)
                CreateGuildConfigAsync(guild);
            return Task.CompletedTask;
        }
        protected virtual Task RemoveGuildConfigAsync(SocketGuild guild)
        {
            _guildConfigService.RemoveConfig(guild.Id);
            return Task.CompletedTask;
        }
    }
}
