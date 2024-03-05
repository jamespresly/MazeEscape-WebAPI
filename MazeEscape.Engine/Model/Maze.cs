using MazeEscape.Engine.Enums;
using MazeEscape.Engine.Extensions;
using System.Text;

namespace MazeEscape.Engine.Model
{
    public class Maze
    {
        public int Width { get; set; }
        public int Height { get; set; }


        public List<MazeSquare> Squares { get; set; }

        public Location ExitLocation { get; set; }

        public Player Player { get; set; }


        public override string ToString()
        {
            return base.ToString();
        }
    }
}