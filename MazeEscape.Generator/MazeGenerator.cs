using System.Diagnostics;
using System.Security.Cryptography;
using MazeEscape.Engine.Struct;
using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;

namespace MazeEscape.Generator;

public class MazeGenerator : IMazeGenerator
{

    private readonly Dictionary<Direction, Offset> _directionMap = new()
    {
        { Direction.Up, new Offset(0, -1) },
        { Direction.Right, new Offset(1, 0) },
        { Direction.Down, new Offset(0, 1) },
        { Direction.Left, new Offset(-1, 0) },
    };

    private readonly Dictionary<Direction, Side> _sidesMap = new()
    {
        { Direction.Up, new Side(Direction.Left, Direction.Right)},
        { Direction.Right, new Side(Direction.Up,Direction.Down)},
        { Direction.Down, new Side(Direction.Right,Direction.Left)},
        { Direction.Left, new Side(Direction.Down,Direction.Up)},
    };

    private readonly char[] _doNotOverwrite = new[]
    {
        BorderChar, MazeChars.PlayerStart, MazeChars.Corridor
    };

    private const char UnvisitedChar = '=';
    private const char BorderChar = 'X';


    private char[][] _mazeChars;

    private int _posX = 0;
    private int _posY = 0;

    private int _relocateCount = 0;

    private List<Coordinate> _unvisited;
    private List<Coordinate> _corridorsToCheck;
    private List<Vector> _remainingUnexploredOnPath;



    public string GenerateRandom(int width, int height)
    {
        _unvisited = new();
        _corridorsToCheck = new();
        _remainingUnexploredOnPath = new();

        _posX = width / 2;
        _posY = height / 2;

        _mazeChars = CreateEmptyMaze(width, height);

        BuildWalls();

        _mazeChars = CreateExit(_mazeChars);

        _mazeChars[height / 2][width / 2] = MazeChars.PlayerStart;

        var concat = string.Join("\n", _mazeChars.Select(x => string.Concat(x)));

        var final = concat.Replace(BorderChar.ToString(), MazeChars.Wall.ToString());

        return final;
    }

    private char[][] CreateExit(char[][] maze)
    {


        var possibleExits = new List<Coordinate>();

        for (var y = 0; y < maze.Length; y++)
        {
            if (maze[y][1] == MazeChars.Corridor)
            {
                possibleExits.Add(new Coordinate(0, y));
            }

            if (maze[y][^2] == MazeChars.Corridor)
            {
                possibleExits.Add(new Coordinate(maze[0].Length - 1, y));
            }
        }

        for (var x = 0; x < maze[0].Length; x++)
        {
            if (maze[1][x] == MazeChars.Corridor)
            {
                possibleExits.Add(new Coordinate(x, 0));
            }

            if (maze[^2][x] == MazeChars.Corridor)
            {
                possibleExits.Add(new Coordinate(x, maze.Length - 1));
            }
        }

        var random = RandomNumberGenerator.GetInt32(possibleExits.Count);

        var exit = possibleExits[random];

        maze[exit.Y][exit.X] = MazeChars.Exit;

        return maze;

    }


