using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Struct;
using System.Security.Cryptography;
using MazeEscape.Generator.Reference;

namespace MazeEscape.Generator
{
    internal class MazeExplorer
    {
 
        private readonly SharedState _sharedState;

        public MazeExplorer(SharedState sharedState)
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


        internal Coordinate MoveInDirection(Coordinate position, Direction direction)
        {
            var offsets = Maps.DirectionMap[direction];

            position.X += offsets.X;
            position.Y += offsets.Y;

            return position;
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
