namespace MazeEscape.Engine.Struct;

internal struct Side
{
    public Side(int left, int right)
    {
        Left = left;
        Right = right;
    }
    public int Left { get; set; }
    public int Right { get; set; }
}