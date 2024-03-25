using MazeEscape.Engine.Interfaces;
using System.Text;
using MazeEscape.Model.Domain;
using MazeEscape.Model.Enums;
using MazeEscape.Model.Extensions;

namespace MazeEscape.Engine;

public class MazeConverter : IMazeConverter
{

    private readonly Dictionary<Orientation, char> _arrowMap = new()
    {
        { Orientation.North, '▲' },
        { Orientation.East, '►' },
        { Orientation.South, '▼' },
        { Orientation.West, '◄' },
    };

    private readonly Dictionary<SquareType, char> _squareMap = new()
    {
        { SquareType.Wall, '+'},
        { SquareType.Corridor, ' '},
        { SquareType.Exit, 'E'}
    };

    private readonly Dictionary<char, SquareType> _charMap = new()
    {
        { '+', SquareType.Wall},
        { ' ',SquareType.Corridor},
        { 'E', SquareType.Exit },
        { 'S', SquareType.Corridor},
        { '▲', SquareType.Corridor},
        { '►', SquareType.Corridor},
        { '▼', SquareType.Corridor},
        { '◄', SquareType.Corridor},
        
    };

    private readonly List<char> _playerArrows = new()
    {
        '▲', '►', '▼', '◄'
    };


    public Maze GenerateFromText(string text)
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

                if (col == 'E')
                {
                    square.IsExit = true;
                    maze.ExitLocation = location;
                }
                else if (col == 'S')
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