using MazeEscape.Engine.Model;

namespace MazeEscape.Engine.Interfaces;

public interface IMazeGenerator
{
    Maze GenerateFromText(string text);
    Maze GenerateRandom(int width, int height);
}