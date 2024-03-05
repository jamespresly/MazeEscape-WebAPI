using MazeEscape.Engine.Model;

namespace MazeEscape.Engine.Interfaces;

public interface IMazeGenerator
{
    Maze GenerateRandom(int width, int height);
}