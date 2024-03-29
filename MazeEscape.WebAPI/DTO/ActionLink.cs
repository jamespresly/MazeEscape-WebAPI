using Newtonsoft.Json;

namespace MazeEscape.WebAPI.DTO;

public class ActionLink : Link
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 4)]
    public object Body { get; set; }
}