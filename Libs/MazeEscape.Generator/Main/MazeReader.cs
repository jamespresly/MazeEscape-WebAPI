using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;
using MazeEscape.Model.Struct;
using MazeEscape.Generator.Helper;
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


        internal MazeScan GetFullScan(Vector vector, char[][] mazeChars)
        {
            var surround = GetScan(vector, mazeChars);

            var back = new Vector(vector.Position, _backMap[vector.Direction]);

            surround.Back = GetView(back, mazeChars);

            return surround;
        }

        internal MazeScan GetScan(Vector vector, char[][] mazeChars)
        {
            var sides = GeneratorMaps.SidesMap[vector.Direction];

            var ahead = GetView(vector, mazeChars);

            var leftVector = new Vector(vector.Position, sides.Left);
            var rightVector = new Vector(vector.Position, sides.Right);

            var left = GetView(leftVector, mazeChars);
            var right = GetView(rightVector, mazeChars);


            var surround = new MazeScan
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

        internal Direction GetNewDirection(MazeScan mazeScan)
        {
            if (!mazeScan.CanMove)
                throw new ArgumentException("no directions available");

            if (mazeScan.CanMoveLeft && mazeScan.CanMoveRight)
            {
                var random = RandomHelper.GetRandomIntLessThan(2);

                var direction = random == 0 ? mazeScan.LeftView.Direction : mazeScan.RightView.Direction;

                return direction;
            }

            if (!mazeScan.CanMoveLeft)
                return mazeScan.RightView.Direction;

            if (!mazeScan.CanMoveRight)
                return mazeScan.LeftView.Direction;

            throw new Exception("no new direction available");

        }

        private MazeView GetView(Vector vector, char[][] mazeChars)
        {
            var lookahead = GetLookAhead(vector, mazeChars);
            var canMoveAhead = CanMoveAhead(lookahead);
            var doubleWall = IsDoubleWallBlockAhead(lookahead);
            var aheadUnvisited = lookahead.Ahead == GeneratorConsts.UnvisitedChar;

            return new MazeView()
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
            return lookAhead.Ahead != GeneratorConsts.BorderChar
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
                    && (lookAhead.Ahead2 == MazeChars.Wall || lookAhead.Ahead2 == GeneratorConsts.BorderChar)
                    && lookAhead.AheadLeft != MazeChars.Corridor
                    && lookAhead.AheadLeft2 != MazeChars.Corridor
                    && lookAhead.AheadRight != MazeChars.Corridor
                    && lookAhead.AheadRight2 != MazeChars.Corridor
                ;
        }

        private LookAhead GetLookAhead(Vector vector, char[][] _mazeChars)
        {
            var aheadOffset = GeneratorMaps.DirectionMap[vector.Direction];

            var offsetList = GeneratorMaps.DirectionMap.ToList();
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
