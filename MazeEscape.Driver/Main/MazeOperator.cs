using MazeEscape.Driver.DTO;
using MazeEscape.Driver.Interfaces;
using MazeEscape.Encoder.Interfaces;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Model.Enums;

namespace MazeEscape.Driver.Main;

public class MazeOperator : IMazeOperator
{
    private readonly IMazeEngine _mazeEngine;
    private readonly IMazeConverter _mazeConverter;
    private readonly IMazeEncoder _mazeEncoder;
    private readonly MazeManagerConfig _config;

    private string _playerStatus = "";

    public MazeOperator(IMazeEngine mazeEngine, IMazeConverter mazeConverter, IMazeEncoder mazeEncoder, MazeManagerConfig config)
    {
        _mazeEngine = mazeEngine;
        _mazeConverter = mazeConverter;
        _mazeEncoder = mazeEncoder;
        _config = config;
    }
    public void InitialiseMazeFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("mazeToken is required");

        var maze = _mazeEncoder.MazeDecode(token, _config.MazeEncryptionKey);

        _mazeEngine.Initialise(maze);
    }

    public string MovePlayer(PlayerMove playerMove)
    {
        _playerStatus = _mazeEngine.MovePlayer(playerMove);

        return _playerStatus;
    }

    public PlayerInfo GetPlayerInfo()
    {
        var maze = _mazeEngine.GetMaze();

        var mazeString = _mazeConverter.ToText(maze);

        var encoded = _mazeEncoder.MazeEncode(mazeString, _config.MazeEncryptionKey);

        var info = new PlayerInfo()
        {
            MazeToken = encoded,
            Facing = maze.Player.FacingDirection.ToString(),
            Info = _playerStatus,
            Position = new Position()
            {
                X = maze.Player.Location.XCoordinate,
                Y = maze.Player.Location.YCoordinate
            }
        };

        if (!_playerStatus.Contains("escaped"))
        {
            var vision = _mazeEngine.GetPlayerVision();
            info.Vision = new Vision
            {
                Ahead = vision.Ahead.ToString(),
                Left = vision.Left.ToString(),
                Right = vision.Right.ToString(),
            };
        }
        else
        {
            info.IsEscaped = true;
        }

        return info;

    }
}