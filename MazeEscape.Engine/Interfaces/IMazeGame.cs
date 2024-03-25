using MazeEscape.Model.Domain;
using MazeEscape.Model.Enums;

namespace MazeEscape.Engine.Interfaces;

public interface IMazeGame
{
    
    void Initialise(string text);
    void Initialise(int width, int height);
    void Initialise(Maze maze);

    Maze GetMaze();
    string PrintMaze();

    string MovePlayer(PlayerMove move);
    PlayerVision GetPlayerVision();


}