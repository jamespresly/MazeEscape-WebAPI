using MazeEscape.Driver.DTO;
using MazeEscape.Model.Enums;

namespace MazeEscape.Driver.Interfaces;

public interface IMazeOperator
{

    void InitialiseMazeFromToken(string token);

    string MovePlayer(PlayerMove playerMove);

    PlayerInfo GetPlayerInfo();
}