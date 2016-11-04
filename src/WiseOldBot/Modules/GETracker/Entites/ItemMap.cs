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

namespace WiseOldBot.Modules.GETracker {
    #region Using

    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class ItemMapExtensions {
        #region Public Methods

        public static IEnumerable<GETrackerItem> PartialMatch(this ItemMap map, string partialKey)
            => map.Where(x => x.Key.Contains(partialKey.ToLower())).SelectMany(kvp => kvp.Value);

        #endregion Public Methods
    }

    public class ItemMap : Dictionary<string, List<GETrackerItem>> {
        #region Public Constructors

        public ItemMap() { }

        public ItemMap(IDictionary<string, List<GETrackerItem>> dict) : base(dict) { }

        #endregion Public Constructors
    }
}