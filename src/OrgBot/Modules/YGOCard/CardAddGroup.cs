#region Header
// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/23/2016 11:01 PM
// Last Revised: 10/23/2016 11:01 PM
// Last Revised by: Alex Gravely
#endregion
namespace OrgBot.Modules.YGOCard {
    using System.Collections.Generic;
    using Discord.Commands;
    using Entities;

    public partial class YGOCardModule {

        [DontAutoLoad]
        public class CardAddGroup : ModuleBase {
            //static Dictionary<IUser, YGOCard> NewCards { get; set; } -- Collection TBD
            //public async Task StartCreateCardAsync() { }
            //public async Task SaveCreateCardAsync() { }
            //public async Task DeleteCreateCardASync() { }
            //public asyn Task SetCardDetailsAsync() { } -- Broken into separate commands for each one, akin to edit?
        }
    }
}