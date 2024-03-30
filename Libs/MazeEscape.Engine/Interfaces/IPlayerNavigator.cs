using MazeEscape.Model.Domain;
using MazeEscape.Model.Enums;

namespace MazeEscape.Engine.Interfaces;

public interface IPlayerNavigator
{
    string Move(PlayerMove move, Maze maze);
    PlayerVision GetVision(Maze maze);
}