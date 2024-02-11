using MazeEscape.Engine.Enums;

namespace MazeEscape.Engine.Model;

public class PlayerVision
{
    public SquareType Ahead { get; set; }
    public SquareType Left { get; set; }
    public SquareType Right { get; set; }

    public Orientation FacingDirection { get; set; }

}