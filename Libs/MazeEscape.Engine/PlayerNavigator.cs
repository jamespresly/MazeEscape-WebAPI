﻿using MazeEscape.Engine.Interfaces;
using MazeEscape.Model.Domain;
using MazeEscape.Model.Enums;
using MazeEscape.Model.Extensions;
using MazeEscape.Model.Struct;

namespace MazeEscape.Engine
{
    public class PlayerNavigator : IPlayerNavigator
    {

        private readonly Dictionary<Orientation, Offset> _orientationOffsetMap = new()
        {
            { Orientation.North, new Offset(0, -1) },
            { Orientation.East, new Offset(1, 0) },
            { Orientation.South, new Offset(0, 1) },
            { Orientation.West, new Offset(-1, 0) },
        };

        public string Move(PlayerMove move, Maze maze)
        {
            var player = maze.Player;

            if(move == PlayerMove.Forward)
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

            var xOffset = offset.X;
            var yOffset = offset.Y;

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
