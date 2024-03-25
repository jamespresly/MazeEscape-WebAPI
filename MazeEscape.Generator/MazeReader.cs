using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Constants;
using MazeEscape.Model.Struct;
using System.Security.Cryptography;
using MazeEscape.Generator.Reference;

namespace MazeEscape.Generator
{
    internal class MazeReader
    {
        internal Direction GetNewDirection(Coordinate position, Direction direction, char[][] mazeChars)
        {

            var sides = Maps.SidesMap[direction];

            var canMoveLeft = CanMoveAhead(position, sides.Left, mazeChars);
            var canMoveRight = CanMoveAhead(position, sides.Right, mazeChars);

            if (canMoveLeft && canMoveRight)
            {
                var random = RandomNumberGenerator.GetInt32(2);

                direction = random == 0 ? sides.Left : sides.Right;
            }
            else if (canMoveLeft || canMoveRight)
            {
                if (canMoveLeft)
                    direction = sides.Left;

                if (canMoveRight)
                    direction = sides.Right;
            }

            return direction;
        }

        internal bool CanMoveLeftOrRight(Coordinate position, Direction direction, char[][] mazeChars)
        {
            var sides = Maps.SidesMap[direction];

            var canMoveLeft = CanMoveAhead(position, sides.Left, mazeChars);
            var canMoveRight = CanMoveAhead(position, sides.Right, mazeChars);

            return canMoveLeft || canMoveRight;
        }

        internal bool CanMoveAhead(Coordinate position, Direction direction, char[][] mazeChars)
        {
            var lookahead = GetLookAhead(position, direction, mazeChars);

            return lookahead.Ahead != Consts.BorderChar
                   && lookahead.Ahead != MazeChars.Corridor
                   && lookahead.Ahead2 != MazeChars.Corridor
                   && lookahead.AheadLeft != MazeChars.Corridor
                   && lookahead.AheadRight != MazeChars.Corridor;
        }
        internal LookAhead GetLookAhead(Coordinate position, Direction direction, char[][] _mazeChars)
        {
            var aheadOffset = Maps.DirectionMap[direction];

            var offsetList = Maps.DirectionMap.ToList();
            var index = offsetList.IndexOf(new(direction, aheadOffset));

            var prevIndex = index == 0 ? offsetList.Count - 1 : index - 1;
            var nextIndex = (index + 1) % offsetList.Count;

            var leftOffset = offsetList[prevIndex].Value;
            var rightOffset = offsetList[nextIndex].Value;

            var leftAhead = _mazeChars[position.Y + leftOffset.Y + aheadOffset.Y][position.X + leftOffset.X + aheadOffset.X];
            var rightAhead = _mazeChars[position.Y + rightOffset.Y + aheadOffset.Y][position.X + rightOffset.X + aheadOffset.X];


            var ahead = _mazeChars[position.Y + aheadOffset.Y][position.X + aheadOffset.X];

            var ahead2 = GetAhead2(position, aheadOffset, _mazeChars);

            return new LookAhead()
            {
                Ahead = ahead,
                Ahead2 = ahead2,
                AheadLeft = leftAhead,
                AheadRight = rightAhead,
            };
        }

        internal char GetAhead2(Coordinate position, Offset aheadOffset, char[][] _mazeChars)
        {
            var ahead2 = '0';

            var x = position.X + aheadOffset.X + aheadOffset.X;
            var y = position.Y + aheadOffset.Y + aheadOffset.Y;

            if (x > 0 && y > 0 && x < _mazeChars[0].Length - 1 && y < _mazeChars.Length - 1)
            {
                ahead2 = _mazeChars[y][x];
            }

            return ahead2;
        }
    }
}
