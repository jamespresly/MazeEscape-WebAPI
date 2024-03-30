using MazeEscape.Model.Enums;

namespace MazeEscape.Model.Domain
{
    public class MazeSquare
    {
        public Location Location { get; set; }
        public SquareType SquareType { get; set; }
        public bool IsExit { get; set; }
    }
}