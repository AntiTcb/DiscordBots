using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestEase;

namespace OrgBot
{
    public interface IYugipediaApi
    {
        [Query("format")]
        string Format { get; set; }

        [Get("")]
        Task<JArray> SearchAsync([QueryMap] ApiParams @params, [Query("search")] string query);

        [Get("")]
        Task<JObject> GetCardAsync([QueryMap] ApiParams @params, [Query("subject")] string cardName);
    }

    public class ApiParams : Dictionary<string, string>
    {
        public ApiParams(string action)
        {
            this["action"] = action;
            this["format"] = "json";
        }

        public static ApiParams OpenSearchParams => new ApiParams("opensearch");
        public static ApiParams CardLookupParams => new ApiParams("browsebysubject");
    }
}
