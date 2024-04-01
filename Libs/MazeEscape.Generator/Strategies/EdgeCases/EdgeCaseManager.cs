using System.Security.Cryptography;
using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Helper;
using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Main;
using MazeEscape.Generator.Struct;

namespace MazeEscape.Generator.Strategies.EdgeCases;

internal class EdgeCaseManager : IEdgeCaseManager
{
    private readonly List<IEdgeCase> _edgeCases;
    private readonly MazeReader _mazeReader;
    private readonly SharedState _sharedState;

    public EdgeCaseManager(List<IEdgeCase> edgeCases, MazeReader mazeReader, SharedState sharedState)
    {
        _edgeCases = edgeCases;
        _mazeReader = mazeReader;
        _sharedState = sharedState;
    }

    public bool ProcessEdgeCases(char[][] mazeChars)
    {
        foreach (var edgeCase in _edgeCases)
        {
            var processed = ProcessEdgeCase(mazeChars, edgeCase.Processed);
            if (processed)
            {
                return true;
            }
        }

        return false;
    }


    private bool ProcessEdgeCase(char[][] mazeChars, Func<MazeScan, bool> edgeCase)
    {
        var cantProcess = new List<Coordinate>();
        var processedAnEdgeCase = false;

        while (cantProcess.Count != _sharedState.Unvisited.Count)
        {
            var random = RandomNumberGenerator.GetInt32(_sharedState.Unvisited.Count);

            var position = _sharedState.Unvisited[random];
            var direction = RandomHelper.GetRandomDirection();

            var surround = _mazeReader.GetFullScan(new Vector(position, direction), mazeChars);

            processedAnEdgeCase = edgeCase(surround);

            if (processedAnEdgeCase)
            {
                break;
            }
            else
            {
                cantProcess.Add(position);
            }
        }

        return processedAnEdgeCase;
    }
}