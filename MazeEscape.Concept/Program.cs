using MazeEscape.Engine;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Model.Enums;

namespace MazeEscape.Concept
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
            IPlayerNavigator playerNavigator = new PlayerNavigator();

            IMazeEngine mazeEngine = new MazeEngine(mazeConverter, playerNavigator);

            mazeEngine.Initialise(testmaze);

            var status = "";


            while (true)
            {
                Console.Clear();

                Console.WriteLine(" Use 'w', 'a' and 'd' to navigate\n");

                var maze = mazeEngine.PrintMaze();

                Console.WriteLine("".PadRight(8) + maze.Replace("\n", "\n".PadRight(9)));

                Console.WriteLine(" " + status);

                if (status.Contains("escaped"))
                {
                    break;
                }

                status = "";


                var vision = mazeEngine.GetPlayerVision();

                Console.WriteLine(" Facing:" + vision.FacingDirection);

                Console.WriteLine("\n Ahead:" + vision.Ahead);

                Console.WriteLine("\n Left:" + vision.Left.ToString().PadRight(8) + "    Right:" + vision.Right + "\n");


                var x = Console.ReadKey();

                if (x.KeyChar == 'w')
                {
                    status = mazeEngine.MovePlayer(PlayerMove.Forward);

                }

                if (x.KeyChar == 'a')
                {
                    mazeEngine.MovePlayer(PlayerMove.Left);
                }

                if (x.KeyChar == 'd')
                {
                    mazeEngine.MovePlayer(PlayerMove.Right);
                }

            }

            Console.ReadLine();


        }

    }
}
