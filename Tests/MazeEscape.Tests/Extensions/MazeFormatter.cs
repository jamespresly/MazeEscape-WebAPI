namespace MazeEscape.Tests.Extensions;

public static class MazeFormatter
{
    public static string FormatForConsole(this string maze)
    {
        return maze.Replace("+", "█");
    }
}