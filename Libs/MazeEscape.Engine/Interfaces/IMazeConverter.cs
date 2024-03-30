using MazeEscape.Model.Domain;

namespace MazeEscape.Engine.Interfaces;

public interface IMazeConverter
{
    Maze Parse(string text);
    

    string ToText(Maze maze);
}