#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 10:51 PM
// Last Revised: 09/14/2016 10:51 PM
// Last Revised by: Alex Gravely
#endregion
namespace BCL.Modules {
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;

    [Module("admin")]
    public class AdminModule {
        [Command("ban"), RequireContext(ContextType.Guild)]
        public async Task Ban(IUserMessage msg) {
            
        }
    }
}