using System.Threading.Tasks;
using Discord.Commands;
using DiscordBCL.Services;

namespace DiscordBCL.Modules
{
    public partial class OwnerModule
    {
        [Group("eval")]
        public class EvalModule : ModuleBase<ShardedCommandContext>
        {
            private readonly EvalService _eval;
            public EvalModule(EvalService eval)
            {
                _eval = eval;
            }

            [Command, Alias("=>"), Priority(1)]
            public async Task EvalAsync([Remainder]string expr) 
                => await _eval.EvaluteAsync(Context, expr);
        }
    }
}
