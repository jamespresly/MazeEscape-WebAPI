using MazeEscape.Engine.Model;

namespace MazeEscape.Engine.Extensions;

internal static class LocationExtensions
{
    public static bool IsSame(this Location source, Location location)
    {
        return source.XCoordinate == location.XCoordinate && source.YCoordinate == location.YCoordinate;
    }
}