using MazeEscape.Model.Enums;

namespace MazeEscape.Model.Extensions;

public static class OrientationExtensions
{

    public static Orientation TurnClockwise(this Orientation src)
    {
        var arr = (Orientation[])Enum.GetValues(src.GetType());
        var i = Array.IndexOf(arr, src) + 1;

        return arr.Length == i ? arr[0] : arr[i];
    }

    public static Orientation TurnAnticlockwise(this Orientation src) 
    {
        var arr = (Orientation[])Enum.GetValues(src.GetType());
        var i = Array.IndexOf(arr, src) - 1;

        return i == -1 ? arr[^1] : arr[i];
        
    }
}