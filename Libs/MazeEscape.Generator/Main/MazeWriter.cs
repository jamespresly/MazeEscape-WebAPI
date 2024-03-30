using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;
using System.Security.Cryptography;
using MazeEscape.Generator.Reference;

namespace MazeEscape.Generator.Main
{
    internal class MazeWriter
    {
        internal static readonly char[] DoNotOverwrite = new[]
        {
            Consts.BorderChar, MazeChars.PlayerStart, MazeChars.Corridor
        };

        private readonly SharedState _sharedState;

        public MazeWriter(SharedState sharedState)
        {
            _sharedState = sharedState;
        }

        public char[][] CreateEmptyMaze(int width, int height)
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
                        chars[y][x] = Consts.BorderChar;
                    }
                    else
                    {
                        chars[y][x] = Consts.UnvisitedChar;
                        _sharedState.Unvisited.Add(new Coordinate(x, y));
                    }
                }
            }

            return chars;
        }

        public void CreateExit()
        {
            var possibleExits = new List<Coordinate>();

            for (var y = 0; y < _sharedState.MazeChars.Length; y++)
            {
                if (_sharedState.MazeChars[y][1] == MazeChars.Corridor)
                {
                    possibleExits.Add(new Coordinate(0, y));
                }

                if (_sharedState.MazeChars[y][^2] == MazeChars.Corridor)
                {
                    possibleExits.Add(new Coordinate(_sharedState.MazeChars[0].Length - 1, y));
                }
            }

            for (var x = 0; x < _sharedState.MazeChars[0].Length; x++)
            {
                if (_sharedState.MazeChars[1][x] == MazeChars.Corridor)
                {
                    possibleExits.Add(new Coordinate(x, 0));
                }

                if (_sharedState.MazeChars[^2][x] == MazeChars.Corridor)
                {
                    possibleExits.Add(new Coordinate(x, _sharedState.MazeChars.Length - 1));
                }
            }

            var random = RandomNumberGenerator.GetInt32(possibleExits.Count);

            var exit = possibleExits[random];

            _sharedState.MazeChars[exit.Y][exit.X] = MazeChars.Exit;
        }

        public void AddPlayer(Coordinate position)
        {
            _sharedState.MazeChars[position.Y][position.X] = MazeChars.PlayerStart;
        }

        public void CreateWallsAtSides(Vector vector)
        {
            var sides = Maps.SidesMap[vector.Direction];

            CreateCharInDirection(vector.Position, sides.Left, MazeChars.Wall);
            CreateCharInDirection(vector.Position, sides.Right, MazeChars.Wall);

            UpdateChar(vector.Position.X, vector.Position.Y, MazeChars.Corridor);
        }

        public void CreateWallAhead(Vector vector)
        {
            CreateCharInDirection(vector.Position, vector.Direction, MazeChars.Wall);
        }

        public void CreateCorridorAhead(Vector vector)
        {
            CreateCharInDirection(vector.Position, vector.Direction, MazeChars.Corridor);
        }

        public void CreateCharInDirection(Coordinate position, Direction direction, char c)
        {
            var offset = Maps.DirectionMap[direction];

            UpdateChar(position.X + offset.X, position.Y + offset.Y, c);
        }

        public void UpdateChar(int x, int y, char c)
        {

            if (!DoNotOverwrite.Contains(_sharedState.MazeChars[y][x]))
            {

                if (_sharedState.MazeChars[y][x] == Consts.UnvisitedChar)
                {
                    _sharedState.Unvisited.Remove(new Coordinate(x, y));
                }

                if (c == MazeChars.Corridor)
                {
                    _sharedState.CorridorsToCheck.Add(new Coordinate(x, y));
                }

                _sharedState.MazeChars[y][x] = c;

            }
        }
    }
}
