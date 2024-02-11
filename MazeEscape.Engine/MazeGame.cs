using MazeEscape.Engine.Enums;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Engine.Model;

namespace MazeEscape.Engine
{
    public class MazeGame : IMazeGame
    {
        public Maze Maze { get; set; }


        private readonly IMazeGenerator _mazeGenerator;
        private readonly IPlayerController _playerController;

      

        public MazeGame(IMazeGenerator mazeGenerator, IPlayerController playerController)
        {
            _mazeGenerator = mazeGenerator;
            _playerController = playerController;
        }
        public void Initialise(Maze maze)
        {
            Maze = maze;
        }

        public void Initialise(string text)
        {
            Maze = _mazeGenerator.GenerateFromText(text);
        }

        public void Initialise(int width, int height)
        {
            if (width < 3 || height < 3)
                throw new ArgumentException("Minimum size: 3 x 3");

            if (width > 100 || height > 100)
                throw new ArgumentException("Maximum size: 100 x 100");

            Maze = _mazeGenerator.GenerateRandom(width, height);

        }

        public Maze GetMaze()
        {
            return Maze;
        }

        public string MovePlayer(PlayerMove move)
        {
           return _playerController.Move(move, Maze);
        }

        public PlayerVision GetPlayerVision()
        {
            return _playerController.GetVision(Maze);
        }

        public string PrintMaze()
        {
            return Maze.ToString();
        }


    }
}
