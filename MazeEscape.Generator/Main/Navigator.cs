using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Struct;
using System.Security.Cryptography;
using MazeEscape.Generator.Reference;

namespace MazeEscape.Generator.Main
{
    internal class Navigator
    {

        private readonly SharedState _sharedState;

        public Navigator(SharedState sharedState)
        {
            _sharedState = sharedState;
        }

        internal Vector Relocate(List<Vector> remainingUnexploredOnPath)
        {
            var index = RandomNumberGenerator.GetInt32(remainingUnexploredOnPath.Count);

            var unvisited = remainingUnexploredOnPath[index];
            var direction = unvisited.Direction;

            return new Vector(unvisited.X, unvisited.Y, direction);
        }


        internal Direction GetRandomDirection()
        {
            return (Direction)RandomNumberGenerator.GetInt32(4);
        }


        internal Vector MoveInDirection(Vector vector)
        {
            var offsets = Maps.DirectionMap[vector.Direction];

            var position = vector.Position;

            position.X += offsets.X;
            position.Y += offsets.Y;

            vector.Position = position;

            if (position.X == 0 || position.Y == 0
                || _sharedState.MazeChars.Length == position.Y || _sharedState.MazeChars[0].Length == position.X)
            {
                throw new Exception("out of bounds");
            }

            return vector;
        }


        internal List<Vector> GetUnexploredConnectedToPath(char[][] mazeChars)
        {
            var directions = Maps.DirectionMap.ToList();

            var next = new List<Vector>();

            var corridorsWithAdjacentUnvisited = new List<Coordinate>();

            foreach (var corridor in _sharedState.CorridorsToCheck)
            {
                var hasAdjacentUnvisited = false;
                foreach (var direction in directions)
                {
                    var adjacent = mazeChars[corridor.Y + direction.Value.Y][corridor.X + direction.Value.X];
                    if (adjacent == Consts.UnvisitedChar)
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
}
