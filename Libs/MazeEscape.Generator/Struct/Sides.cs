using MazeEscape.Generator.Enums;

namespace MazeEscape.Generator.Struct;

internal struct Sides
{
    public Sides(Direction left, Direction right)
    {
        Left = left;
        Right = right;
    }
    public Direction Left { get; set; }
    public Direction Right { get; set; }
}