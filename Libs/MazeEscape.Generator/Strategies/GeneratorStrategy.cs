using System.Security.Cryptography;
using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Reference;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;

namespace MazeEscape.Generator.Strategies;

internal class GeneratorStrategy : IGeneratorStrategy
{
    
    private readonly IWallBuildingStrategy _wallBuildingStrategy;
    private readonly IEdgeCaseManager _edgeCaseManager;
    public SharedState SharedState { get; set; }


    public GeneratorStrategy(IWallBuildingStrategy wallBuildingStrategy, IEdgeCaseManager edgeCaseManager, SharedState sharedState)
    {
        _wallBuildingStrategy = wallBuildingStrategy;
        _edgeCaseManager = edgeCaseManager;

        SharedState = sharedState;
    }

    public char[][] BuildWalls(Coordinate startPosition, char[][] mazeChars)
    {
        _wallBuildingStrategy.Init(startPosition);

        while (SharedState.Unvisited.Any())
        {
            _wallBuildingStrategy.NavigateAndBuild(mazeChars);

            _edgeCaseManager.ProcessEdgeCases(mazeChars);
        }

        return mazeChars;
    }

    public char[][] CreateEmpty(int width, int height)
    {
        var chars = new char[height][];

        for (var h = 0; h < chars.Length; h++)
        {
            chars[h] = new char[width];
        }

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                {
                    chars[y][x] = GeneratorConsts.BorderChar;
                }
                else
                {
                    chars[y][x] = GeneratorConsts.UnvisitedChar;
                    SharedState.Unvisited.Add(new Coordinate(x, y));
                }
            }
        }

        return chars;
    }

    public Coordinate GetPlayerStart(char[][] mazeChars)
    {
        var position = new Coordinate(mazeChars[0].Length / 2, mazeChars.Length / 2);
        return position;
    }

    public Coordinate GetExit(char[][] mazeChars)
    {
        var possibleExits = new List<Coordinate>();

        for (var y = 0; y < mazeChars.Length; y++)
        {
            if (mazeChars[y][1] == MazeChars.Corridor)
            {
                possibleExits.Add(new Coordinate(0, y));
            }

            if (mazeChars[y][^2] == MazeChars.Corridor)
            {
                possibleExits.Add(new Coordinate(mazeChars[0].Length - 1, y));
            }
        }

        for (var x = 0; x < mazeChars[0].Length; x++)
        {
            if (mazeChars[1][x] == MazeChars.Corridor)
            {
                possibleExits.Add(new Coordinate(x, 0));
            }

            if (mazeChars[^2][x] == MazeChars.Corridor)
            {
                possibleExits.Add(new Coordinate(x, mazeChars.Length - 1));
            }
        }

        var random = RandomNumberGenerator.GetInt32(possibleExits.Count);

        var exit = possibleExits[random];

        return exit;
    }

    public char[][] FinishMaze(Coordinate playerStart, Coordinate exit)
    {
        SharedState.MazeChars[playerStart.Y][playerStart.X] = MazeChars.PlayerStart;
        SharedState.MazeChars[exit.Y][exit.X] = MazeChars.Exit;

        return SharedState.MazeChars;
    }

}