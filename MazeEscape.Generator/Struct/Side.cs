using MazeEscape.Generator.Enums;

namespace MazeEscape.Generator.Struct;

internal struct Side
{
    public Side(Direction left, Direction right)
    {
        Left = left;
        Right = right;
    }
    public Direction Left { get; set; }
    public Direction Right { get; set; }
}