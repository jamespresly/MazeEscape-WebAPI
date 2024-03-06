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

        public MazeManager(IMazeGame mazeGame, IMazeEncoder mazeEncoder)
        {
            _mazeGame = mazeGame;
            _mazeEncoder = mazeEncoder;
        }


        public List<string> GetPresets(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            var files = directoryInfo.GetFiles();

            var fileNames = files.Select(x => Path.GetFileNameWithoutExtension(x.Name));

            return fileNames.ToList();
        }

        public string CreateMaze(CreateMode createMode, CreateParams createParams, string encryptionKey, string path = "")
        {
            if (createMode == CreateMode.Preset)
            {
                var presetName = createParams.Preset?.PresetName;

                if (string.IsNullOrEmpty(presetName))
                    throw new ArgumentException("presetName is required");


                if (!GetPresets(path).Contains(presetName))
                {
                    throw new FileNotFoundException("Preset:" + presetName + " not found");
                }
                else
                {
                    var text = File.ReadAllText(path + "\\" + presetName + ".txt");

                    _mazeGame.Initialise(text);
                    var maze = _mazeGame.GetMaze();

                    var token = _mazeEncoder.MazeEncode(maze, encryptionKey);

                    return token;
                }
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
