using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;
using MazeEscape.WebAPI.Interfaces;


namespace MazeEscape.WebAPI.Main
{
    public class MazeAppManager : IMazeAppManager
    {

        private readonly IPresetFileManager _presetFileManager;
        private readonly IMazeEngineManager _mazeEngineManager;
        private readonly IEnumerable<IMazeCreator> _mazeCreators;

        private readonly Dictionary<CreateMode, Type> _creatorMap = new()
        {
            { CreateMode.Preset, typeof(PresetMazeCreator)},
            { CreateMode.Custom, typeof(CustomMazeCreator)},
            { CreateMode.Random, typeof(RandomMazeCreator)}
        };

        public MazeAppManager( IMazeEngineManager mazeEngineManager, IEnumerable<IMazeCreator> mazeCreators, IPresetFileManager presetFileManager)
        {
            _mazeEngineManager = mazeEngineManager;
            _mazeCreators = mazeCreators;
            _presetFileManager = presetFileManager;
        }

        public List<string> GetPresets()
        {
            return _presetFileManager.GetPresetFileNames();
        }

        public PlayerInfo GetPlayerInfo(PlayerParams playerParams)
        {
            _mazeEngineManager.InitialiseMazeFromToken(playerParams.MazeToken);

            if (playerParams.PlayerMove != null)
            {
                _mazeEngineManager.MovePlayer((PlayerMove)playerParams.PlayerMove);
            }

            return _mazeEngineManager.GetPlayerInfo();
        }

        public MazeCreated CreateMaze(CreateMode createMode, CreateParams createParams)
        {
            var creator = _mazeCreators.FirstOrDefault(x => x.GetType() == _creatorMap[createMode]);

            if (creator == null)
                throw new ArgumentException("mazecreator not found");

            var mazeText = creator.CreateMaze(createParams);

            if (string.IsNullOrEmpty(mazeText))
                throw new ArgumentException("mazeText cannot be empty");

            var created = _mazeEngineManager.CreateMazeFromText(mazeText);

            return created;
        }

    }
}
