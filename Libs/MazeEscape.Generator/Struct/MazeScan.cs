using MazeEscape.Generator.Enums;

namespace MazeEscape.Generator.Struct;

struct MazeScan
{
    public Coordinate Position { get; set; }
    public Direction Direction { get; set; }

    public MazeView ForwardView { get; set; }
    public MazeView LeftView { get; set; }
    public MazeView RightView { get; set; }
    public MazeView Back { get; set; }

    public List<MazeView> Views => new List<MazeView>(){ ForwardView, RightView, Back, LeftView};

    public bool CanMoveForward { get; set; }
    public bool CanMoveLeft { get; set; }
    public bool CanMoveRight { get; set; }

    public bool CanMove => CanMoveForward | CanMoveLeft | CanMoveRight;
    
}