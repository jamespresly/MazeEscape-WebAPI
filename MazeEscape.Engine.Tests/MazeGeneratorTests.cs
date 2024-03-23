using MazeEscape.Engine.Tests.Helper;

namespace MazeEscape.Engine.Tests;

public class MazeGeneratorTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [Ignore("")]
    public void CreateRandomTest()
    {
        var mazeGenerator = new MazeGenerator();

        var size = 20;

        var random = mazeGenerator.GenerateRandom(size, size);

        Console.WriteLine(random);
    }

    
    [Test]
    [Ignore("")]
    public void PathTest()
    {
        var testMaze =
            "++++++++++\n" +
            "+       ++\n" +
            "+ +++++ ++\n" +
            "+ +      +\n" +
            "+ + ++++ +\n" +
            "+ + +S   +\n" +
            "+ +   ++++\n" +
            "+ ++++++ E\n" +
            "+        +\n" +
            "++++++++++\n";

        var pathTreeBuilder = new PathTreeBuilder();
        var mazeConverter = new MazeConverter();

        Console.WriteLine(testMaze);

        var maze = mazeConverter.GenerateFromText(testMaze);

        var tree = pathTreeBuilder.BuildTree(maze);
        var paths = tree.GetPaths(tree);

        var mazeText = mazeConverter.ToText(maze);

        foreach (var path in paths)
        {
            var pathFormatted = pathTreeBuilder.GetPathString(mazeText, path);

            if (path[^1].IsExit)
                Console.WriteLine(pathFormatted);
        }

    }




}