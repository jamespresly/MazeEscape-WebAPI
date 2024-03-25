using System.Diagnostics;
using System.Text;
using FluentAssertions;
using MazeEscape.Encoder;
using MazeEscape.Encoder.Interfaces;

namespace MazeEscape.Tests
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
            IMazeEncoder mazeEncoder = new MazeEncoder();

            var input = "+E+\n" + 
                        "+ +\n" +
                        "+S+\n" +
                        "+++";

          
            var encoded = mazeEncoder.MazeEncode(input, testKey);

            var outMaze = mazeEncoder.MazeDecode(encoded, testKey);

            outMaze.Should().BeEquivalentTo(input);
        }

        [Test]
        public void Benchmark()
        {
            var st = new Stopwatch();

            IMazeEncoder mazeEncoder = new MazeEncoder();

            var min = 3;
            var max = 100;

            var times = new List<long>();

            for (var i = min; i <= max; i++)
            {
                st.Reset();

                var mazeString = GetRandom(i);

                st.Start();

                var encoded = mazeEncoder.MazeEncode(mazeString, testKey);

                var decoded = mazeEncoder.MazeDecode(encoded, testKey);

                decoded.Should().BeEquivalentTo(mazeString);

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