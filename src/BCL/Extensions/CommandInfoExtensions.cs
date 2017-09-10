namespace BCL.Extensions
{
    using Discord.Commands;
    using System.Linq;

    public static class CommandInfoExtensions
    {
        public static bool CanExecute(this CommandInfo cmd, ICommandContext ctx) => (cmd.CheckPreconditionsAsync(ctx)).GetAwaiter().GetResult().IsSuccess;

        public static bool CanExecute(this ModuleInfo mod, ICommandContext ctx) => mod.Commands.Any(c => c.CanExecute(ctx));        
    }
}
