using MazeEscape.Generator.Main;
using MazeEscape.GeneratorDemo.Helper;

namespace MazeEscape.GeneratorDemo
{
    internal class Program
    {
        private readonly DemoHelper _dh;

        public Program()
        {
            var generator = new MazeGenerator();
            var treeHelper = new TreeHelper();
            var formattingHelper = new FormattingHelper();
            var consoleHelper = new ConsoleHelper();

            _dh = new DemoHelper(generator, treeHelper, formattingHelper, consoleHelper);
        }

        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        private void Run()
        {
            var width = 50;
            var height = 50;

            var continuousMode = true;
            var iterations = 500;

            var backgroundColour = ConsoleColor.Black;
            var borderColour = ConsoleColor.DarkGray;
            var mazeColour = ConsoleColor.White;

            var escapeRouteColour = ConsoleColor.DarkGreen;
            var BfsColour = ConsoleColor.Blue;
            var DfsBranchesColour = ConsoleColor.DarkGreen;
            var DfsLeavesColour = ConsoleColor.DarkRed;

            var plotMazeBuild = true;
            var plotEscapeRoute = true;

            var plotBfsRoute = true;
            var plotDfsRoute = true;


            Console.WriteLine("press any key to start...");
            Console.ReadKey();

            _dh.InitialiseConsole(backgroundColour, borderColour);
            
            for (var i = 0; i < iterations; i++)
            {
                var formattedMazeSteps = _dh.GetMazeBuildSteps(width, height);
                
                var border = formattedMazeSteps.First();
                var completedMaze = formattedMazeSteps.Last();

                _dh.PlotBorderAndBackground(border);

                if (plotMazeBuild)
                {
                    _dh.PlotMazeBuild(formattedMazeSteps, mazeColour);
                }

                _dh.PromptIfNotContinuousMode(continuousMode, 500);

                if (plotBfsRoute)
                {
                    _dh.ResetConsole(border, completedMaze, mazeColour);

                    _dh.PlotPathsBreadthFirst(completedMaze, BfsColour);
                }

                if (plotDfsRoute)
                {
                    _dh.ResetConsole(border, completedMaze, mazeColour);

                    _dh.PlotPathsDepthFirst(completedMaze, DfsBranchesColour, DfsLeavesColour);
                }

                if (plotEscapeRoute)
                {
                    _dh.ResetConsole(border, completedMaze, mazeColour);

                    _dh.PlotExitRoute(completedMaze, escapeRouteColour);
                }

                _dh.PromptIfNotContinuousMode(continuousMode, 1000);
            }
        }

    }
}
