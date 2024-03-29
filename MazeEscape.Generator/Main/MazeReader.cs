using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;
using MazeEscape.Model.Struct;
using System.Security.Cryptography;
using MazeEscape.Generator.Reference;

namespace MazeEscape.Generator.Main
{
    internal class MazeReader
    {
        private readonly Dictionary<Direction, Direction> _backMap = new()
        {
            { Direction.Up, Direction.Down },
            { Direction.Left, Direction.Right },
            { Direction.Right, Direction.Left },
            { Direction.Down, Direction.Up },
        };


        internal Surround GetFullSurround(Vector vector, char[][] mazeChars)
        {
            var surround = GetSurround(vector, mazeChars);

            var back = new Vector(vector.Position, _backMap[vector.Direction]);

            surround.Back = GetView(back, mazeChars);

            return surround;
        }

        internal Surround GetSurround(Vector vector, char[][] mazeChars)
        {
            var sides = Maps.SidesMap[vector.Direction];

            var ahead = GetView(vector, mazeChars);

            var leftVector = new Vector(vector.Position, sides.Left);
            var rightVector = new Vector(vector.Position, sides.Right);

            var left = GetView(leftVector, mazeChars);
            var right = GetView(rightVector, mazeChars);


            var surround = new Surround
            {
                Position = vector.Position,
                Direction = ahead.Direction,

                ForwardView = ahead,
                LeftView = left,
                RightView = right,

                CanMoveForward = ahead.CanMoveInDirection,
                CanMoveLeft = left.CanMoveInDirection,
                CanMoveRight = right.CanMoveInDirection
            };

            return surround;
        }

        internal Direction GetNewDirection(Surround surround)
        {
            if (!surround.CanMove)
                throw new ArgumentException("no vector.Directions available");

            if (surround.CanMoveLeft && surround.CanMoveRight)
            {
                var random = RandomNumberGenerator.GetInt32(2);

                var direction = random == 0 ? surround.LeftView.Direction : surround.RightView.Direction;

                return direction;
            }

            if (!surround.CanMoveLeft)
                return surround.RightView.Direction;

            if (!surround.CanMoveRight)
                return surround.LeftView.Direction;

            throw new Exception("bad direction");


        }

        private View GetView(Vector vector, char[][] mazeChars)
        {
            var lookahead = GetLookAhead(vector, mazeChars);
            var canMoveAhead = CanMoveAhead(lookahead);
            var doubleWall = IsDoubleWallBlockAhead(lookahead);
            var aheadUnvisited = lookahead.Ahead == Consts.UnvisitedChar;

            return new View()
            {
                Direction = vector.Direction,
                LookAhead = lookahead,
                CanMoveInDirection = canMoveAhead,
                IsDoubleWallBlockAhead = doubleWall,
                IsUnvisitedAhead = aheadUnvisited
            };

        }


        private bool CanMoveAhead(LookAhead lookAhead)
        {
            return lookAhead.Ahead != Consts.BorderChar
                   && lookAhead.Ahead != MazeChars.Corridor
                   && lookAhead.Ahead2 != MazeChars.Corridor
                   && lookAhead.AheadLeft != MazeChars.Corridor
                   && lookAhead.AheadRight != MazeChars.Corridor
                   && lookAhead.AheadLeft2 != MazeChars.Corridor
                   && lookAhead.AheadRight2 != MazeChars.Corridor;
        }

        private bool IsDoubleWallBlockAhead(LookAhead lookAhead)
        {
            return lookAhead.Ahead == MazeChars.Wall
                    && (lookAhead.Ahead2 == MazeChars.Wall || lookAhead.Ahead2 == Consts.BorderChar)
                    && lookAhead.AheadLeft != MazeChars.Corridor
                    && lookAhead.AheadLeft2 != MazeChars.Corridor
                    && lookAhead.AheadRight != MazeChars.Corridor
                    && lookAhead.AheadRight2 != MazeChars.Corridor
                ;
        }

        // todo make private
        internal LookAhead GetLookAhead(Vector vector, char[][] _mazeChars)
        {
            var aheadOffset = Maps.DirectionMap[vector.Direction];

            var offsetList = Maps.DirectionMap.ToList();
            var index = offsetList.IndexOf(new(vector.Direction, aheadOffset));

            var prevIndex = index == 0 ? offsetList.Count - 1 : index - 1;
            var nextIndex = (index + 1) % offsetList.Count;

            var leftOffset = offsetList[prevIndex].Value;
            var rightOffset = offsetList[nextIndex].Value;

            var leftAhead = _mazeChars[vector.Position.Y + leftOffset.Y + aheadOffset.Y][vector.Position.X + leftOffset.X + aheadOffset.X];
            var rightAhead = _mazeChars[vector.Position.Y + rightOffset.Y + aheadOffset.Y][vector.Position.X + rightOffset.X + aheadOffset.X];

            var ahead = _mazeChars[vector.Position.Y + aheadOffset.Y][vector.Position.X + aheadOffset.X];

            var ahead2 = GetAhead2(vector.Position, aheadOffset, _mazeChars);

            var leftAhead2 = '0';
            var rightAhead2 = '0';

            if (ahead2 != '0')
            {
                leftAhead2 = _mazeChars[vector.Position.Y + leftOffset.Y + aheadOffset.Y * 2][vector.Position.X + leftOffset.X + aheadOffset.X * 2];
                rightAhead2 = _mazeChars[vector.Position.Y + rightOffset.Y + aheadOffset.Y * 2][vector.Position.X + rightOffset.X + aheadOffset.X * 2];
            }

            return new LookAhead()
            {
                Ahead = ahead,
                Ahead2 = ahead2,
                AheadLeft = leftAhead,
                AheadRight = rightAhead,
                AheadLeft2 = leftAhead2,
                AheadRight2 = rightAhead2
            };
        }

        private char GetAhead2(Coordinate position, Offset aheadOffset, char[][] _mazeChars)
        {
            var ahead2 = '0';

            var x = position.X + aheadOffset.X * 2;
            var y = position.Y + aheadOffset.Y * 2;

            if (x >= 0 && y >= 0 && x <= _mazeChars[0].Length - 1 && y <= _mazeChars.Length - 1)
            {
                ahead2 = _mazeChars[y][x];
            }

            return ahead2;
        }


    }
}
