using MazeEscape.Generator.Main;

namespace MazeEscape.GeneratorDemo.Helper
{
    internal class DemoHelper
    {
        private readonly MazeGenerator _mazeGenerator;
        private readonly TreeHelper _treeHelper;
        private readonly FormattingHelper _formattingHelper;
        private readonly ConsoleHelper _consoleHelper;

        internal DemoHelper(MazeGenerator mazeGenerator, TreeHelper treeHelper, FormattingHelper formattingHelper, ConsoleHelper consoleHelper)
        {
            _mazeGenerator = mazeGenerator;
            _treeHelper = treeHelper;
            _formattingHelper = formattingHelper;
            _consoleHelper = consoleHelper;
        }

        public List<string> GetMazeBuildSteps(int width, int height)
        {
            var final = _mazeGenerator.GenerateRandom(width, height);
            var steps = _mazeGenerator.Steps;
            steps.Add(final);
            var formattedMazeSteps = _formattingHelper.FormatMazeStepsForConsole(steps);

            return formattedMazeSteps;
        }

        internal void PlotExitRoute(string lastFrame, ConsoleColor pathColour)
        {
            var formattedLastFrame = _formattingHelper.FormatMazeTextForParser(lastFrame);
            var pathPlots = _treeHelper.GetExitPathsPlots(formattedLastFrame);

            pathPlots = pathPlots.Select(_formattingHelper.FormatPathPlotsForConsole).ToList();

            foreach (var pathPlot in pathPlots)
            {
                _consoleHelper.WriteDiffsToConsole(new List<string>() { lastFrame, pathPlot[0] }, 20, pathColour);

                _consoleHelper.WriteDiffsToConsole(pathPlot, 5, pathColour);
            }
        }

        internal void PlotPathsBreadthFirst(string lastFrame, ConsoleColor pathColour)
        {
            var formattedLastFrame = _formattingHelper.FormatMazeTextForParser(lastFrame);
            var pathPlot = _treeHelper.GetBreadthFirstSearchPlot(formattedLastFrame);

            pathPlot = _formattingHelper.FormatPathPlotsForConsole(pathPlot);

            _consoleHelper.WriteDiffsToConsole(new List<string>() { lastFrame, pathPlot[0] }, 20, pathColour);

            _consoleHelper.WriteDiffsToConsole(pathPlot, 5, pathColour);
        }

        internal void PlotPathsDepthFirst(string lastFrame, ConsoleColor branches, ConsoleColor leaves)
        {
            var formattedLastFrame = _formattingHelper.FormatMazeTextForParser(lastFrame);
            var pathPlots = _treeHelper.GetDepthFirstSearchPlots(formattedLastFrame);

            pathPlots = pathPlots.Select(_formattingHelper.FormatPathPlotsForConsole).ToList();

            for (var i = 0; i < pathPlots.Count; i++)
            {
                var pathPlot = pathPlots[i];

                if (i == 0)
                {
                    _consoleHelper.WriteDiffsToConsole(new List<string>() { lastFrame, pathPlot[0] }, 1, branches);
                }
                else
                {
                    _consoleHelper.WriteDiffsToConsole(new List<string>() { lastFrame, pathPlot[0] }, 1, leaves);
                }

                _consoleHelper.WriteDiffsToConsole(pathPlot, 1, branches);

            }
        }

        internal void PlotBorderAndBackground(string border)
        {
            _consoleHelper.WriteFirstFrame(border);
        }
        internal void PlotMazeBuild(List<string> formattedMazeSteps, ConsoleColor mazeColour)
        {
            _consoleHelper.WriteDiffsToConsole(formattedMazeSteps, 1, mazeColour);
        }
        public void InitialiseConsole(ConsoleColor backgroundColour, ConsoleColor borderColour)
        {
            _consoleHelper.InitialiseConsole(backgroundColour, borderColour);
        }

        internal void ResetConsole(string firstFrame, string lastFrame, ConsoleColor mazeColour)
        {
            _consoleHelper.WriteDiffsToConsole(new List<string>() { firstFrame, lastFrame }, 1, mazeColour);
        }
        internal void PromptIfNotContinuousMode(bool continuousMode, int delay)
        {
            Thread.Sleep(delay);

            if (!continuousMode)
            {
                _consoleHelper.PromptUser();
            }
        }

       
    }
}
