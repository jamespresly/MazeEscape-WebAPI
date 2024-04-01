using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Main;
using MazeEscape.Generator.Reference;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;

namespace MazeEscape.Generator.Strategies.EdgeCases;

internal class PartEnclosureEdgeCase : IEdgeCase
{
    private readonly MazeWriter _mazeWriter;

    public PartEnclosureEdgeCase(MazeWriter mazeWriter)
    {
        _mazeWriter = mazeWriter;
    }
    public bool Processed(MazeScan mazeScan)
    {
        foreach (var surroundView in mazeScan.Views)
        {
            if (IsPartEnclosure(surroundView))
            {
                _mazeWriter.CreateCorridorAhead(new Vector(mazeScan.Position, surroundView.Direction));
                return true;
            }
        }
        return false;
    }

    private bool IsPartEnclosure(MazeView mazeView)
    {
        return mazeView.LookAhead.Ahead == MazeChars.Wall && mazeView.LookAhead.Ahead2 == MazeChars.Wall 
               && ((mazeView.LookAhead.AheadLeft == MazeChars.Corridor && mazeView.LookAhead.AheadRight != MazeChars.Corridor)
                   
                   || (mazeView.LookAhead.AheadLeft != MazeChars.Corridor && mazeView.LookAhead.AheadRight == MazeChars.Corridor));
    }


}