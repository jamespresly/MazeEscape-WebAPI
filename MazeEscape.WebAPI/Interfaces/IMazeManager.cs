using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;

namespace MazeEscape.WebAPI.Interfaces;

public interface IMazeManager
{
    List<string> GetPresets(string rootPath);
    string CreateMaze(CreateMode createMode, CreateParams createParams);
    PlayerInfo GetPlayerInfo(MazeState? mazeState);
}