using MazeEscape.WebAPI.Enums;

namespace MazeEscape.WebAPI.DTO;

public class PlayerParams
{
    public string MazeToken { get; set; }

    public PlayerMove? PlayerMove { get; set; }
}