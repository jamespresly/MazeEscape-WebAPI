using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Main;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;

namespace MazeEscape.Generator.Strategies.EdgeCases;

internal class EnclosureEdgeCase : IEdgeCase
{
    private readonly MazeWriter _mazeWriter;

    public EnclosureEdgeCase(MazeWriter mazeWriter)
    {
        _mazeWriter = mazeWriter;
    }
    public bool Processed(MazeScan mazeScan)
    {
        foreach (var surroundView in mazeScan.Views)
        {
            if (IsEnclosure(surroundView))
            {
                _mazeWriter.CreateCorridorAhead(new Vector(mazeScan.Position, surroundView.Direction));
                return true;
            }
        }
        return false;
    }

    private bool IsEnclosure(MazeView mazeView)
    {
        return mazeView.LookAhead.Ahead == MazeChars.Wall
               && mazeView.LookAhead.AheadLeft != MazeChars.Corridor
               && mazeView.LookAhead.AheadRight != MazeChars.Corridor;

    }
}