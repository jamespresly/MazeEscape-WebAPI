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
            { CreateMode.Random, typeof(Random)}
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

        public PlayerInfo GetPlayerInfo(MazeState mazeState, PlayerMove? playerMove)
        {
            _mazeEngineManager.InitialiseMaze(mazeState.MazeToken);

            if (playerMove != null)
            {
                _mazeEngineManager.MovePlayer((PlayerMove)playerMove);
            }

            return _mazeEngineManager.GetPlayerInfo();
        }

        public string CreateMaze(CreateMode createMode, CreateParams createParams)
        {
            var creator = _mazeCreators.FirstOrDefault(x => x.GetType() == _creatorMap[createMode]);

            if (creator == null)
                throw new ArgumentException("mazecreator not found");

            var mazeText = creator.CreateMaze(createParams);

            if (string.IsNullOrEmpty(mazeText))
                throw new ArgumentException("mazeText cannot be empty");

            return _mazeEngineManager.CreateMazeToken(mazeText);
        }

    }
}
