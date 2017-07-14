using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Discord.Commands;
using System.Linq;

namespace DiscordBCL
{
    internal class CommandInfoTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> Read(ICommandContext context, string input, IServiceProvider services)
        {
            var cmdService = services.GetRequiredService<CommandService>();
            var cmd = cmdService.Commands.FirstOrDefault(c => c.Aliases.Any(a => 
                string.Equals(a, input, StringComparison.OrdinalIgnoreCase) && c.CanExecute(context, services)));

            if (cmd == null)
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ObjectNotFound, "Command was not found."));
            else
                return Task.FromResult(TypeReaderResult.FromSuccess(cmd));
        }
    }
}
