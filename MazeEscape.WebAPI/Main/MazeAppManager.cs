using MazeEscape.Driver.Interfaces;
using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;
using MazeEscape.WebAPI.Interfaces;
using MazeEscape.WebAPI.Validator;


namespace MazeEscape.WebAPI.Main
{
    public class MazeAppManager : IMazeAppManager
    {

        private readonly IMazeDriver _mazeDriver;
        private readonly IEnumerable<IMazeInputValidator> _mazeInputValidators;

        private readonly Dictionary<CreateMode, Type> _creatorMap = new()
        {
            { CreateMode.Preset, typeof(PresetMazeInputValidator)},
            { CreateMode.Custom, typeof(CustomMazeInputValidator)},
            { CreateMode.Random, typeof(RandomMazeInputValidator)}
        };

        private readonly Dictionary<PlayerMove, Model.Enums.PlayerMove> _moveMap = new()
        {
            { PlayerMove.Forward, Model.Enums. PlayerMove.Forward},
            { PlayerMove.TurnLeft,  Model.Enums. PlayerMove.Left},
            { PlayerMove.TurnRight, Model.Enums.  PlayerMove.Right}
        };

        public MazeAppManager(IMazeDriver mazeDriver, IEnumerable<IMazeInputValidator> mazeInputValidators)
        {
            _mazeDriver = mazeDriver;
            _mazeInputValidators = mazeInputValidators;
        }

        public List<string> GetPresets()
        {
            var mazeCreator = _mazeDriver.InitMazeCreator();

            return mazeCreator.GetPresets();
        }

        public PlayerInfo GetPlayerInfo(PlayerParams playerParams)
        {
            var mazeOperator = _mazeDriver.InitMazeOperator();

            mazeOperator.InitialiseMazeFromToken(playerParams.MazeToken);

            if (playerParams.PlayerMove != null)
            {
                var move = _moveMap[(PlayerMove)playerParams.PlayerMove];
                mazeOperator.MovePlayer(move);
            }

            var player = mazeOperator.GetPlayerInfo();
            var position = player.Position;
            var vision = player.Vision;

            return new PlayerInfo()
            {
                MazeToken = player.MazeToken,
                Info = player.Info,
                Facing =  player.Facing,
                Position = new Position()
                {
                    X = position.X,
                    Y = position.Y,
                },
                Vision = player.Vision == null ? null : new Vision()
                {
                    Ahead = vision.Ahead,
                    Left = vision.Left,
                    Right = vision.Right
                },
                IsEscaped = player.IsEscaped
            };
        }

        public MazeCreated CreateMaze(CreateParams createParams)
        {
            var validator = _mazeInputValidators.FirstOrDefault(x => x.GetType() == _creatorMap[createParams.CreateMode]);

            if (validator == null)
                throw new ArgumentException("maze input validator not found");

            validator.Validate(createParams);

            Driver.DTO.MazeCreated? created = Create(createParams);

            if (created == null)
                throw new ArgumentException("maze creation failed");

            return new MazeCreated()
            {
                MazeToken = created.MazeToken,
                Width = created.Width,
                Height = created.Height
            };

        }

        private Driver.DTO.MazeCreated? Create(CreateParams createParams)
        {
            var mazeCreator = _mazeDriver.InitMazeCreator();

            Driver.DTO.MazeCreated? created = null;

            if (createParams.CreateMode == CreateMode.Custom)
            {
                created = mazeCreator.CreateCustomMaze(createParams.Custom.MazeText);
            }

            if (createParams.CreateMode == CreateMode.Preset)
            {
                created = mazeCreator.CreatePresetMaze(createParams.Preset.PresetName);
            }

            if (createParams.CreateMode == CreateMode.Random)
            {
                created = mazeCreator.CreateRandomMaze((int)createParams.Random.Width, (int)createParams.Random.Height);
            }

            return created;
        }
    }
}
