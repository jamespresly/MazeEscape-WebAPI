using MazeEscape.Encoder.Interfaces;
using MazeEscape.Engine.Interfaces;
using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;
using MazeEscape.WebAPI.Interfaces;



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
            string mazeText = "";

            if (createMode == CreateMode.Preset)
            {
                mazeText = GetPresetMazeText(createParams);
            }
            else if (createMode == CreateMode.Custom)
            {
                mazeText = GetCustomMazeText(createParams);
            }
            else
            {
                throw new NotImplementedException();
            }


            if (string.IsNullOrEmpty(mazeText))
                throw new ArgumentException("mazeText cannot be empty");


            _mazeGame.Initialise(mazeText);
            var maze = _mazeGame.GetMaze();

            var token = _mazeEncoder.MazeEncode(maze, _managerConfig.MazeEncryptionKey);

            return token;
        }

        public PlayerInfo GetPlayerInfo(MazeState? mazeState)
        {
            var token = mazeState?.MazeToken;

            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("mazeToken is required");


            return new PlayerInfo();
        }

        private static string GetCustomMazeText(CreateParams createParams)
        {
        
            var mazeText = createParams.Custom?.MazeText;

            if (string.IsNullOrEmpty(mazeText))
                throw new ArgumentException("mazeText is required");

            var allowedChars = new char[] { '+', ' ', 'S', 'E', '\n' };


            var chars = mazeText.ToCharArray();

            for (var i = 0; i < chars.Length; i++)
            {
                if (!allowedChars.Contains(chars[i]))
                {
                    throw new ArgumentException("mazeText format is incorrect. "
                                                + "Must contain only '+' for walls, ' ' for corridor, 'S' for start point, 'E' for end point and '\\n' only." +
                                                " e.g. \n+E+\n+ +\n+S+\n+++");
                }
            }

            return mazeText;
        }

        private string GetPresetMazeText(CreateParams createParams)
        {
            
            var presetName = createParams.Preset?.PresetName;

            if (string.IsNullOrEmpty(presetName))
                throw new ArgumentException("presetName is required");


            if (!GetPresets().Contains(presetName))
            {
                throw new FileNotFoundException("Preset:" + presetName + " not found");
            }

            var mazeText = File.ReadAllText(_managerConfig.FullPresetsPath + "\\" + presetName + ".txt");

            return mazeText;
        }

       


    
    }
}
