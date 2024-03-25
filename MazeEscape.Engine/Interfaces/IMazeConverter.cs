using MazeEscape.Model.Domain;

namespace MazeEscape.Engine.Interfaces;

public interface IMazeConverter
{
    Maze GenerateFromText(string text);
    

    string ToText(Maze maze);
}