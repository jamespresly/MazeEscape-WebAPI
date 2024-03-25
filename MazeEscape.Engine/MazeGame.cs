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
            Maze = _mazeConverter.Parse(text);
        }

        public void Initialise(int width, int height)
        {
            if (width < 10 || height < 10)
                throw new ArgumentException("Minimum size: 10 x 10");

            if (width > 50 || height > 50)
                throw new ArgumentException("Maximum size: 50 x 50");

            var mazeText = _mazeGenerator.GenerateRandom(width, height);
            Maze = _mazeConverter.Parse(mazeText);

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
