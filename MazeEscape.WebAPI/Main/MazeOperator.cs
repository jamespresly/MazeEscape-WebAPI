using MazeEscape.Encoder.Interfaces;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Model.Enums;
using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.DTO.Internal;
using MazeEscape.WebAPI.Interfaces;

namespace MazeEscape.WebAPI.Main;

public class MazeOperator : IMazeOperator
{
    private readonly IMazeGame _mazeGame;
    private readonly IMazeConverter _mazeConverter;
    private readonly IMazeEncoder _mazeEncoder;
    private readonly MazeManagerConfig _managerConfig;

    private string _playerStatus = "";

    private readonly Dictionary<Enums.PlayerMove,PlayerMove> _moveMap = new()
    {
        { Enums.PlayerMove.Forward, PlayerMove.Forward},
        { Enums.PlayerMove.TurnLeft, PlayerMove.Left},
        { Enums.PlayerMove.TurnRight, PlayerMove.Right}
    };

    public MazeOperator(IMazeGame mazeGame, IMazeConverter mazeConverter, IMazeEncoder mazeEncoder, MazeManagerConfig managerConfig)
    {
        _mazeGame = mazeGame;
        _mazeConverter = mazeConverter;
        _mazeEncoder = mazeEncoder;
        _managerConfig = managerConfig;
    }

    public MazeCreated CreateMazeFromText(string mazeText)
    {
        _mazeGame.Initialise(mazeText);
        var maze = _mazeGame.GetMaze();

        var mazeString = _mazeConverter.ToText(maze);

        var token = _mazeEncoder.MazeEncode(mazeString, _managerConfig.MazeEncryptionKey);

        return new MazeCreated()
        {
            MazeToken = token,
            Width = maze.Width,
            Height = maze.Height,
        };
    }

    public void InitialiseMazeFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("mazeToken is required");

        var maze = _mazeEncoder.MazeDecode(token, _managerConfig.MazeEncryptionKey);

        _mazeGame.Initialise(maze);
    }

    public string MovePlayer(Enums.PlayerMove playerMove)
    {
        var move = _moveMap[playerMove];
        _playerStatus = _mazeGame.MovePlayer(move);

        return _playerStatus;
    }

    public PlayerInfo GetPlayerInfo()
    {
        var maze = _mazeGame.GetMaze();

        var mazeString = _mazeConverter.ToText(maze);

        var encoded = _mazeEncoder.MazeEncode(mazeString, _managerConfig.MazeEncryptionKey);

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
            var vision = _mazeGame.GetPlayerVision();
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