using MazeEscape.Generator.Main;
using MazeEscape.Generator.Strategies;
using MazeEscape.GeneratorDemo.Helper;

namespace MazeEscape.GeneratorDemo
{
    internal class Program
    {
        private readonly DemoHelper _dh;

        public Program()
        {
            var generatorStrategyBuilder = new GeneratorStrategyBuilder(true);
            var generator = new MazeGenerator(generatorStrategyBuilder);
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

            var continuousMode = false;
            var iterations = 500;

            var backgroundColour = ConsoleColor.Black;
            var borderColour = ConsoleColor.DarkGray;
            var mazeColour = ConsoleColor.White;

            var escapeRouteColour = ConsoleColor.DarkGreen;
            var BfsColour = ConsoleColor.Blue;
            var DfsBranchesColour = ConsoleColor.DarkGreen;
            var DfsLeavesColour = ConsoleColor.DarkRed;

            var plotMazeBuildSteps = true;
            var plotEscapeRoute = true;

            var plotBfsRoute = true;
            var plotDfsRoute = false;


            Console.WriteLine("press any key to start...");
            Console.ReadKey();


            for (var i = 0; i < iterations; i++)
            {
                var formattedMazeSteps = _dh.GetMazeBuildSteps(width, height);
                
                var border = formattedMazeSteps.First();
                var completedMaze = formattedMazeSteps.Last();

                _dh.InitialiseConsole(backgroundColour, borderColour);

                _dh.PlotBorderAndBackground(border);

                if (plotMazeBuildSteps)
                {
                    _dh.PlotMazeBuild(formattedMazeSteps, mazeColour);
                }
                else
                {
                    _dh.ResetConsole(border, completedMaze, mazeColour);
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

                if(plotBfsRoute || plotDfsRoute || plotEscapeRoute)
                    _dh.PromptIfNotContinuousMode(continuousMode, 1000);
            }
        }

    }
}
