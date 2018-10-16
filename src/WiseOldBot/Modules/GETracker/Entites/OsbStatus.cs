using Newtonsoft.Json;

namespace WiseOldBot
{
    public class OsbStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("health")]
        public float Health { get; set; }
        [JsonProperty("updateFrequency")]
        public int UpdateFrequency { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }


        public class Wrapper
        {
            [JsonProperty("data")]
            public OsbStatus Data { get; set; }
        }
    }
}
