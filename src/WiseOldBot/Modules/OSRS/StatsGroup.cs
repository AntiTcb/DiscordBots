namespace WiseOldBot.Modules.OSRS
{
    using System.Threading.Tasks;
    using BCL;
    using BCL.Preconditions;
    using Discord;
    using Discord.Commands;
    using Entities;

    public partial class OSRSModule
    {
        [Name("OSRS Stats")]
        public class StatsGroup : ModuleBase<SocketCommandContext>
        {
            private WiseOldBotConfig _config => (WiseOldBotConfig)Globals.BotConfig;
            
            [Command("lookup"), Alias("l", "stats", "hs", "highscores"), Priority(5)]
            [Summary("Looks up an account from the high scores, with various skill and game mode options.")]
            [Remarks("lookup regular all anti-tcb")]
            public async Task GetStatsAsync(
                [Summary("Game Mode")] GameMode gameMode = GameMode.Regular, 
                [Summary("Skill")] SkillType skill = SkillType.All,
                [Summary("Account username"), Remainder]string playerName = "")
            {
                if (string.IsNullOrEmpty(playerName) && !_config.UsernameMap.TryGetValue(Context.User.Id, out playerName))
                {
                    await ReplyAsync("You must specify a username, or set a default username using the `defname` command.");
                    return;
                }
                await GetHighscoresAsync(playerName, skill, gameMode);
            }

            [Command("lookup"), Alias("l", "stats", "hs", "highscores"), Priority(2)]
            public Task GetStatsAsync(GameMode mode, [Remainder] string playerName = "")
                => GetStatsAsync(mode, SkillType.All, playerName);

            [Command("lookup"), Alias("l", "stats", "hs", "highscores"), Priority(3)]
            public Task GetStatsAsync(SkillType skill, [Remainder] string playerName = "")
                => GetStatsAsync(GameMode.Regular, skill, playerName);

            [Command("lookup"), Alias("l", "stats", "hs", "highscores"), Priority(1)]
            public Task GetStatsAsync([Remainder] string playerName = "")
                => GetStatsAsync(GameMode.Regular, SkillType.All, playerName);


            [Command("defname"), Alias("account", "setusername"), Priority(2)]
            [Summary("Sets a default RuneScape username.")]
            [Remarks("defname anti-tcb")]
            [RequireRole(179649074260082688)]
            public Task SetUsernameAsync(
                [Summary("Person to set the username for")] IUser target,
                [Remainder, Summary("RuneScape username")] string username)
                => ManageUsernameMapAsync(target, username);            

            [Command("defname"), Alias("account", "setusername"), Priority(1)]
            [Summary("Sets a default RuneScape username.")]
            [Remarks("defname anti-tcb")]
            public Task SetUsernameAsync([Remainder, Summary("RuneScape username")] string username)
                => ManageUsernameMapAsync(Context.User, username);

            private async Task ManageUsernameMapAsync(IUser user, string username)
            {
                switch (username)
                {
                    case "--unset":
                    case "--remove":
                        _config.UsernameMap.Remove(user.Id);
                        await ReplyAsync($"Removed username for {user}");
                        break;
                    default:
                        _config.UsernameMap[user.Id] = username;
                        await ReplyAsync($"Username for {user} set to `{username}`");
                        break;
                }
                await ConfigHandler.SaveAsync(Globals.CONFIG_PATH, Globals.BotConfig);
            }

            private async Task<IUserMessage> GetHighscoresAsync(string username, SkillType skill, GameMode mode)
            {
                var player = await OSRSAPIClient.DownloadStatsAsync(username, mode);
                return player == null
                    ? await ReplyAsync($"Could not find highscores for {username}")
                    : await ReplyAsync("", embed: player.SkillToDiscordEmbed(skill).Build());
            }
        }
    }
}