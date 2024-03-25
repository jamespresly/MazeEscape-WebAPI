namespace MazeEscape.Model.Domain
{
    public class Maze
    {
        public int Width { get; set; }
        public int Height { get; set; }


        public List<MazeSquare> Squares { get; set; }

        public Location ExitLocation { get; set; }

        public Player Player { get; set; }


    }
}