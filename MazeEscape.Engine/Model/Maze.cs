using System.Text;
using MazeEscape.Engine.Enums;
using MazeEscape.Engine.Extensions;

namespace MazeEscape.Engine.Model
{
    public class Maze
    {
        public int Width { get; set; }
        public int Height { get; set; }


        public List<MazeSquare> Squares { get; set; }

        public Location ExitLocation { get; set; }

        public Player Player { get; set; }


        private readonly Dictionary<Orientation, string> _arrowMap =
            new()
            {
                { Orientation.North, "\u25b2"},
                { Orientation.East, "\u25ba"},
                { Orientation.South, "\u25bc"},
                { Orientation.West, "\u25c4"},
            };


        public string ToBase64String()
        {

            var bytes = new byte[Width * Height];

            var i = 0;

            foreach (var square in Squares)
            {

                bytes[i++] = Convert.ToByte(square.SquareType == SquareType.Wall);

            }

            var base64String = Convert.ToBase64String(bytes);

            return base64String;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            var player = Player;

            for (var i = 0; i < Height; i++)
            {

                for (var j = 0; j < Width; j++)
                {

                    var index = i * Width + j;

                    var square = Squares[index];

                    var squareText = "";

                    if (player.Location.IsSame(square.Location))
                    {
                        squareText = _arrowMap[player.FacingDirection];
                    }
                    else
                    {
                        squareText = square.SquareType == SquareType.Wall ? "+" : " ";
                    }

                    sb.Append(squareText);
                }

                sb.AppendLine();

            }

            return sb.ToString();
        }
    }
}