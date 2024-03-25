using System.Diagnostics;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Engine.Struct;
using System.Security.Cryptography;
using MazeEscape.Model.Constants;

namespace MazeEscape.Engine;

public class MazeGenerator : IMazeGenerator
{

    private readonly Dictionary<int, Offset> _directionMap = new()
    {
        { 0, new Offset(0, -1) },
        { 1, new Offset(1, 0) },
        { 2, new Offset(0, 1) },
        { 3, new Offset(-1, 0) },
    };

    private readonly Dictionary<int, Side> _sidesMap = new()
    {
        { 0, new Side(1,3)},
        { 1, new Side(0,2)},
        { 2, new Side(1,3)},
        { 3, new Side(0,2)},
    };

    private readonly char[] _doNotOverwrite = new[]
    {
        MazeChars.Corridor, BorderChar, MazeChars.PlayerStart
    };

    private const char UnvisitedChar = '=';
    private const char BorderChar = 'X';


    private char[][] _mazeChars;

    private int _posX = 0;
    private int _posY = 0;

    private int _relocateCount = 0;

    private List<Tuple<int, int>> _unvisited;
    private List<Tuple<int, int>> _corridorsToCheck;
    private List<Tuple<int, int, int>> _remainingUnexploredOnPath;



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


        var possibleExits = new List<Tuple<int, int>>();

        for (var y = 0; y < maze.Length; y++)
        {
            if (maze[y][1] == MazeChars.Corridor)
            {
                possibleExits.Add(new Tuple<int, int>(0, y));
            }

            if (maze[y][^2] == MazeChars.Corridor)
            {
                possibleExits.Add(new Tuple<int, int>(maze[0].Length - 1, y));
            }
        }

        for (var x = 0; x < maze[0].Length; x++)
        {
            if (maze[1][x] == MazeChars.Corridor)
            {
                possibleExits.Add(new Tuple<int, int>(x, 0));
            }

            if (maze[^2][x] == MazeChars.Corridor)
            {
                possibleExits.Add(new Tuple<int, int>(x, maze.Length - 1));
            }
        }

        var random = RandomNumberGenerator.GetInt32(possibleExits.Count);

        var exit = possibleExits[random];

        maze[exit.Item2][exit.Item1] = MazeChars.Exit;

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
                    _unvisited.Add(new Tuple<int, int>(x, y));
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

            var cantProcess = new List<Tuple<int, int, int>>();

            // edge cases
            if (_unvisited.Count > 0)
            {
                while (cantProcess.Count != (_unvisited.Count * 4))
                {
                    var random = RandomNumberGenerator.GetInt32(_unvisited.Count);

                    direction = GetRandomDirection();

                    var unvisited = _unvisited[random];

                    _posX = unvisited.Item1;
                    _posY = unvisited.Item2;

                    if (cantProcess.Contains(new Tuple<int, int, int>(_posX, _posY, direction)))
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

                    cantProcess.Add(new Tuple<int, int, int>(_posX, _posY, direction));

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

    private int Relocate()
    {
        var index = RandomNumberGenerator.GetInt32(_remainingUnexploredOnPath.Count);

        var unvisited = _remainingUnexploredOnPath[index];

        _posX = unvisited.Item1;
        _posY = unvisited.Item2;

        var direction = unvisited.Item3;

        if (_mazeChars[_posY][_posX] != MazeChars.Corridor)
        {
            DebugPrint();
            throw new Exception("invalid relocate");
        }

        _relocateCount++;

        return direction;
    }

    private int GetNewDirection(int direction)
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

    private bool CanMoveLeftOrRight(int direction)
    {
        var sides = _sidesMap[direction];

        var canMoveLeft = CanMoveAhead(sides.Left);
        var canMoveRight = CanMoveAhead(sides.Right);

        return canMoveLeft || canMoveRight;
    }

    private bool CanMoveAhead(int direction)
    {
        var lookahead = GetLookAhead(direction);

        return lookahead.Ahead != BorderChar
               && lookahead.Ahead != MazeChars.Corridor
               && lookahead.Ahead2 != MazeChars.Corridor
               && lookahead.AheadLeft != MazeChars.Corridor
               && lookahead.AheadRight != MazeChars.Corridor;
    }

    private List<Tuple<int, int, int>> GetUnexploredConnectedToPath()
    {
        var directions = _directionMap.ToList();

        var next = new List<Tuple<int, int, int>>();

        var corridorsWithAdjacentUnvisited = new List<Tuple<int, int>>();

        foreach (var corridor in _corridorsToCheck)
        {
            var hasAdjacentUnvisited = false;
            foreach (var direction in directions)
            {
                var adjacent = _mazeChars[corridor.Item2 + direction.Value.Y][corridor.Item1 + direction.Value.X];
                if (adjacent == UnvisitedChar)
                {
                    hasAdjacentUnvisited = true;

                    next.Add(new Tuple<int, int, int>(corridor.Item1, corridor.Item2, direction.Key));
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


    private LookAhead GetLookAhead(int direction)
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
    private int GetRandomDirection()
    {
        return RandomNumberGenerator.GetInt32(4);
    }


    private void MoveInDirection(int direction)
    {
        var offsets = _directionMap[direction];

        _posX += offsets.X;
        _posY += offsets.Y;
    }

    private void CreateWallsAtSides(int direction)
    {
        var sides = _sidesMap[direction];

        CreateCharInDirection(sides.Left, MazeChars.Wall);
        CreateCharInDirection(sides.Right, MazeChars.Wall);

        UpdateChar(_posX, _posY, MazeChars.Corridor);
    }

    private void CreateWallAhead(int direction)
    {
        CreateCharInDirection(direction, MazeChars.Wall);
    }

    private void CreateCorridorAhead(int direction)
    {
        CreateCharInDirection(direction, MazeChars.Corridor);
    }



    private void CreateCharInDirection(int direction, char c)
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
            _unvisited.Remove(new Tuple<int, int>(x, y));
        }

        if (c == MazeChars.Corridor)
        {
            _corridorsToCheck.Add(new Tuple<int, int>(x, y));
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