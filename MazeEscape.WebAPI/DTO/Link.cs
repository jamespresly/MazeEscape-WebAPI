using Newtonsoft.Json;

namespace MazeEscape.WebAPI.DTO
{
    public class Link
    {
        public string Description { get; set; }
        public string Href { get; set; }
        public string Method { get; set; }

        [JsonIgnore]
        public string QueryParams { get; set; }
    }
}