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
    using System;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Interfaces;

    [Module("admin")]
    public class AdminModule {
        IConfig _config;
        public AdminModule(IConfig config) {
            _config = config;
        }

        [Command("ban"), RequireContext(ContextType.Guild), RequirePermission(GuildPermission.BanMembers)]
        public async Task BanAsync(IUserMessage msg, IUser userToBan) => await (msg.Channel as IGuildChannel)?.Guild.AddBanAsync(userToBan);

        [Command("setprefix"), RequirePermission(ChannelPermission.ManageChannel)]
        public async Task ChangeBotPrefixAsync(IUserMessage msg, char prefix) {
            _config.CommandPrefix = prefix;
            ConfigHandler.Save(BotBase.CONFIG_PATH, _config);
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