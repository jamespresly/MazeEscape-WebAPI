using FluentAssertions;
using MazeEscape.Engine.Enums;
using MazeEscape.Engine.Interfaces;

namespace MazeEscape.Engine.Tests
{
    public class PlayerControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void VisonTest()
        {
            var minmaze =
                "+E+\n" +
                "+ +\n" +
                "+S+\n" +
                "+++";

            IMazeConverter mazeConverter = new MazeConverter();
            

            var maze = mazeConverter.GenerateFromText(minmaze);

            IPlayerController playerController = new PlayerController();


            var vision  = playerController.GetVision(maze);

            vision.Ahead.Should().Be(SquareType.Corridor);


        }
    }
}