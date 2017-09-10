namespace BCL.Interfaces
{
    using System.Threading.Tasks;
    using Discord;
    using Discord.WebSocket;

    public interface IBotBase {
        DiscordSocketClient Client { get; set; }
        CommandHandler Commands { get; set; }

        Task StartAsync<T>() where T : IBotConfig, new();
        Task HandleConfigsAsync<T>() where T : IBotConfig, new();
        Task InstallCommandsAsync();
        Task LoginAndConnectAsync(TokenType tokenType);
    }
}