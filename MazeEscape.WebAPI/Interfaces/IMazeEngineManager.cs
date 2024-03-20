using MazeEscape.WebAPI.DTO;

namespace MazeEscape.WebAPI.Interfaces;

public interface IMazeEngineManager
{
    string CreateMazeToken(string mazeText);

    void InitialiseMaze(string token);

    string MovePlayer(Enums.PlayerMove playerMove);

    PlayerInfo GetPlayerInfo();
}