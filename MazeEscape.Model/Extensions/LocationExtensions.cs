using MazeEscape.Model.Domain;

namespace MazeEscape.Model.Extensions;

public static class LocationExtensions
{
    public static bool IsSame(this Location source, Location location)
    {
        return source.XCoordinate == location.XCoordinate && source.YCoordinate == location.YCoordinate;
    }
}