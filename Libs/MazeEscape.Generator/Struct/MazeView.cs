using MazeEscape.Generator.Enums;

namespace MazeEscape.Generator.Struct;

struct MazeView
{
    public Direction Direction { get; set; }
    public LookAhead LookAhead { get; set; }
    public bool IsDoubleWallBlockAhead { get; set; }
    public bool IsUnvisitedAhead { get; set; }
    public bool CanMoveInDirection { get; set; }
}