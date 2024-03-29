using MazeEscape.WebAPI.DTO;


namespace MazeEscape.WebAPI.Interfaces;

public interface IMazeAppManager
{
    List<string> GetPresets();
    MazeCreated CreateMaze(CreateParams createParams);
    PlayerInfo GetPlayerInfo(PlayerParams playerParam);

}