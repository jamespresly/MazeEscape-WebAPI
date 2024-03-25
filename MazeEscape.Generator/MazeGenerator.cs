using System.Diagnostics;
using System.Security.Cryptography;
using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Reference;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;

namespace MazeEscape.Generator;

public class MazeGenerator : IMazeGenerator
{
    private MazeReader _mazeReader;
    private MazeWriter _mazeWriter;
    private MazeExplorer _mazeExplorer;

    private SharedState _sharedState;

    private List<Vector> _remainingUnexploredOnPath;

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
        _mazeExplorer = new MazeExplorer(_sharedState);

        _sharedState.MazeChars = _mazeWriter.CreateEmptyMaze(width, height);

        var position = new Coordinate(width / 2, height / 2);

        GenerateMaze(position, _sharedState.MazeChars);

        _mazeWriter.CreateExit();
        _mazeWriter.AddPlayer(position);

        return MazeToString(_sharedState.MazeChars);
    }


    private void GenerateMaze(Coordinate position, char[][] mazeChars)
    {

        var direction = _mazeExplorer.GetRandomDirection();

        _mazeWriter.CreateWallsAtSides(position, direction);


        var relocated = false;


        while (_sharedState.Unvisited.Any())
        {
            _remainingUnexploredOnPath = _mazeExplorer.GetUnexploredConnectedToPath(mazeChars);

            while (_remainingUnexploredOnPath.Any())
            {

                // make path
                if (_mazeReader.CanMoveAhead(position, direction, mazeChars))
                {
                    relocated = false;

                    position = _mazeExplorer.MoveInDirection(position, direction);
                    _mazeWriter.CreateWallsAtSides(position, direction);

                    if (!_mazeReader.CanMoveAhead(position, direction, mazeChars) || GetRandomChance(4))
                    {
                        direction = _mazeReader.GetNewDirection(position, direction, mazeChars);
                    }

                }
                else if (_mazeReader.CanMoveLeftOrRight(position, direction, mazeChars))
                {
                    relocated = false;
                    direction = _mazeReader.GetNewDirection(position, direction, mazeChars);
                }
                else
                {

                    if (relocated)
                    {
                        _mazeWriter.CreateWallAhead(position, direction);
                    }


                    if (_remainingUnexploredOnPath.Any())
                    {
                        var newLocation = _mazeExplorer.Relocate(_remainingUnexploredOnPath);

                        direction = newLocation.Direction;
                        position.X = newLocation.X;
                        position.Y = newLocation.Y;

                        relocated = true;
                    }

                }

                _remainingUnexploredOnPath = _mazeExplorer.GetUnexploredConnectedToPath(mazeChars);

                //DebugPrint(mazeChars, position);
            }

            ProcessEdgeCases(position, mazeChars);

        }
    }

    private void ProcessEdgeCases(Coordinate position, char[][] mazeChars)
    {
  
        var cantProcess = new List<Vector>();

        // edge cases
        if (_sharedState.Unvisited.Count > 0)
        {
            while (cantProcess.Count != (_sharedState.Unvisited.Count * 4))
            {
                var random = RandomNumberGenerator.GetInt32(_sharedState.Unvisited.Count);

                var direction = _mazeExplorer.GetRandomDirection();
                position = _sharedState.Unvisited[random];

                if (cantProcess.Contains(new Vector(position.X, position.Y, direction)))
                    continue;

                var lookahead = _mazeReader.GetLookAhead(position, direction, mazeChars);

                while (lookahead.Ahead == Consts.UnvisitedChar)
                {
                    position = _mazeExplorer.MoveInDirection(position, direction);
                    lookahead = _mazeReader.GetLookAhead(position, direction, mazeChars);
                }

                if (BreakEnclosureEdgeCase(lookahead, position, direction)) 
                    break;

                if (FillCrossEdgeCase(lookahead, position)) 
                    break;

                if (BreakBoundaryCornerEdgeCase(lookahead, position, direction)) 
                    break;

                cantProcess.Add(new Vector(position.X, position.Y, direction));

                if (cantProcess.Count == (_sharedState.Unvisited.Count * 4))
                {
                    throw new Exception("cant process remaining:" + cantProcess.Count + "\n");// + DebugPrint());
                }
            }


            //DebugPrint(mazeChars, position);
        }
    }

    private bool BreakEnclosureEdgeCase(LookAhead lookahead, Coordinate position, Direction direction)
    {
        // Enclosure
        //
        //  +++++
        //  +===+
        //  +===+
        //  +++++
        //
        if (lookahead.Ahead == MazeChars.Wall && lookahead.Ahead2 == MazeChars.Corridor)
        {
            _mazeWriter.CreateCorridorAhead(position, direction);
            _mazeWriter.UpdateChar(position.X, position.Y, MazeChars.Corridor);
            return true;
        }

        return false;
    }

    private bool FillCrossEdgeCase(LookAhead lookahead, Coordinate position)
    {
        // Wall cross 
        //
        //     +
        //     +
        //  ++===++
        //     +
        //     +
        //
        if (lookahead.Ahead == MazeChars.Wall && lookahead.AheadLeft == MazeChars.Corridor &&
            lookahead.AheadRight == MazeChars.Corridor)
        {
            _mazeWriter.UpdateChar(position.X, position.Y, MazeChars.Wall);
            return true;
        }

        return false;
    }

    private bool BreakBoundaryCornerEdgeCase(LookAhead lookahead, Coordinate position, Direction direction)
    {
        // Stuck in the corner - or against the boundary
        //
        //  X+
        //  X+
        //  X=++
        //  XXXX
        //
        if (lookahead.Ahead == MazeChars.Wall &&
            (lookahead.AheadLeft == Consts.BorderChar || lookahead.AheadRight == Consts.BorderChar))
        {
            _mazeWriter.UpdateChar(position.X, position.Y, MazeChars.Corridor);
            _mazeWriter.CreateCorridorAhead(position, direction);
            return true;
        }

        return false;
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

    private string DebugPrint(char[][] mazeChars, Coordinate position)
    {
        var p = mazeChars[position.Y][position.X];

        mazeChars[position.Y][position.X] = 'P';
        var concat = string.Join("\n", mazeChars.Select(x => string.Concat(x)));

        Debug.WriteLine(concat);

        mazeChars[position.Y][position.X] = p;

        return concat;
    }


}