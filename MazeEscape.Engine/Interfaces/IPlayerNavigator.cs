using MazeEscape.Engine.Enums;
using MazeEscape.Engine.Model;

namespace MazeEscape.Engine.Interfaces;

public interface IPlayerNavigator
{
    string Move(PlayerMove move, Maze maze);
    PlayerVision GetVision(Maze maze);
}