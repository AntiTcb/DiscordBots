namespace BCL.Interfaces {
    public interface IBotConfig {

        string BotToken { get; set; }
        ulong LogChannel { get; set; }

    }
}