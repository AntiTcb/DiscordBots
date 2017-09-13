namespace WiseOldBot.Modules.GETracker.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    public class ItemMap : Dictionary<string, List<GETrackerItem>>
    {
        public ItemMap() { }

        public ItemMap(IDictionary<string, List<GETrackerItem>> dict) : base(dict) { }

        public IEnumerable<GETrackerItem> FindItemOrItems(string itemName)
        {
            itemName = itemName.ToLower();
            List<GETrackerItem> returnItems;
            if (TryGetValue(itemName, out returnItems))
            {
                return returnItems;
            }
            return FindItems(itemName);
        }

        public IEnumerable<GETrackerItem> FindItems(string itemName)
            => this.Where(x => x.Key.Contains(itemName.ToLower())).SelectMany(kvp => kvp.Value);
    }
}