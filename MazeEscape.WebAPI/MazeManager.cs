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


        public List<string> GetPresets()
        {
            throw new NotImplementedException();
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
