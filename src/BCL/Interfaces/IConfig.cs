#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/27/2016 1:48 AM
// Last Revised: 09/27/2016 1:48 AM
// Last Revised by: Alex Gravely
#endregion
namespace BCL.Interfaces {
    public interface IConfig {
        string BotToken { get; set; }
        ulong LogChannel { get; set; }
    }
}