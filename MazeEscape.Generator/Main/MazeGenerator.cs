using System.Diagnostics;
using System.Security.Cryptography;
using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Reference;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;

namespace MazeEscape.Generator.Main;

public class MazeGenerator : IMazeGenerator
{

    private bool _debug = false;
    private List<string> _debugSteps;

    private MazeReader _mazeReader;
    private MazeWriter _mazeWriter;
    private Navigator _navigator;

    private SharedState _sharedState;

    private List<Vector> _remainingUnexploredOnPath;


    public List<string> GenerateRandomWithDebugSteps(int width, int height)
    {
        _debug = true;

        _debugSteps = new List<string>();

        var final = GenerateRandom(width, height);

        _debugSteps.Add(final);

        return _debugSteps;
    }

    public string GenerateRandom(int width, int height)
    {
        _remainingUnexploredOnPath = new();

        _sharedState = new SharedState()
        {
            CorridorsToCheck = new List<Coordinate>(),
            Unvisited = new List<Coordinate>(),
        };

        _mazeReader = new MazeReader();
        _mazeWriter = new MazeWriter(_sharedState);
        _navigator = new Navigator(_sharedState);

        _sharedState.MazeChars = _mazeWriter.CreateEmptyMaze(width, height);

        var position = new Coordinate(width / 2, height / 2);

        if (_debug)
            DebugPrint(_sharedState.MazeChars, position);

        GenerateMaze(position, _sharedState.MazeChars);

        _mazeWriter.CreateExit();
        _mazeWriter.AddPlayer(position);

        return MazeToString(_sharedState.MazeChars);
    }

    private void GenerateMaze(Coordinate startPosition, char[][] mazeChars)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var moveForwardCount = 0;
        var vector = new Vector(startPosition, _navigator.GetRandomDirection());

        _mazeWriter.CreateWallsAtSides(vector);



