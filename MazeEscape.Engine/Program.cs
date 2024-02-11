using Maze_poc.Enums;
using Maze_poc.Interfaces;

namespace Maze_poc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();

            p.Run();
        }

        private void Run()
        {

            var minmaze =
                "+E+\n" +
                "+ +\n" +
                "+S+\n" +
                "+++";

            var testmaze =
                "+E+++++++++\n" +
                "+ +       +\n" +
                "+ + +++++ +\n" +
                "+ + +   + +\n" +
                "+ + +S+ + +\n" +
                "+ + +++ + +\n" +
                "+ +     + +\n" +
                "+ +++++++ +\n" +
                "+         +\n" +
                "+++++++++++\n";



            IMazeGenerator mazeGenerator = new MazeGenerator();
            IPlayerController playerController = new PlayerController();

            IMazeGame mazeGame = new MazeGame(mazeGenerator, playerController);

            mazeGame.Initialise(testmaze);



            var key = "yNiPC0Se/P5fO2ie4mdmpIIk/IQbGg+AYKrOBGGX1q4=";

            IMazeEncoder mazeEncoder = new MazeEncoder();
            var encoded = mazeEncoder.MazeEncode(mazeGame.GetMaze(), key);

            
           
            var status = "";


            while (true)
            {
                Console.Clear();

                Console.WriteLine("\n" + mazeGame.PrintMaze());

                Console.WriteLine(status);

                if (status.Contains("escaped"))
                {
                     break;
                }

                status = "";
                    

                var vision = mazeGame.GetPlayerVision();

                Console.WriteLine("Facing:" + vision.FacingDirection);

                Console.WriteLine("\nAhead:" + vision.Ahead);

                Console.WriteLine("\nLeft:" + vision.Left.ToString().PadRight(8) +  "    Right:" + vision.Right + "\n");
                

                var x = Console.ReadKey();

                if (x.KeyChar == 'w')
                {
                   status = mazeGame.MovePlayer(PlayerMove.Forward);

                }

                if (x.KeyChar == 'a')
                {
                    mazeGame.MovePlayer(PlayerMove.Left);
                }

                if (x.KeyChar == 'd')
                {
                    mazeGame.MovePlayer(PlayerMove.Right);
                }

            }

            Console.ReadLine();


        }


       

    }
}
