using MazeEscape.Engine.Enums;
using MazeEscape.Engine.Extensions;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Engine.Model;

namespace MazeEscape.Engine
{
    public class PlayerNavigator : IPlayerNavigator
    {

        private readonly Dictionary<Orientation, Tuple<int, int>> _orientationOffsetMap =
            new()
            {
                { Orientation.North, new(0, -1) },
                { Orientation.East, new(1, 0) },
                { Orientation.South, new(0, 1) },
                { Orientation.West, new(-1, 0) },
            };



        public string Move(PlayerMove move, Maze maze)
        {
            var player = maze.Player;

            if (move == PlayerMove.Forward)
            {
                var vision = GetVision(maze);

                if (vision.Ahead == SquareType.Wall)
                {
                    return "You walked into a wall";

                }

                player.Location = GetNextLocation(player.Location, player.FacingDirection);

                if (player.Location.IsSame(maze.ExitLocation))
                {
                    return "You escaped";

                }
            }

            if (move == PlayerMove.Right)
            {
                player.FacingDirection = player.FacingDirection.TurnClockwise();
            }

            if (move == PlayerMove.Left)
            {
                player.FacingDirection = player.FacingDirection.TurnAnticlockwise();
            }

            return "";
        }

        public PlayerVision GetVision(Maze maze)
        {
            var vision = new PlayerVision();

            var player = maze.Player;


            var aheadLocation = GetNextLocation(player.Location, player.FacingDirection);
            var leftLocation = GetNextLocation(player.Location, player.FacingDirection.TurnAnticlockwise());
            var rightLocation = GetNextLocation(player.Location, player.FacingDirection.TurnClockwise());

            vision.Ahead = GetSquareTypeFromLocation(aheadLocation, maze);
            vision.Left = GetSquareTypeFromLocation(leftLocation, maze);
            vision.Right = GetSquareTypeFromLocation(rightLocation, maze);

            vision.FacingDirection = maze.Player.FacingDirection;

            return vision;
        }
        private Location GetNextLocation(Location location, Orientation inDirection)
        {

            var offset = _orientationOffsetMap[inDirection];

            var xOffset = offset.Item1;
            var yOffset = offset.Item2;

            var nextLocation = new Location()
            {
                XCoordinate = location.XCoordinate + xOffset,
                YCoordinate = location.YCoordinate + yOffset
            };

            return nextLocation;
        }

        private SquareType GetSquareTypeFromLocation(Location location, Maze maze)
        {
            var square = maze.Squares.FirstOrDefault(x => x.Location.IsSame(location));

            if (square == null)
                throw new Exception("Square not found");

            return square.SquareType;
        }

     
    }
}
