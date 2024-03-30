namespace MazeEscape.GeneratorDemo.Helper
{
    internal class FormattingHelper
    {
        internal string FormatMazeTextForParser(string mazeText)
        {
            return mazeText.Replace("█", "+");
        }

        internal List<string> FormatPathPlotsForConsole(List<string> plots)
        {
            return plots.Select(s => s
                .Replace("#", "█")
                .Replace("+", "█")).ToList();
        }

        internal List<string> FormatMazeStepsForConsole(List<string> steps)
        {
            return steps.Select(s => s
                .Replace("X", "█")
                .Replace("+", "█")
                .Replace("=", "-")).ToList();

            return steps;
        }
    }
}
