using MazeEscape.Engine;
using MazeEscape.Model.Domain;
using MazeEscape.Tests.Helper;

namespace MazeEscape.GeneratorDemo.Helper
{
    internal class TreeHelper
    {
        internal static List<string> GetPathsBFS(string mazeString, bool toExit = false)
        {
            var pathTreeBuilder = new PathTreeBuilder();
            var mazeConverter = new MazeConverter();

            var maze = mazeConverter.Parse(mazeString);

            var tree = pathTreeBuilder.BuildTree(maze);
            var paths = tree.GetPaths(tree).ToList();

            var mazeText = mazeConverter.ToText(maze);

            if (toExit)
            {
                paths = paths.Where(p => p[^1].IsExit).ToList();
            }


            var squares = new List<MazeSquare>();
            var max = paths.Max(c => c.Count);
            var full = new List<string>();

            for (var i = 0; i < max; i++)
            {

                var x = paths.Where(c => i < c.Count)
                                .Select(x => x.ElementAt(i))
                                .Distinct().ToList();

                squares.AddRange(x);

                var pathFormatted = pathTreeBuilder.GetPathString(mazeText, squares);
                pathFormatted = pathFormatted.Replace("#", "█");

                full.Add(pathFormatted);
            }


            return full;
        }

        internal static List<List<string>> GetPathsDFS(string mazeString, bool toExit = false)
        {
            var pathTreeBuilder = new PathTreeBuilder();
            var mazeConverter = new MazeConverter();

            var maze = mazeConverter.Parse(mazeString);

            var tree = pathTreeBuilder.BuildTree(maze);
            var paths = tree.GetPaths(tree);

            var mazeText = mazeConverter.ToText(maze);

            if (toExit)
            {
                paths = paths.Where(p => p[^1].IsExit).ToList();
            }

            var pathPlots = new List<List<string>>();


            var squares = new List<MazeSquare>();

            foreach (var path in paths)
            {
                var pathPlot = new List<string>();


                for (var i = 0; i < path.Count; i++)
                {
                    var mazeSquare = path[i];

                    if (squares.Count < i + 1)
                    {
                        squares.Add(mazeSquare);

                        var pathFormatted = pathTreeBuilder.GetPathString(mazeText, squares);
                        pathFormatted = pathFormatted.Replace("#", "█");

                        pathPlot.Add(pathFormatted);
                    }

                    else if (squares[i] != mazeSquare)
                    {
                        if (i == 0)
                        {
                            squares = new List<MazeSquare>();
                            squares.Add(mazeSquare);

                            var pathFormatted = pathTreeBuilder.GetPathString(mazeText, squares);
                            pathFormatted = pathFormatted.Replace("#", "█");

                            pathPlot.Add(pathFormatted);
                        }
                        else
                        {
                            squares.RemoveRange(i, squares.Count - i);

                            var pathFormatted = pathTreeBuilder.GetPathString(mazeText, squares);
                            pathFormatted = pathFormatted.Replace("#", "█");

                            pathPlot.Add(pathFormatted);


                            squares.Add(mazeSquare);

                            pathFormatted = pathTreeBuilder.GetPathString(mazeText, squares);
                            pathFormatted = pathFormatted.Replace("#", "█");

                            pathPlot.Add(pathFormatted);
                        }


                    }




                }

                pathPlots.Add(pathPlot);
            }

            return pathPlots;
        }
    }
}
