using MazeEscape.Engine.Interfaces;
using MazeEscape.Model.Domain;
using MazeEscape.Model.Enums;

namespace MazeEscape.Engine
{
    public class MazeGame : IMazeGame
    {
        public Maze Maze { get; set; }


        private readonly IMazeConverter _mazeConverter;
        private readonly IMazeGenerator _mazeGenerator;
        private readonly IPlayerNavigator _playerNavigator;

        
        public MazeGame(IMazeConverter mazeConverter, IMazeGenerator mazeGenerator, IPlayerNavigator playerNavigator)
        {
            _mazeConverter = mazeConverter;
            _mazeGenerator = mazeGenerator;
            _playerNavigator = playerNavigator;
        }
        public void Initialise(Maze maze)
        {
            Maze = maze;
        }

        public void Initialise(string text)
        {
            Maze = _mazeConverter.GenerateFromText(text);
        }

        public void Initialise(int width, int height)
        {
            if (width < 3 || height < 3)
                throw new ArgumentException("Minimum size: 3 x 3");

            if (width > 100 || height > 100)
                throw new ArgumentException("Maximum size: 100 x 100");

            var mazeText = _mazeGenerator.GenerateRandom(width, height);
            Maze = _mazeConverter.GenerateFromText(mazeText);

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
