#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/05/2016 7:12 PM
// Last Revised: 10/05/2016 7:12 PM
// Last Revised by: Alex Gravely
#endregion
namespace BCL.Interfaces {
    public interface IServerConfig {
        string CommandPrefix { get; set; }
    }
}