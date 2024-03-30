using MazeEscape.Model.Enums;

namespace MazeEscape.Model.Domain;

public class PlayerVision
{
    public SquareType Ahead { get; set; }
    public SquareType Left { get; set; }
    public SquareType Right { get; set; }

    public Orientation FacingDirection { get; set; }

}