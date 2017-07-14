using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBCL
{
    internal class ModuleInfoTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> Read(ICommandContext context, string input, IServiceProvider services)
        {
            var cmdService = services.GetRequiredService<CommandService>();
            var module = cmdService.Modules.FirstOrDefault(m => 
                string.Equals(m.Name, input, StringComparison.OrdinalIgnoreCase) && m.CanExecute(context, services));

            if (module == null)
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ObjectNotFound, "Module was not found."));
            else
                return Task.FromResult(TypeReaderResult.FromSuccess(module));
        }
    }
}
