namespace MazeEscape.Encoder.Interfaces;

public interface IMazeEncoder
{
    string MazeEncode(string mazeString, string encryptionKey);
    string MazeDecode(string mazeToken, string encryptionKey);
}