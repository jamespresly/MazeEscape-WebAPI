using MazeEscape.Generator.Enums;

namespace MazeEscape.Generator.Struct;

internal struct Vector()
{
    public Vector(int x, int y, Direction direction) : this()
    {
        Position = new Coordinate(x, y);
        Direction = direction;
    }

    public Vector(Coordinate position, Direction direction) : this()
    {
        Position = position;
        Direction = direction;
    }

    public int X => Position.X;
    public int Y => Position.Y;

    public Coordinate Position { get; set; }

    public Direction Direction { get; set; }
}