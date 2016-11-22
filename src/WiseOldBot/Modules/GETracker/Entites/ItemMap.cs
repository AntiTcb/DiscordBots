#region Header

// Description:
// 
// Solution: DiscordBots
// Project: WiseOldBot
// 
// Created: 09/20/2016 12:17 PM
// Last Revised: 09/20/2016 9:42 PM
// Last Revised by: Alex Gravely

#endregion

namespace WiseOldBot.Modules.GETracker.Entities {
    #region Using

    using System.Collections.Generic;
    using System.Linq;

    #endregion 

    public class ItemMap : Dictionary<string, List<GETrackerItem>> {
        #region Public Constructors

        public ItemMap() { }

        public ItemMap(IDictionary<string, List<GETrackerItem>> dict) : base(dict) { }

        internal IEnumerable<GETrackerItem> FindItems(string partialKey)
            => this.Where(x => x.Key.Contains(partialKey.ToLower())).SelectMany(kvp => kvp.Value);

        internal IEnumerable<GETrackerItem> FindItemOrItems(string itemName) {
            itemName = itemName.ToLower();
            if (ContainsKey(itemName)) {
                return this[itemName];
            }
            return FindItems(itemName);
        }

        #endregion Public Constructors
    }
}