        while (_sharedState.Unvisited.Any())
        {
            _remainingUnexploredOnPath = _navigator.GetUnexploredConnectedToPath(mazeChars);

            while (_remainingUnexploredOnPath.Any())
            {
                var surround = _mazeReader.GetSurround(vector, mazeChars);

                if (surround.CanMoveForward)
                {
                    if (moveForwardCount % 2 == 0)
                    {
                        var direction = GetDirection(vector, mazeChars, moveForwardCount, surround);
                        if (vector.Direction != direction)
                        {
                            vector.Direction = direction;
                            moveForwardCount = 0;
                        }
                    }

                    vector = _navigator.MoveInDirection(vector);
                    _mazeWriter.CreateWallsAtSides(vector);

                    moveForwardCount++;
                }
                else
                {
                    if (surround.ForwardView.IsUnvisitedAhead)
                    {
                        _mazeWriter.CreateWallAhead(vector);
                    }

                    if (surround.ForwardView.IsDoubleWallBlockAhead)
                    {
                        _mazeWriter.CreateCorridorAhead(vector);
                    }
                }

                if (surround.CanMove)
                {
                    if (!surround.CanMoveForward || GetRandomChance(4) && moveForwardCount % 2 == 0)
                    {
                        vector.Direction = _mazeReader.GetNewDirection(surround);

                        moveForwardCount = 0;
                    }
                }
                else
                {
                    vector = _navigator.Relocate(_remainingUnexploredOnPath);
                    moveForwardCount = 0;
                }



                _remainingUnexploredOnPath = _navigator.GetUnexploredConnectedToPath(mazeChars);

                if (_debug)
                {
                    DebugPrint(mazeChars, vector.Position);
                }
            }


            ManageEdgeCases(mazeChars);

            if (_debug)
            {
                DebugPrint(mazeChars, vector.Position);
            }


            if (stopwatch.ElapsedMilliseconds > 2500)
                return;

        }

    }



    private Direction GetDirection(Vector vector, char[][] mazeChars, int moveForwardCount, Surround surround)
    {
        
        if (surround.LeftView.IsDoubleWallBlockAhead)
        {
            vector.Direction = surround.LeftView.Direction;
        }

        if (surround.RightView.IsDoubleWallBlockAhead)
        {
            vector.Direction = surround.RightView.Direction;
        }

        if (surround.LeftView.IsDoubleWallBlockAhead && surround.RightView.IsDoubleWallBlockAhead)
        {
            var random = RandomNumberGenerator.GetInt32(2);

            vector.Direction = random == 0 ? surround.LeftView.Direction : surround.RightView.Direction;
        }
        
        return vector.Direction;
    }

    private bool ProcessEdgeCase(char[][] mazeChars, Func<Surround, bool> edgeCase)
    {
        var cantProcess = new List<Coordinate>();

        var processedAnEdgeCase = false;

        while (cantProcess.Count != _sharedState.Unvisited.Count)
        {
            var random = RandomNumberGenerator.GetInt32(_sharedState.Unvisited.Count);

            var position = _sharedState.Unvisited[random];
            var direction = _navigator.GetRandomDirection();

            var surround = _mazeReader.GetFullSurround(new Vector(position, direction), mazeChars);

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


    private bool ManageEdgeCases(char[][] mazeChars)
    {

        var processed = ProcessEdgeCase(mazeChars, BreakEnclosure);

        if (processed)
        {
            return true;
        }


        processed = ProcessEdgeCase(mazeChars, FinishWallCross);

        if (processed)
        {
            return true;
        }


        processed = ProcessEdgeCase(mazeChars, BreakPartEnclosure);

        if (processed)
        {
            return true;
        }

        return processed;

    }

    private bool FinishWallCross(Surround surround)
    {
        int count = 0;
        foreach (var surroundView in surround.Views)
        {
            if (IsWallCross(surroundView))
            {
                count++;
            }
        }

        if (count >= 2)
        {
            _mazeWriter.UpdateChar(surround.Position.X, surround.Position.Y, MazeChars.Wall);
            return true;
        }

        return false;
    }

    private bool IsWallCross(View view)
    {
        return view.LookAhead.Ahead == MazeChars.Wall
               && (view.LookAhead.AheadLeft == MazeChars.Corridor || view.LookAhead.AheadLeft == Consts.BorderChar)
               && (view.LookAhead.AheadRight == MazeChars.Corridor || view.LookAhead.AheadRight == Consts.BorderChar);

    }

    private bool BreakEnclosure(Surround surround)
    {
        foreach (var surroundView in surround.Views)
        {
            if (IsEnclosure(surroundView))
            {
                _mazeWriter.CreateCorridorAhead(new Vector(surround.Position, surroundView.Direction));
                return true;
            }
        }
        return false;
    }

    private bool BreakPartEnclosure(Surround surround)
    {

        foreach (var surroundView in surround.Views)
        {
            if (IsPartEnclosure(surroundView))
            {
                _mazeWriter.CreateCorridorAhead(new Vector(surround.Position, surroundView.Direction));
                return true;
            }
        }
        return false;
    }


    private bool IsWallOrBorder(char c)
    {
        return c == MazeChars.Wall || c == Consts.BorderChar || c == Consts.UnvisitedChar;
    }

    private bool IsEnclosure(View view)
    {
        return view.LookAhead.Ahead == MazeChars.Wall
               && IsWallOrBorder(view.LookAhead.AheadLeft)
               && IsWallOrBorder(view.LookAhead.AheadRight);

    }

    private bool IsPartEnclosure(View view)
    {
        return view.LookAhead.Ahead == MazeChars.Wall && view.LookAhead.Ahead2 == MazeChars.Wall &&
               (view.LookAhead.AheadLeft == MazeChars.Corridor && IsWallOrBorder(view.LookAhead.AheadRight)
                || IsWallOrBorder(view.LookAhead.AheadLeft) && view.LookAhead.AheadRight == MazeChars.Corridor);
    }


    private bool GetRandomChance(int i)
    {
        var random = RandomNumberGenerator.GetInt32(i);

        return random == 0;
    }

    private string MazeToString(char[][] sharedStateMazeChars)
    {
        var concat = string.Join("\n", _sharedState.MazeChars.Select(x => string.Concat(x)));
        var final = concat.Replace(Consts.BorderChar.ToString(), MazeChars.Wall.ToString());

        return final;
    }

    private void DebugPrint(char[][] mazeChars, Coordinate position)
    {
        var p = mazeChars[position.Y][position.X];

        mazeChars[position.Y][position.X] = '#';
        var concat = string.Join("\n", mazeChars.Select(x => string.Concat(x)));

        _debugSteps.Add(concat);

        mazeChars[position.Y][position.X] = p;
    }


}