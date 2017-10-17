using System.Threading.Tasks;
using Discord;

namespace WiseOldBot
{
    public class Program
    {
        static Task Main(string[] args)
            => new WiseOldBot(args.Length == 1 ? int.Parse(args[0]) : 1).RunAsync();
    }
}
