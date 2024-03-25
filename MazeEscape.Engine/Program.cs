using MazeEscape.Engine.Interfaces;
using MazeEscape.Model.Enums;

namespace MazeEscape.Engine
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
            

            IMazeConverter mazeConverter = new MazeConverter();
            IMazeGenerator mazeGenerator = new MazeGenerator();
            IPlayerNavigator playerNavigator = new PlayerNavigator();

            IMazeGame mazeGame = new MazeGame(mazeConverter, mazeGenerator, playerNavigator);

            mazeGame.Initialise(testmaze);

            var status = "";


            while (true)
            {
                Console.Clear();

                Console.WriteLine(" Use 'w', 'a' and 'd' to navigate\n");

                var maze = mazeGame.PrintMaze();

                Console.WriteLine("".PadRight(8) + maze.Replace("\n", "\n".PadRight(9)));

                Console.WriteLine(" " + status);

                if (status.Contains("escaped"))
                {
                    break;
                }

                status = "";


                var vision = mazeGame.GetPlayerVision();

                Console.WriteLine(" Facing:" + vision.FacingDirection);

                Console.WriteLine("\n Ahead:" + vision.Ahead);

                Console.WriteLine("\n Left:" + vision.Left.ToString().PadRight(8) + "    Right:" + vision.Right + "\n");


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
