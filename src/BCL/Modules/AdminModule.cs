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
    using Interfaces;

    [Module("admin", AutoLoad = false)]
    public class AdminModule {
        IBotConfig _botConfig;
        public AdminModule(IBotConfig botConfig) {
            _botConfig = botConfig;
        }

        [Command("ban"), RequireContext(ContextType.Guild), RequirePermission(GuildPermission.BanMembers)]
        public async Task BanAsync(IUserMessage msg, IUser userToBan) => await (msg.Channel as IGuildChannel)?.Guild.AddBanAsync(userToBan);

        [Command("setprefix"), RequirePermission(ChannelPermission.ManageChannel)]
        public async Task ChangeBotPrefixAsync(IUserMessage msg, string prefix) {
            _botConfig.CommandPrefix = char.Parse(prefix);
            ConfigHandler.SaveAsync(BotBase.CONFIG_PATH, _botConfig);
        }

        [Command("dismiss"), Alias("leave"), Remarks("Instructs the bot to leave this Guild"),
         RequireContext(ContextType.Guild), RequirePermission(GuildPermission.ManageGuild)]
        public async Task Dismiss(IUserMessage msg)
        {
            var guild = (msg.Channel as IGuildChannel)?.Guild;
            await msg.Channel.SendMessageAsync("I have been dismissed! Ta-ta everyone!");
            await guild?.LeaveAsync();
        }

    }
}