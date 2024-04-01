using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;
using MazeEscape.Generator.Reference;

namespace MazeEscape.Generator.Main
{
    internal class MazeWriter
    {
        internal static readonly char[] DoNotOverwrite = new[]
        {
            GeneratorConsts.BorderChar, MazeChars.PlayerStart, MazeChars.Corridor
        };

        private readonly SharedState _sharedState;

        public MazeWriter(SharedState sharedState)
        {
            _sharedState = sharedState;
        }

        public void WriteEmpty(Vector vector)
        {
            if (_sharedState.Debug)
                DebugPrint(_sharedState.MazeChars, vector.Position);
        }

        public void CreateWallsAtSides(Vector vector)
        {
            var sides = GeneratorMaps.SidesMap[vector.Direction];

            CreateCharInDirection(vector.Position, sides.Left, MazeChars.Wall);
            CreateCharInDirection(vector.Position, sides.Right, MazeChars.Wall);

            UpdateChar(vector.Position.X, vector.Position.Y, MazeChars.Corridor);

            if(_sharedState.Debug)
                DebugPrint(_sharedState.MazeChars, vector.Position);
        }

        public void CreateWallAhead(Vector vector)
        {
            CreateCharInDirection(vector.Position, vector.Direction, MazeChars.Wall);

            if (_sharedState.Debug)
                DebugPrint(_sharedState.MazeChars, vector.Position);
        }

        public void CreateCorridorAhead(Vector vector)
        {
            CreateCharInDirection(vector.Position, vector.Direction, MazeChars.Corridor);

            if (_sharedState.Debug)
                DebugPrint(_sharedState.MazeChars, vector.Position);
        }

        public void CreateCharInDirection(Coordinate position, Direction direction, char c)
        {
            var offset = GeneratorMaps.DirectionMap[direction];

            UpdateChar(position.X + offset.X, position.Y + offset.Y, c);
        }

        public void UpdateChar(int x, int y, char c)
        {
            if (!DoNotOverwrite.Contains(_sharedState.MazeChars[y][x]))
            {
                var position = new Coordinate(x, y);

                if (_sharedState.MazeChars[y][x] == GeneratorConsts.UnvisitedChar)
                {
                    _sharedState.Unvisited.Remove(position);
                }

                if (c == MazeChars.Corridor)
                {
                    _sharedState.CorridorsToCheck.Add(position);
                }

                _sharedState.MazeChars[y][x] = c;
            }
        }

        private void DebugPrint(char[][] mazeChars, Coordinate position)
        {
            var p = mazeChars[position.Y][position.X];

            mazeChars[position.Y][position.X] = '#';
            var concat = string.Join("\n", mazeChars.Select(x => string.Concat(x)));

            _sharedState.DebugSteps.Add(concat);
          
            mazeChars[position.Y][position.X] = p;
        }

       
    }
}
