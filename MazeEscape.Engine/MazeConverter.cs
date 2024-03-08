using MazeEscape.Engine.Enums;
using MazeEscape.Engine.Extensions;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Engine.Model;
using System.Text;

namespace MazeEscape.Engine;

public class MazeConverter : IMazeConverter
{

    public Maze GenerateFromText(string text)
    {

        var maze = new Maze();

        maze.Squares = new List<MazeSquare>();

        text = text.Replace("\r", "");

        var rows = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        maze.Width = rows[0].Length;
        maze.Height = rows.Length;

        var rowCount = 0;

        foreach (var row in rows)
        {

            var colCount = 0;

            foreach (var c in row)
            {

                var square = new MazeSquare()
                {
                    Location = new Location()
                    {
                        XCoordinate = colCount,
                        YCoordinate = rowCount
                    },

                };

                if (c == '+')
                {
                    square.SquareType = SquareType.Wall;
                }
                else if(c == ' ')
                {
                    square.SquareType = SquareType.Corridor;
                }
                else if (c == 'E')
                {
                    square.SquareType = SquareType.Exit;
                    square.IsExit = true;
                    maze.ExitLocation = new Location()
                    {
                        XCoordinate = colCount,
                        YCoordinate = rowCount

                    };
                }
                else if (c == 'S')
                {
                    maze.Player = new Player()
                    {
                        FacingDirection = Orientation.North,
                        Location = new Location()
                        {
                            XCoordinate = colCount,
                            YCoordinate = rowCount
                        }
                    };
                    square.SquareType = SquareType.Corridor;
                }
                else if (c == '\u25b2' || c == '\u25ba' || c == '\u25bc' || c == '\u25c4')  
                {
                    square.SquareType = SquareType.Corridor;

                    var revCharMap = _arrowMap.ToDictionary(x => x.Value, x => x.Key);

                    var direction = revCharMap[c.ToString()];

                    maze.Player = new Player()
                    {

                        FacingDirection = direction,
                        Location = new Location()
                        {
                            XCoordinate = colCount,
                            YCoordinate = rowCount
                        }
                    };
                }
                else
                {
                    throw new InvalidDataException("Invalid character:" + c + " found in maze");
                }



                maze.Squares.Add(square);

                colCount++;
            }

            rowCount++;
        }

        return maze;
    }



    private readonly Dictionary<Orientation, string> _arrowMap =
        new()
        {
            { Orientation.North, "\u25b2" },
            { Orientation.East, "\u25ba" },
            { Orientation.South, "\u25bc" },
            { Orientation.West, "\u25c4" },
        };

    private readonly Dictionary<SquareType, char> _squareMap = new()
    {
        { SquareType.Wall, '+'},
        { SquareType.Corridor, ' '},
        { SquareType.Exit, 'E'}
    };

    public string ToText(Maze maze)
    {
        var sb = new StringBuilder();

        var player = maze.Player;

        for (var i = 0; i < maze.Height; i++)
        {

            for (var j = 0; j < maze.Width; j++)
            {

                var index = i * maze.Width + j;

                var square = maze.Squares[index];

                var squareText = "";

                if (player.Location.IsSame(square.Location))
                {
                    squareText = _arrowMap[player.FacingDirection];
                }
                else
                {
                    squareText = _squareMap[square.SquareType].ToString();
                }

                sb.Append(squareText);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}