using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBCL
{
    public class UriTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (Uri.TryCreate(input, UriKind.RelativeOrAbsolute, out var uri))
                return Task.FromResult(TypeReaderResult.FromSuccess(uri));
            else
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Could not parse the input as a Uri."));
        }
    }
}
