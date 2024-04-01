using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Helper;
using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Main;
using MazeEscape.Generator.Reference;
using MazeEscape.Generator.Struct;

namespace MazeEscape.Generator.Strategies;

internal class RandomWalkStrategy : IWallBuildingStrategy
{
    private readonly MazeReader _mazeReader;
    private readonly MazeWriter _mazeWriter;
    private readonly SharedState _sharedState;

    private List<Vector> _remainingUnexploredOnPath;

    private int _moveForwardCount;
    private Vector _vector;


    public RandomWalkStrategy(MazeReader mazeReader, MazeWriter mazeWriter, SharedState sharedState)
    {
        _mazeReader = mazeReader;
        _mazeWriter = mazeWriter;
        _sharedState = sharedState;
    }

    public void Init(Coordinate startPosition)
    {
        _moveForwardCount = 0;
        _vector = new Vector(startPosition, RandomHelper.GetRandomDirection());

        _mazeWriter.WriteEmpty(_vector);

        _mazeWriter.CreateWallsAtSides(_vector);
    }

    public void NavigateAndBuild(char[][] mazeChars)
    {
        _remainingUnexploredOnPath = GetUnexploredConnectedToPath(mazeChars);

        while (_remainingUnexploredOnPath.Any())
        {
            var mazeViews = _mazeReader.GetScan(_vector, mazeChars);

            if (mazeViews.CanMoveForward)
            {
                if (_moveForwardCount % 2 == 0)
                {
                    var direction = GetDirection(_vector, mazeChars, mazeViews);
                    if (_vector.Direction != direction)
                    {
                        _vector.Direction = direction;
                        _moveForwardCount = 0;
                    }
                }

                _vector = MoveInDirection(_vector);
                _mazeWriter.CreateWallsAtSides(_vector);

                _moveForwardCount++;
            }
            else
            {
                if (mazeViews.ForwardView.IsUnvisitedAhead)
                {
                    _mazeWriter.CreateWallAhead(_vector);
                }

                if (mazeViews.ForwardView.IsDoubleWallBlockAhead)
                {
                    _mazeWriter.CreateCorridorAhead(_vector);
                }
            }

            if (mazeViews.CanMove)
            {
                if (!mazeViews.CanMoveForward || RandomHelper.GetRandomChance(4) && _moveForwardCount % 2 == 0)
                {
                    _vector.Direction = _mazeReader.GetNewDirection(mazeViews);

                    _moveForwardCount = 0;
                }
            }
            else
            {
                _vector = Relocate(_remainingUnexploredOnPath);
                _moveForwardCount = 0;
            }


            _remainingUnexploredOnPath = GetUnexploredConnectedToPath(mazeChars);
        }

    }

 
    private Direction GetDirection(Vector vector, char[][] mazeChars, MazeScan mazeScan)
    {

        if (mazeScan.LeftView.IsDoubleWallBlockAhead)
        {
            vector.Direction = mazeScan.LeftView.Direction;
        }

        if (mazeScan.RightView.IsDoubleWallBlockAhead)
        {
            vector.Direction = mazeScan.RightView.Direction;
        }

        if (mazeScan.LeftView.IsDoubleWallBlockAhead && mazeScan.RightView.IsDoubleWallBlockAhead)
        {
            var random = RandomHelper.GetRandomIntLessThan(2);

            vector.Direction = random == 0 ? mazeScan.LeftView.Direction : mazeScan.RightView.Direction;
        }

        return vector.Direction;
    }

  

    internal Vector Relocate(List<Vector> remainingUnexploredOnPath)
    {
        var index = RandomHelper.GetRandomIntLessThan(remainingUnexploredOnPath.Count);

        var unvisited = remainingUnexploredOnPath[index];
        var direction = unvisited.Direction;

        return new Vector(unvisited.X, unvisited.Y, direction);
    }

  

    internal Vector MoveInDirection(Vector vector)
    {
        var offsets = GeneratorMaps.DirectionMap[vector.Direction];

        var position = vector.Position;

        position.X += offsets.X;
        position.Y += offsets.Y;

        vector.Position = position;

        return vector;
    }


    internal List<Vector> GetUnexploredConnectedToPath(char[][] mazeChars)
    {
        var directions = GeneratorMaps.DirectionMap.ToList();

        var next = new List<Vector>();

        var corridorsWithAdjacentUnvisited = new List<Coordinate>();

        foreach (var corridor in _sharedState.CorridorsToCheck)
        {
            var hasAdjacentUnvisited = false;
            foreach (var direction in directions)
            {
                var adjacent = mazeChars[corridor.Y + direction.Value.Y][corridor.X + direction.Value.X];
                if (adjacent == GeneratorConsts.UnvisitedChar)
                {
                    hasAdjacentUnvisited = true;

                    next.Add(new Vector(corridor.X, corridor.Y, direction.Key));
                }
            }

            if (hasAdjacentUnvisited)
            {
                corridorsWithAdjacentUnvisited.Add(corridor);
            }
        }

        _sharedState.CorridorsToCheck = corridorsWithAdjacentUnvisited;

        return next;
    }

}