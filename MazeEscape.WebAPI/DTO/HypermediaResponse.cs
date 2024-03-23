using Newtonsoft.Json;

namespace MazeEscape.WebAPI.DTO
{
    public class HypermediaResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ActionLink> Actions { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Link> Links { get; set; }
    }
}
