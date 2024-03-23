using System.Text.Json.Serialization;

namespace MazeEscape.WebAPI.DTO;

public class MazeCreated
{
    public string MazeToken { get; set; }
    [JsonIgnore]
    public int Width { get; set; }
    [JsonIgnore]
    public int Height { get; set; }

}