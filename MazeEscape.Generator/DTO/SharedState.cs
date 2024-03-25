using MazeEscape.Generator.Struct;

namespace MazeEscape.Generator.DTO;

internal class SharedState
{
    public char[][] MazeChars { get; set; }

    public List<Coordinate> Unvisited { get; set; }
    public List<Coordinate> CorridorsToCheck { get; set; }
}