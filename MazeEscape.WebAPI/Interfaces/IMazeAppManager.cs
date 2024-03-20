using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;

namespace MazeEscape.WebAPI.Interfaces;

public interface IMazeAppManager
{
    List<string> GetPresets();
    string CreateMaze(CreateMode createMode, CreateParams createParams);
    PlayerInfo GetPlayerInfo(MazeState mazeState, PlayerMove? playerMove);

}