    private char[][] CreateEmptyMaze(int width, int height)
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
                    chars[y][x] = BorderChar;
                }
                else
                {
                    chars[y][x] = UnvisitedChar;
                    _unvisited.Add(new Coordinate(x, y));
                }
            }
        }

        return chars;
    }


    private void BuildWalls()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var direction = GetRandomDirection();

        CreateWallsAtSides(direction);


        var relocated = false;


        while (_unvisited.Any())
        {
            _remainingUnexploredOnPath = GetUnexploredConnectedToPath();

            while (_remainingUnexploredOnPath.Any())
            {

                // make path
                if (CanMoveAhead(direction))
                {
                    relocated = false;

                    MoveInDirection(direction);
                    CreateWallsAtSides(direction);

                    if (!CanMoveAhead(direction) || GetRandomChance(4))
                    {
                        direction = GetNewDirection(direction);
                    }

                }
                else if (CanMoveLeftOrRight(direction))
                {
                    relocated = false;
                    direction = GetNewDirection(direction);
                }
                else
                {

                    if (relocated)
                    {
                        CreateWallAhead(direction);
                    }


                    if (_remainingUnexploredOnPath.Any())
                    {
                        direction = Relocate();
                        relocated = true;
                    }

                }

                _remainingUnexploredOnPath = GetUnexploredConnectedToPath();

                //DebugPrint();
            }

            var cantProcess = new List<Vector>();

            // edge cases
            if (_unvisited.Count > 0)
            {
                while (cantProcess.Count != (_unvisited.Count * 4))
                {
                    var random = RandomNumberGenerator.GetInt32(_unvisited.Count);

                    direction = GetRandomDirection();

                    var unvisited = _unvisited[random];

                    _posX = unvisited.X;
                    _posY = unvisited.Y;

                    if (cantProcess.Contains(new Vector(_posX, _posY, direction)))
                        continue;

                    var lookahead = GetLookAhead(direction);

                    while (lookahead.Ahead == UnvisitedChar)
                    {
                        MoveInDirection(direction);
                        lookahead = GetLookAhead(direction);
                    }

                    // Enclosure
                    //
                    //  +++++
                    //  +===+
                    //  +===+
                    //  +++++
                    //
                    if (lookahead.Ahead == MazeChars.Wall && lookahead.Ahead2 == MazeChars.Corridor)
                    {
                        CreateCorridorAhead(direction);
                        UpdateChar(_posX, _posY, MazeChars.Corridor);
                        break;
                    }

                    // Wall cross 
                    //
                    //     +
                    //     +
                    //  ++===++
                    //     +
                    //     +
                    //
                    if (lookahead.Ahead == MazeChars.Wall && lookahead.AheadLeft == MazeChars.Corridor && lookahead.AheadRight == MazeChars.Corridor)
                    {
                        UpdateChar(_posX, _posY, MazeChars.Wall);
                        break;
                    }

                    // Stuck in the corner - or against the boundary
                    //
                    //  X+
                    //  X+
                    //  X=++
                    //  XXXX
                    //
                    if (lookahead.Ahead == MazeChars.Wall && (lookahead.AheadLeft == BorderChar || lookahead.AheadRight == BorderChar))
                    {
                        UpdateChar(_posX, _posY, MazeChars.Corridor);
                        CreateCorridorAhead(direction);
                        break;
                    }

                    cantProcess.Add(new Vector(_posX, _posY, direction));

                    DebugPrint();

                    if (cantProcess.Count == (_unvisited.Count * 4))
                        throw new Exception("cant process remaining:" + cantProcess.Count + "\n" + DebugPrint());
                }


                //DebugPrint();
            }

        }
    }

    private bool GetRandomChance(int i)
    {
        var random = RandomNumberGenerator.GetInt32(i);

        return random == 0;
    }

    private Direction Relocate()
    {
        var index = RandomNumberGenerator.GetInt32(_remainingUnexploredOnPath.Count);

        var unvisited = _remainingUnexploredOnPath[index];

        _posX = unvisited.X;
        _posY = unvisited.Y;

        var direction = unvisited.Direction;

        if (_mazeChars[_posY][_posX] != MazeChars.Corridor)
        {
            DebugPrint();
            throw new Exception("invalid relocate");
        }

        _relocateCount++;

        return direction;
    }

    private Direction GetNewDirection(Direction direction)
    {

        var sides = _sidesMap[direction];

        var canMoveLeft = CanMoveAhead(sides.Left);
        var canMoveRight = CanMoveAhead(sides.Right);

        if (canMoveLeft && canMoveRight)
        {
            var random = RandomNumberGenerator.GetInt32(2);

            direction = random == 0 ? sides.Left : sides.Right;
        }
        else if (canMoveLeft || canMoveRight)
        {
            if (canMoveLeft)
                direction = sides.Left;

            if (canMoveRight)
                direction = sides.Right;
        }

        return direction;
    }

    private bool CanMoveLeftOrRight(Direction direction)
    {
        var sides = _sidesMap[direction];

        var canMoveLeft = CanMoveAhead(sides.Left);
        var canMoveRight = CanMoveAhead(sides.Right);

        return canMoveLeft || canMoveRight;
    }

    private bool CanMoveAhead(Direction direction)
    {
        var lookahead = GetLookAhead(direction);

        return lookahead.Ahead != BorderChar
               && lookahead.Ahead != MazeChars.Corridor
               && lookahead.Ahead2 != MazeChars.Corridor
               && lookahead.AheadLeft != MazeChars.Corridor
               && lookahead.AheadRight != MazeChars.Corridor;
    }

    private List<Vector> GetUnexploredConnectedToPath()
    {
        var directions = _directionMap.ToList();

        var next = new List<Vector>();

        var corridorsWithAdjacentUnvisited = new List<Coordinate>();

        foreach (var corridor in _corridorsToCheck)
        {
            var hasAdjacentUnvisited = false;
            foreach (var direction in directions)
            {
                var adjacent = _mazeChars[corridor.Y + direction.Value.Y][corridor.X + direction.Value.X];
                if (adjacent == UnvisitedChar)
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

        _corridorsToCheck = corridorsWithAdjacentUnvisited;

        return next;
    }


    private LookAhead GetLookAhead(Direction direction)
    {
        var aheadOffset = _directionMap[direction];

        var offsetList = _directionMap.ToList();
        var index = offsetList.IndexOf(new(direction, aheadOffset));

        var prevIndex = index == 0 ? offsetList.Count - 1 : index - 1;
        var nextIndex = (index + 1) % offsetList.Count;

        var leftOffset = offsetList[prevIndex].Value;
        var rightOffset = offsetList[nextIndex].Value;

        var leftAhead = _mazeChars[_posY + leftOffset.Y + aheadOffset.Y][_posX + leftOffset.X + aheadOffset.X];
        var rightAhead = _mazeChars[_posY + rightOffset.Y + aheadOffset.Y][_posX + rightOffset.X + aheadOffset.X];


        var ahead = _mazeChars[_posY + aheadOffset.Y][_posX + aheadOffset.X];

        var ahead2 = GetAhead2(aheadOffset);

        return new LookAhead()
        {
            Ahead = ahead,
            Ahead2 = ahead2,
            AheadLeft = leftAhead,
            AheadRight = rightAhead,
        };
    }

    private char GetAhead2(Offset aheadOffset)
    {
        var ahead2 = '0';

        var x = _posX + aheadOffset.X + aheadOffset.X;
        var y = _posY + aheadOffset.Y + aheadOffset.Y;

        if (x > 0 && y > 0 && x < _mazeChars[0].Length - 1 && y < _mazeChars.Length - 1)
        {
            ahead2 = _mazeChars[y][x];
        }

        return ahead2;
    }
    private Direction GetRandomDirection()
    {
        return (Direction)RandomNumberGenerator.GetInt32(4);
    }


    private void MoveInDirection(Direction direction)
    {
        var offsets = _directionMap[direction];

        _posX += offsets.X;
        _posY += offsets.Y;
    }

    private void CreateWallsAtSides(Direction direction)
    {
        var sides = _sidesMap[direction];

        CreateCharInDirection(sides.Left, MazeChars.Wall);
        CreateCharInDirection(sides.Right, MazeChars.Wall);

        UpdateChar(_posX, _posY, MazeChars.Corridor);
    }

    private void CreateWallAhead(Direction direction)
    {
        CreateCharInDirection(direction, MazeChars.Wall);
    }

    private void CreateCorridorAhead(Direction direction)
    {
        CreateCharInDirection(direction, MazeChars.Corridor);
    }



    private void CreateCharInDirection(Direction direction, char c)
    {
        var offset = _directionMap[direction];

        UpdateChar(_posX + offset.X, _posY + offset.Y, c);
    }


    private void UpdateChar(int x, int y, char c)
    {

        if (_doNotOverwrite.Contains(_mazeChars[y][x]))
            return;


        if (_mazeChars[y][x] == UnvisitedChar)
        {
            _unvisited.Remove(new Coordinate(x, y));
        }

        if (c == MazeChars.Corridor)
        {
            _corridorsToCheck.Add(new Coordinate(x, y));
        }

        _mazeChars[y][x] = c;

    }

    private string DebugPrint()
    {
        var p = _mazeChars[_posY][_posX];

        _mazeChars[_posY][_posX] = 'P';

        var concat = string.Join("\n", _mazeChars.Select(x => string.Concat(x)));
        Debug.WriteLine(concat);

        _mazeChars[_posY][_posX] = p;

        return concat;
    }


}