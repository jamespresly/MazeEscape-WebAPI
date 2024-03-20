using MazeEscape.Encoder.Interfaces;
using MazeEscape.Engine.Interfaces;
using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Interfaces;

namespace MazeEscape.WebAPI.Main;

public class MazeEngineManager : IMazeEngineManager
{
    private readonly IMazeGame _mazeGame;
    private readonly IMazeEncoder _mazeEncoder;
    private readonly MazeManagerConfig _managerConfig;

    private string _playerStatus = "";

    private readonly Dictionary<Enums.PlayerMove, Engine.Enums.PlayerMove> _moveMap = new()
    {
        { Enums.PlayerMove.Forward, Engine.Enums.PlayerMove.Forward},
        { Enums.PlayerMove.TurnLeft, Engine.Enums.PlayerMove.Left},
        { Enums.PlayerMove.TurnRight, Engine.Enums.PlayerMove.Right}
    };

    public MazeEngineManager(IMazeGame mazeGame, IMazeEncoder mazeEncoder, MazeManagerConfig managerConfig)
    {
        _mazeGame = mazeGame;
        _mazeEncoder = mazeEncoder;
        _managerConfig = managerConfig;
    }

    public string CreateMazeToken(string mazeText)
    {
        _mazeGame.Initialise(mazeText);
        var maze = _mazeGame.GetMaze();

        var token = _mazeEncoder.MazeEncode(maze, _managerConfig.MazeEncryptionKey);
        return token;
    }

    public void InitialiseMaze(string token)
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
      
        var encoded = _mazeEncoder.MazeEncode(maze, _managerConfig.MazeEncryptionKey);

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