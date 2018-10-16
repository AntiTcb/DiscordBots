namespace WiseOldBot.Modules.GETracker.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    public class ItemMap : Dictionary<string, List<GETrackerItem>>
    {
        public ItemMap() { }

        public ItemMap(IDictionary<string, List<GETrackerItem>> dict) : base(dict) { }

        public IReadOnlyCollection<GETrackerItem> FindItemOrItems(string itemName)
        {
            itemName = itemName.ToLower();
            if (TryGetValue(itemName, out var returnItems))
            {
                return returnItems;
            }
            return FindItems(itemName);
        }

        public IReadOnlyCollection<GETrackerItem> FindItems(string itemName)
            => this.Where(x => x.Key.Contains(itemName.ToLower())).SelectMany(kvp => kvp.Value).ToArray();
    }
}