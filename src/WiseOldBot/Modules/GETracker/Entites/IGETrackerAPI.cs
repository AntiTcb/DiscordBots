namespace WiseOldBot.Modules.GETracker.Entities
{
    using RestEase;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGETrackerAPI
    {
        [Get("items/{itemId}")]
        Task<GETrackerItem.Wrapper> DownloadItemAsync([Path("itemId")] int itemId);

        [Get("items")]
        Task<Dictionary<string, List<GETrackerItem>>> DownloadItemsAsync();
    }
}