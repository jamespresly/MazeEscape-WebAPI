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
            var directoryInfo = new DirectoryInfo(path + "\\..\\Presets");
            var files = directoryInfo.GetFiles();

            var fileNames = files.Select(x => Path.GetFileNameWithoutExtension(x.Name));

            return fileNames.ToList();
        }

        public string CreateMaze(CreateMode createMode, CreateParams createParams)
        {
            throw new NotImplementedException();
        }

        public PlayerInfo GetPlayerInfo(MazeState? mazeState)
        {
            throw new NotImplementedException();
        }
    }
}
