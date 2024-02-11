using MazeEscape.Engine.Model;

namespace MazeEscape.Engine.Interfaces;

public interface IMazeEncoder
{
    string MazeEncode(Maze maze, string encryptionKey);
    Maze MazeDecode(string mazeToken, string encryptionKey);
}