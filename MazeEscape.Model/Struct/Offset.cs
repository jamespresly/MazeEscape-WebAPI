namespace MazeEscape.Model.Struct;

public struct Offset
{
    public Offset(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }
}