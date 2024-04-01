using MazeEscape.Generator.Struct;

namespace MazeEscape.Generator.DTO;

public class SharedState
{
    public char[][] MazeChars { get; set; }

    public List<Coordinate> Unvisited { get; set; }
    public List<Coordinate> CorridorsToCheck { get; set; }

    public List<string> DebugSteps { get; set; }
    public bool Debug { get; set; }
}