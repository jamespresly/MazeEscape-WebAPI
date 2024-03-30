using MazeEscape.Driver.DTO;
using MazeEscape.Driver.Interfaces;
using MazeEscape.Encoder.Interfaces;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Generator.Interfaces;

namespace MazeEscape.Driver.Main
{
    internal class MazeCreator : IMazeCreator
    {
        private readonly IMazeGenerator _mazeGenerator;
        private readonly IMazeEncoder _mazeEncoder;
        private readonly IMazeConverter _mazeConverter;
        private readonly IPresetFileManager _presetFileManager;
        private readonly MazeManagerConfig _config;

        public MazeCreator(IMazeGenerator mazeGenerator,
                           IMazeEncoder mazeEncoder,
                           IMazeConverter mazeConverter,
                           IPresetFileManager presetFileManager,
                           MazeManagerConfig config)
        {
            _mazeGenerator = mazeGenerator;
            _mazeEncoder = mazeEncoder;
            _mazeConverter = mazeConverter;
            _presetFileManager = presetFileManager;
            _config = config;
        }
        public MazeCreated CreateRandomMaze(int width, int height)
        {
            var randomMazeInput = _mazeGenerator.GenerateRandom(width, height);

            return GetTokenFromInput(randomMazeInput);
        }

        public MazeCreated CreateCustomMaze(string customInput)
        {
            return GetTokenFromInput(customInput);
        }

        public MazeCreated CreatePresetMaze(string presetName)
        {
            if (!GetPresets().Contains(presetName))
            {
                throw new FileNotFoundException("Preset:" + presetName + " not found");
            }

            var presetInputText = File.ReadAllText(_config.FullPresetsPath + "\\" + presetName + ".txt");

            return GetTokenFromInput(presetInputText);
        }

        public List<string> GetPresets()
        {
            return _presetFileManager.GetPresetFileNames();
        }

        private MazeCreated GetTokenFromInput(string input)
        {
            var maze = _mazeConverter.Parse(input);
            var mazeString = _mazeConverter.ToText(maze);

            var token = _mazeEncoder.MazeEncode(mazeString, _config.MazeEncryptionKey);

            return new MazeCreated()
            {
                MazeToken = token,
                Width = maze.Width,
                Height = maze.Height,
            };
        }
    }
}
