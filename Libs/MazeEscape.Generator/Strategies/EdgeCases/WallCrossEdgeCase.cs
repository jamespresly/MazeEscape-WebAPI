using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Main;
using MazeEscape.Generator.Reference;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;

namespace MazeEscape.Generator.Strategies.EdgeCases;

internal class WallCrossEdgeCase : IEdgeCase
{
    private readonly MazeWriter _mazeWriter;

    public WallCrossEdgeCase(MazeWriter mazeWriter)
    {
        _mazeWriter = mazeWriter;
    }

    public bool Processed(MazeScan mazeScan)
    {
        var count = 0;

        foreach (var surroundView in mazeScan.Views)
        {
            if (IsWallCross(surroundView))
            {
                count++;
            }
        }

        if (count >= 2)
        {
            _mazeWriter.UpdateChar(mazeScan.Position.X, mazeScan.Position.Y, MazeChars.Wall);
            return true;
        }

        return false;
    }

    private bool IsWallCross(MazeView mazeView)
    {
        return mazeView.LookAhead.Ahead == MazeChars.Wall
               && (mazeView.LookAhead.AheadLeft == MazeChars.Corridor || mazeView.LookAhead.AheadLeft == GeneratorConsts.BorderChar)
               && (mazeView.LookAhead.AheadRight == MazeChars.Corridor || mazeView.LookAhead.AheadRight == GeneratorConsts.BorderChar);
    }
}