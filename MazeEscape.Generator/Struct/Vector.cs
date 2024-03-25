using MazeEscape.Generator.Enums;

namespace MazeEscape.Generator.Struct;

internal struct Vector()
{
    public Vector(int x, int y, Direction direction) : this()
    {
        X = x;
        Y = y;
        Direction = direction;
    }
    public int X { get; set; }
    public int Y { get; set; }

    public Direction Direction { get; set; }
}