using MazeEscape.Engine.Model;

namespace MazeEscape.Encoder.Interfaces;

public interface IMazeEncoder
{
    string MazeEncode(Maze maze, string encryptionKey);
    Maze MazeDecode(string mazeToken, string encryptionKey);
}