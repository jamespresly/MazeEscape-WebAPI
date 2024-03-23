using MazeEscape.WebAPI.DTO;

namespace MazeEscape.WebAPI.Interfaces;

public interface IMazeEngineManager
{
    MazeCreated CreateMazeFromText(string mazeText);

    void InitialiseMazeFromToken(string token);

    string MovePlayer(Enums.PlayerMove playerMove);

    PlayerInfo GetPlayerInfo();
}