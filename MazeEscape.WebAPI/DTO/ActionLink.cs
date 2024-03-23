using Newtonsoft.Json;

namespace MazeEscape.WebAPI.DTO;

public class ActionLink : Link
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public object Body { get; set; }
}