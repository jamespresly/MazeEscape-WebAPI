using FluentAssertions;
using MazeEscape.Engine;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Model.Enums;

namespace MazeEscape.Tests
{
    public class PlayerNavigatorTests
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
            

            var maze = mazeConverter.Parse(minmaze);

            IPlayerNavigator playerNavigator = new PlayerNavigator();


            var vision  = playerNavigator.GetVision(maze);

            vision.Ahead.Should().Be(SquareType.Corridor);
            vision.Left.Should().Be(SquareType.Wall);
            vision.Right.Should().Be(SquareType.Wall);

            playerNavigator.Move(PlayerMove.Right, maze);

            vision = playerNavigator.GetVision(maze);

            vision.Ahead.Should().Be(SquareType.Wall);
            vision.Left.Should().Be(SquareType.Corridor);
            vision.Right.Should().Be(SquareType.Wall);

            playerNavigator.Move(PlayerMove.Right, maze);

            vision = playerNavigator.GetVision(maze);

            vision.Ahead.Should().Be(SquareType.Wall);
            vision.Left.Should().Be(SquareType.Wall);
            vision.Right.Should().Be(SquareType.Wall);

            playerNavigator.Move(PlayerMove.Right, maze);

            vision = playerNavigator.GetVision(maze);

            vision.Ahead.Should().Be(SquareType.Wall);
            vision.Left.Should().Be(SquareType.Wall);
            vision.Right.Should().Be(SquareType.Corridor);

            playerNavigator.Move(PlayerMove.Right, maze);

            vision = playerNavigator.GetVision(maze);

            vision.Ahead.Should().Be(SquareType.Corridor);
            vision.Left.Should().Be(SquareType.Wall);
            vision.Right.Should().Be(SquareType.Wall);
        }
    }
}