using MazeEscape.Engine;
using MazeEscape.GeneratorDemo.Tree;
using MazeEscape.Model.Domain;

namespace MazeEscape.GeneratorDemo.Helper
{
    internal class TreeHelper
    {
        private readonly PathTreeBuilder _pathTreeBuilder = new PathTreeBuilder();
        private readonly MazeConverter _mazeConverter = new MazeConverter();

        private string _mazeText;
        private List<List<MazeSquare>> _paths;

        private void BuildTree(string mazeString)
        {
            var maze = _mazeConverter.Parse(mazeString);
            var tree = _pathTreeBuilder.BuildTree(maze);

            _paths = tree.GetPaths(tree).ToList();
            _mazeText = _mazeConverter.ToText(maze);
        }

        internal List<string> GetBreadthFirstSearchPlot(string mazeString)
        {
            BuildTree(mazeString);

            var longestPathSize = _paths.Max(c => c.Count);

            var currentPath = new List<MazeSquare>();
            var plot = new List<string>();

            for (var i = 0; i < longestPathSize; i++)
            {
                var allUnique = _paths.Where(c => i < c.Count)
                                      .Select(x => x.ElementAt(i))
                                      .Distinct()
                                      .ToList();

                currentPath.AddRange(allUnique);
                var pathFormatted = _pathTreeBuilder.GetPathString(_mazeText, currentPath);
                plot.Add(pathFormatted);
            }

            return plot;
        }

        internal List<List<string>> GetDepthFirstSearchPlots(string mazeString)
        {
            BuildTree(mazeString);

            var currentPath = new List<MazeSquare>();
            var plots = new List<List<string>>();

            foreach (var path in _paths)
            {
                var plot = new List<string>();

                for (var i = 0; i < path.Count; i++)
                {
                    var squareToAdd = path[i];

                    var add = false;

                    if (currentPath.Count < i + 1)
                    {
                        add = true;
                    }
                    else if (currentPath[i] != squareToAdd)
                    {
                        currentPath.RemoveRange(i, currentPath.Count - i);

                        var plotFrame = _pathTreeBuilder.GetPathString(_mazeText, currentPath);
                        plot.Add(plotFrame);

                        add = true;
                    }
                    else
                    {
                        // square is already added in the right place
                    }

                    if (add)
                    {
                        currentPath.Add(squareToAdd);
                        var plotFrame = _pathTreeBuilder.GetPathString(_mazeText, currentPath);
                        plot.Add(plotFrame);
                    }
                }

                plots.Add(plot);
            }

            return plots;
        }

        internal List<List<string>> GetExitPathsPlots(string mazeString)
        {
            var plots = new List<List<string>>();

            BuildTree(mazeString);

            _paths = _paths.Where(p => p[^1].IsExit).ToList();

            var currentPath = new List<MazeSquare>();

            foreach (var path in _paths)
            {
                var plot = new List<string>();

                for (var i = 0; i < path.Count; i++)
                {
                    var squareToAdd = path[i];
                    currentPath.Add(squareToAdd);

                    var plotFrame = _pathTreeBuilder.GetPathString(_mazeText, currentPath);
                    plot.Add(plotFrame);
                }

                plots.Add(plot);
            }

            return plots;
        }

    }
}
