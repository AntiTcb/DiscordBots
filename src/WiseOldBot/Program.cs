#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/14/2016 4:09 AM
// Last Revised: 09/15/2016 1:03 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot {
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using BCL;
    using BCL.Interfaces;
    using BCL.Modules;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.DependencyModel;
    using OSRS;
    using TypeReaders;
    using Game = Discord.API.Game;

    #endregion

    public class Program {
        #region Public Methods
        public static void Main(string[] args) => new WiseOldBot().StartAsync<WiseOldBotConfig>().GetAwaiter().GetResult();

        #endregion Public Methods

       
    }
}