using System.Diagnostics;
using FluentAssertions;
using MazeEscape.Encoder.Interfaces;
using System.Text;
using MazeEscape.Engine;
using MazeEscape.Engine.Interfaces;


namespace MazeEscape.Encoder.Tests
{
    public class EncoderTests
    {

        private string testKey = "yNiPC0Se/P5fO2ie4mdmpIIk/IQbGg+AYKrOBGGX1q4=";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EncoderTest()
        {
            IMazeConverter mazeConverter = new MazeConverter();
            IMazeEncoder mazeEncoder = new MazeEncoder(mazeConverter);

            var minmaze = "+E+\n" + 
                                "+ +\n" +
                                "+S+\n" +
                                "+++";

            var inMaze = mazeConverter.GenerateFromText(minmaze);

            var encoded = mazeEncoder.MazeEncode(inMaze, testKey);

            var outMaze = mazeEncoder.MazeDecode(encoded, testKey);

            var text = mazeConverter.ToText(outMaze);


            outMaze.Height.Should().Be(inMaze.Height);
            outMaze.Width.Should().Be(inMaze.Width);
            outMaze.Should().BeEquivalentTo(inMaze);

        }

        [Test]
        public void Benchmark()
        {
            var st = new Stopwatch();

            IMazeConverter mazeConverter = new MazeConverter();
            IMazeEncoder mazeEncoder = new MazeEncoder(mazeConverter);

            var min = 3;
            var max = 100;

            var times = new List<long>();

            for (var i = min; i <= max; i++)
            {
                st.Reset();

                var maze = GetRandom(i);

                st.Start();

                var m = mazeConverter.GenerateFromText(maze);

                var encoded = mazeEncoder.MazeEncode(m, testKey);

                var decoded = mazeEncoder.MazeDecode(encoded, testKey);

                var text = mazeConverter.ToText(decoded);

                st.Stop();

                times.Add(st.ElapsedMilliseconds);

                Console.WriteLine("Size:" + i + " Encoding took:" + st.ElapsedMilliseconds + "ms");

            }

            var average = Math.Round(times.Average(),2);

            Console.WriteLine("Average time:" + average);

            average.Should().BeLessOrEqualTo(5);
        }

        private string GetRandom(int size)
        {
            var rand = new Random();

            var sb = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var r = rand.NextInt64(2);

                    sb.Append(r == 0 ? " " : "+");

                }

                sb.Append("\n");
            }

            sb.Remove(sb.Length - 1, 1);

            var maze = sb.ToString().ToCharArray();

            maze[0] = 'S';
            maze[1] = 'E';

            return new string(maze);
        }
    }
}