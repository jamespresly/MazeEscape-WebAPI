using MazeEscape.Engine.Enums;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Engine.Model;

namespace MazeEscape.Engine;

public class MazeGenerator : IMazeGenerator
{

    public Maze GenerateFromText(string text)
    {

        var maze = new Maze();

        maze.Squares = new List<MazeSquare>();

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
                else if (c == 'E')
                {
                    square.SquareType = SquareType.Exit;
                    maze.ExitLocation = new Location()
                    {
                        XCoordinate = colCount,
                        YCoordinate = rowCount

                    };
                }
                else
                {
                    square.SquareType = SquareType.Corridor;
                }

                if (c == 'S')
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

                }

                maze.Squares.Add(square);

                colCount++;
            }

            rowCount++;
        }

        return maze;
    }

    public Maze GenerateRandom(int width, int height)
    {
        throw new NotImplementedException();
    }
}