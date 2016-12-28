// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 10/11/2016 6:18 PM
// Last Revised: 10/19/2016 6:16 PM
// Last Revised by: Alex Gravely

namespace WiseOldBot {

    public class Program {

        public static void Main(string[] args)
            => new WiseOldBot().StartAsync<WiseOldBotConfig>().GetAwaiter().GetResult();
    }
}