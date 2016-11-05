#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BotTests
// 
// Created: 11/04/2016 4:37 PM
// Last Revised: 11/04/2016 4:37 PM
// Last Revised by: Alex Gravely
#endregion
namespace BotTests.Mocks {
    using Discord;
    using Discord.API;
    using Discord.WebSocket;

    public static class Users {
        public static IUser PublicUser => new MockUser {
            Id = 89613772372574208,
            Username = "AntiTcb",
            Discriminator = "1720"
        };
    }
}