using System.Threading.Tasks;
using Discord.Commands;
using DiscordBCL.Services;

namespace DiscordBCL.Modules
{
    public partial class OwnerModule
    {
        [Group("eval"), Hidden]
        public class EvalModule : ModuleBase<ShardedCommandContext>
        {
            public EvalService Eval { get; set; }

            [Command("=>"), Alias("exec")]
            public async Task EvalAsync([Remainder]string expr) 
                => await Eval.EvaluteAsync(Context, expr);
        }
    }
}
