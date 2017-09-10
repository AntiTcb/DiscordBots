
namespace OrgBot {
    public class Program {

        public static void Main(string[] args) => new OrgBot().StartAsync<OrgBotConfig>().GetAwaiter().GetResult();

    }
}