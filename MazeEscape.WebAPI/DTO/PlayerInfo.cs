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
}
