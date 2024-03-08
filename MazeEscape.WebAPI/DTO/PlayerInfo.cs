namespace MazeEscape.WebAPI.DTO
{
    public class PlayerInfo
    {
        public string MazeToken { get; set; }

        public Position Position { get; set; }

        public string Facing { get; set; }

        public Vision Vision { get; set; }

        public string Info { get; set; }

        public bool IsEscaped { get; set; }

    }

    public class Vision
    {
        public string Ahead { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
