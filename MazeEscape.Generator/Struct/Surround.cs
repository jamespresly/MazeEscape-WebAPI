using MazeEscape.Generator.Enums;

namespace MazeEscape.Generator.Struct;

struct Surround
{
    public Coordinate Position { get; set; }
    public Direction Direction { get; set; }

    public View ForwardView { get; set; }
    public View LeftView { get; set; }
    public View RightView { get; set; }
    public View Back { get; set; }

    public List<View> Views => new List<View>(){ ForwardView, RightView, Back, LeftView};

    public bool CanMoveForward { get; set; }
    public bool CanMoveLeft { get; set; }
    public bool CanMoveRight { get; set; }

    public bool CanMove => CanMoveForward | CanMoveLeft | CanMoveRight;
    
}