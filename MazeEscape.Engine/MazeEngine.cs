using MazeEscape.Engine.Interfaces;
using MazeEscape.Model.Domain;
using MazeEscape.Model.Enums;

namespace MazeEscape.Engine
{
    public class MazeEngine : IMazeEngine
    {
        public Maze Maze { get; set; }


        private readonly IMazeConverter _mazeConverter;
        private readonly IPlayerNavigator _playerNavigator;

        
        public MazeEngine(IMazeConverter mazeConverter, IPlayerNavigator playerNavigator)
        {
            _mazeConverter = mazeConverter;
            _playerNavigator = playerNavigator;
        }

        public void Initialise(string text)
        {
            Maze = _mazeConverter.Parse(text);
        }

        public Maze GetMaze()
        {
            return Maze;
        }

        public string MovePlayer(PlayerMove move)
        {
           return _playerNavigator.Move(move, Maze);
        }

        public PlayerVision GetPlayerVision()
        {
            return _playerNavigator.GetVision(Maze);
        }

        public string PrintMaze()
        {
            return _mazeConverter.ToText(Maze);
        }


    }
}
