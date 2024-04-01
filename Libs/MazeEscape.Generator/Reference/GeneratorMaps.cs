using MazeEscape.Generator.Enums;
using MazeEscape.Generator.Struct;
using MazeEscape.Model.Struct;

namespace MazeEscape.Generator.Reference;

internal class GeneratorMaps
{
    internal static readonly Dictionary<Direction, Offset> DirectionMap = new()
    {
        { Direction.Up, new Offset(0, -1) },
        { Direction.Right, new Offset(1, 0) },
        { Direction.Down, new Offset(0, 1) },
        { Direction.Left, new Offset(-1, 0) },
    };

    internal static readonly Dictionary<Direction, Sides> SidesMap = new()
    {
        { Direction.Up, new Sides(Direction.Left, Direction.Right)},
        { Direction.Right, new Sides(Direction.Up,Direction.Down)},
        { Direction.Down, new Sides(Direction.Right,Direction.Left)},
        { Direction.Left, new Sides(Direction.Down,Direction.Up)},
    };
}