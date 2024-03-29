using MazeEscape.Engine;
using MazeEscape.Model.Domain;
using MazeEscape.Tests.Helper;
using MazeEscape.Generator.Main;
using MazeEscape.GeneratorDemo.Helper;

namespace MazeEscape.GeneratorDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        private void Run()
        {
            var width = 30;
            var height = 30;

            var continuousMode = false;
            var iterations = 500;

            var backgroundColour = ConsoleColor.Black;
            var borderColour = ConsoleColor.DarkGray;
            var mazeColour = ConsoleColor.White;

            var escapeRouteColour = ConsoleColor.DarkGreen;
            var pathDfsColour = ConsoleColor.DarkGreen;
            var pathBfsColour = ConsoleColor.Blue;

            var plotMazeBuild = true;
            var plotEscapeRoute = true;

            var plotBfsRoute = false;
            var plotDfsRoute = false;


            Console.WriteLine("press any key to start...");
            Console.ReadKey();

            var generator = new MazeGenerator();

            for (var i = 0; i < iterations; i++)
            {
                var steps = generator.GenerateRandomWithDebugSteps(width, height);
                var formatted = FormatMazeSteps(steps);
                var lastFrame = steps.Last();

                ConsoleHelper.WriteFirstFrame(backgroundColour, borderColour, formatted.First());

                if (plotMazeBuild)
                {
                    ConsoleHelper.WriteDiffsToConsole(formatted, 1, mazeColour);
                }
                else
                {
                    ConsoleHelper.WriteDiffsToConsole(new List<string>() { formatted.First(), lastFrame }, 1, mazeColour);
                }

                Thread.Sleep(500);

                ConsoleHelper.PromptIfNotContinousMode(continuousMode);

                if (plotBfsRoute)
                {
                    PlotPathsBFS(lastFrame, pathBfsColour);
                }

                if (plotDfsRoute)
                {
                    PlotPathsDFS(lastFrame, pathDfsColour);
                }

                if (plotEscapeRoute)
                {
                    PlotExitRoute(lastFrame, escapeRouteColour);
                }


                Thread.Sleep(1000);

                ConsoleHelper.PromptIfNotContinousMode(continuousMode);
            }
        }

        private static void PlotExitRoute(string lastFrame, ConsoleColor pathColour)
        {
            var pathTreeBuilder = new PathTreeBuilder();
            var mazeConverter = new MazeConverter();

            var maze = mazeConverter.Parse(lastFrame);

            var tree = pathTreeBuilder.BuildTree(maze);
            var paths = tree.GetPaths(tree).ToList();

            var mazeText = mazeConverter.ToText(maze);

            paths = paths.Where(p => p[^1].IsExit).ToList();


            var pathPlots = new List<List<string>>();

            var squares = new List<MazeSquare>();

            foreach (var path in paths)
            {
                var pathPlot = new List<string>();


                for (var i = 0; i < path.Count; i++)
                {
                    squares.Add(path[i]);

                    var pathFormatted = pathTreeBuilder.GetPathString(mazeText, squares);
                    pathFormatted = pathFormatted.Replace("#", "█");

                    pathPlot.Add(pathFormatted);
                }

                pathPlots.Add(pathPlot);
            }

            foreach (var pathPlot in pathPlots)
            {
                //ConsoleHelper.WriteDiffsToConsole(new List<string>() { lastFrame, pathPlots[0] }, 20, ConsoleColor.DarkGreen);
                ConsoleHelper.WriteDiffsToConsole(pathPlot, 5, pathColour);
            }

      


        }
        private static void PlotPathsBFS(string lastFrame, ConsoleColor pathColour)
        {
            var pathPlots = TreeHelper.GetPathsBFS(lastFrame);

            ConsoleHelper.WriteDiffsToConsole(new List<string>() { lastFrame, pathPlots[0] }, 20, ConsoleColor.DarkGreen);

            ConsoleHelper.WriteDiffsToConsole(pathPlots, 5, pathColour);
        }

        private static void PlotPathsDFS(string lastFrame,ConsoleColor pathColour)
        {
            var pathPlots = TreeHelper.GetPathsDFS(lastFrame);

            int count = 0;

            string last = lastFrame;

            foreach (var pathPlot in pathPlots)
            {
                if (count > 0)
                    ConsoleHelper.WriteDiffsToConsole(new List<string>() { lastFrame, pathPlot[0] }, 1, ConsoleColor.DarkRed);
                else
                {
                    ConsoleHelper.WriteDiffsToConsole(new List<string>() { last, pathPlot[0] }, 1, ConsoleColor.DarkGreen);
                }

                //pathPlot.Insert(0, lastFrame);
                //pathPlot.Insert(0, pathPlot[0]);

                last = pathPlot.Last();


                //pathPlot.Add(lastFrame);
                //pathPlot.Add(pathPlot[0]);

                ConsoleHelper.WriteDiffsToConsole(pathPlot, 1, pathColour);

                count++;

            }
        }

        private static List<string> FormatMazeSteps(List<string> steps)
        {
            for (var j = 0; j < steps.Count; j++)
            {
                steps[j] = steps[j].Replace("X", "█")
                                   .Replace("+", "█")
                                   .Replace("=", "-");
            }

            return steps;
        }

    }
}
