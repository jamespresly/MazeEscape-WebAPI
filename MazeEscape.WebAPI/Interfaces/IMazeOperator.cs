using MazeEscape.WebAPI.DTO;

namespace MazeEscape.WebAPI.Interfaces;

public interface IMazeOperator
{
    MazeCreated CreateMazeFromText(string mazeText);

    void InitialiseMazeFromToken(string token);

    string MovePlayer(Enums.PlayerMove playerMove);

    PlayerInfo GetPlayerInfo();
}