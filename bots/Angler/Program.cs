using System.Threading.Tasks;

namespace Angler
{
    public class Program
    {
        static Task Main(string[] args) 
            => new Angler(args.Length == 1 ? int.Parse(args[0]) : 1).RunAsync();
    }
}
