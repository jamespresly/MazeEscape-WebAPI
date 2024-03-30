using MazeEscape.Model.Domain;
using MazeEscape.Model.Enums;

namespace MazeEscape.Engine.Interfaces;

public interface IMazeEngine
{
    void Initialise(string text);

    Maze GetMaze();
    string PrintMaze();

    string MovePlayer(PlayerMove move);
    PlayerVision GetPlayerVision();


}