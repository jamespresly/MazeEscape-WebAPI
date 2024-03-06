using MazeEscape.Encoder.Interfaces;
using MazeEscape.Engine.Interfaces;
using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;
using MazeEscape.WebAPI.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace MazeEscape.WebAPI
{
    public class MazeManager : IMazeManager
    {
        private readonly IMazeGame _mazeGame;
        private readonly IMazeEncoder _mazeEncoder;
        private readonly MazeManagerConfig _managerConfig;

        public MazeManager(IMazeGame mazeGame, IMazeEncoder mazeEncoder, MazeManagerConfig managerConfig)
        {
            _mazeGame = mazeGame;
            _mazeEncoder = mazeEncoder;
            _managerConfig = managerConfig;
        }


        public List<string> GetPresets()
        {
            var directoryInfo = new DirectoryInfo(_managerConfig.FullPresetsPath);
            var files = directoryInfo.GetFiles();

            var fileNames = files.Select(x => Path.GetFileNameWithoutExtension(x.Name));

            return fileNames.ToList();
        }

        public string CreateMaze(CreateMode createMode, CreateParams createParams)
        {
            if (createMode == CreateMode.Preset)
            {
                var presetName = createParams.Preset?.PresetName;

                if (string.IsNullOrEmpty(presetName))
                    throw new ArgumentException("presetName is required");


                if (!GetPresets().Contains(presetName))
                {
                    throw new FileNotFoundException("Preset:" + presetName + " not found");
                }
                else
                {
                    var text = File.ReadAllText(_managerConfig.FullPresetsPath + "\\" + presetName + ".txt");

                    _mazeGame.Initialise(text);
                    var maze = _mazeGame.GetMaze();

                    var token = _mazeEncoder.MazeEncode(maze, _managerConfig.MazeEncryptionKey);

                    return token;
                }
            }
            else if (createMode == CreateMode.Custom)
            {
                var mazeText = createParams.Custom?.MazeText;

                if (string.IsNullOrEmpty(mazeText))
                    throw new ArgumentException("mazeText is required");

                var allowedChars = new char[] { '+', ' ', 'S', 'E', '\n' };



                var chars = mazeText.ToCharArray();

                for (int i = 0; i < chars.Length; i++)
                {
                    if (!allowedChars.Contains(chars[i]))
                    {
                        throw new ArgumentException("mazeText format is incorrect. "
                                                    + "Must contain only '+' for walls, ' ' for corridor, 'S' for start point, 'E' for end point and '\\n' only." +
                                                    " e.g. \n+E+\n+ +\n+S+\n+++");
                    }
                }


                _mazeGame.Initialise(mazeText);
                var maze = _mazeGame.GetMaze();

                var token = _mazeEncoder.MazeEncode(maze, _managerConfig.MazeEncryptionKey);

                return token;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public PlayerInfo GetPlayerInfo(MazeState? mazeState)
        {
            throw new NotImplementedException();
        }

     
    }
}
