using MazeEscape.Generator.Struct;

namespace MazeEscape.Generator.Interfaces
{
    internal interface IEdgeCase
    {
        bool Processed(MazeScan mazeScan);
    }
}
