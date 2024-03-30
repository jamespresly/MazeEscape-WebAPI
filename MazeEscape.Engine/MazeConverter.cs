using MazeEscape.Engine.Interfaces;
using System.Text;
using MazeEscape.Model.Constants;
using MazeEscape.Model.Domain;
using MazeEscape.Model.Enums;
using MazeEscape.Model.Extensions;

namespace MazeEscape.Engine;

public class MazeConverter : IMazeConverter
{

    private readonly Dictionary<Orientation, char> _arrowMap = new()
    {
        { Orientation.North,MazeChars.UpArrow },
        { Orientation.East, MazeChars.RightArrow },
        { Orientation.South,MazeChars.DownArrow },
        { Orientation.West, MazeChars.LeftArrow},
    };

    private readonly Dictionary<SquareType, char> _squareMap = new()
    {
        { SquareType.Wall, MazeChars.Wall},
        { SquareType.Corridor, MazeChars.Corridor},
        { SquareType.Exit, MazeChars.Exit}
    };

    private readonly Dictionary<char, SquareType> _charMap = new()
    {
        { MazeChars.Wall, SquareType.Wall},
        { MazeChars.Corridor, SquareType.Corridor},
        { MazeChars.Exit, SquareType.Exit },
        { MazeChars.PlayerStart, SquareType.Corridor},
        { MazeChars.UpArrow , SquareType.Corridor},
        { MazeChars.RightArrow, SquareType.Corridor},
        { MazeChars.DownArrow , SquareType.Corridor},
        { MazeChars.LeftArrow, SquareType.Corridor},
    };

    private readonly List<char> _playerArrows = new()
    {
        MazeChars.UpArrow, MazeChars.RightArrow, MazeChars.DownArrow, MazeChars.LeftArrow
    };


    public Maze Parse(string text)
    {

        var maze = new Maze
        {
            Squares = new List<MazeSquare>()
        };

        text = text.Replace("\r", "");

        var rows = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        maze.Width = rows[0].Length;
        maze.Height = rows.Length;

        var rowCount = 0;

        foreach (var row in rows)
        {
            var colCount = 0;

            foreach (var col in row)
            {
                var location = new Location()
                {
                    XCoordinate = colCount,
                    YCoordinate = rowCount
                };

                var square = new MazeSquare()
                {
                    Location = location,
                    SquareType = _charMap[col]
                };

                if (col == MazeChars.Exit)
                {
                    square.IsExit = true;
                    maze.ExitLocation = location;
                }
                else if (col == MazeChars.PlayerStart)
                {
                    maze.Player = new Player()
                    {
                        // todo make this configurable
                        FacingDirection = Orientation.North,
                        Location = location
                    };
                }
                else if (_playerArrows.Contains(col))
                {
                    var revCharMap = _arrowMap.ToDictionary(x => x.Value, x => x.Key);
                    var direction = revCharMap[col];

                    maze.Player = new Player()
                    {
                        FacingDirection = direction,
                        Location = location
                    };
                }
             
                maze.Squares.Add(square);

                colCount++;
            }

            rowCount++;
        }

        return maze;
    }


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
                    squareText = _arrowMap[player.FacingDirection].ToString();
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