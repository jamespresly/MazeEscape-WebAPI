using System.Diagnostics;
using FluentAssertions;
using MazeEscape.Engine;
using MazeEscape.Generator;
using MazeEscape.Tests.Helper;

namespace MazeEscape.Tests;

public class MazeGeneratorTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CreateSmallRandomTest()
    {
        var mazeGenerator = new MazeGenerator();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        for (var size = 3; size < 10; size++)
        {
          

            var random = mazeGenerator.GenerateRandom(size, size);

            Console.WriteLine("time:" + stopwatch.ElapsedMilliseconds);
            Console.WriteLine(random);

            random.Should().NotContain("=");

            random.Where(c => c == 'S').Should().HaveCount(1, "should have one start point");
            random.Where(c => c == 'E').Should().HaveCount(1, "should have one exit");
        }
      
    }

    [Test]
    public void CreateLargeRandomTest()
    {
        var mazeGenerator = new MazeGenerator();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var size = 100;
        var random = mazeGenerator.GenerateRandom(size, size);

        Console.WriteLine("time:" + stopwatch.ElapsedMilliseconds);
        Console.WriteLine(random);

        random.Should().NotContain("=");

        random.Where(c => c == 'S').Should().HaveCount(1, "should have one start point");
        random.Where(c => c == 'E').Should().HaveCount(1, "should have one exit");
        
    }

    [Test]
    public void CreateNonSquareRandomTest()
    {
        var mazeGenerator = new MazeGenerator();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var random = mazeGenerator.GenerateRandom(30, 20);

        Console.WriteLine("time:" + stopwatch.ElapsedMilliseconds);
        Console.WriteLine(random);

        random.Should().NotContain("=");

        var randomChars = random.ToCharArray();

        random.Where(c => c == 'S').Should().HaveCount(1, "should have one start point");
        random.Where(c => c == 'E').Should().HaveCount(1, "should have one exit");

    }

    [Test]
    public void CreateManyRandomsTest()
    {
        var mazeGenerator = new MazeGenerator();

        var stopwatch = new Stopwatch();

        for (var i = 10; i < 70; i++)
        {
            stopwatch.Start();

            var random = mazeGenerator.GenerateRandom(i, i);

            Console.WriteLine("time:" + stopwatch.ElapsedMilliseconds);
            Console.WriteLine(random);

            random.Should().NotContain("=");

            random.Where(c => c == 'S').Should().HaveCount(1, "should have one start point");
            random.Where(c => c == 'E').Should().HaveCount(1, "should have one exit");

            stopwatch.Reset();
        }


    }

    [Test]
    public void ManyPathTest()
    {
        var pathTreeBuilder = new PathTreeBuilder();
        var mazeConverter = new MazeConverter();


        var stopwatch = new Stopwatch();


        for (var size = 10; size < 50; size++)
        {
            var mazeGenerator = new MazeGenerator();

            stopwatch.Start();

            var random = mazeGenerator.GenerateRandom(size, size);

            random.Should().NotContain("=");

          
            Console.WriteLine(random);

            var maze = mazeConverter.Parse(random);

            Debug.WriteLine("getting paths");

            var tree = pathTreeBuilder.BuildTree(maze);
            var paths = tree.GetPaths(tree);

            Debug.WriteLine("done");


            var hasExitPath = false;

            if (paths.Any())
            {
                Console.WriteLine("total paths:" + paths.Count());
                paths = paths.Where(c => c[^1].IsExit).OrderBy(x=>x.Count).ToList();

                if (paths.Any())
                {
                    hasExitPath = true;
                    Console.WriteLine("size:" + (size + size) + " exit path:" + paths.First().Count);
                }

            }

            if (!hasExitPath)
            {
                Console.WriteLine(random);
                Console.WriteLine("no exit path found");
            }
                
            hasExitPath.Should().BeTrue();
        }
    }

    [Test]
    public void PathTest()
    {
        var mazeGenerator = new MazeGenerator();
        var random = mazeGenerator.GenerateRandom(30, 30);

        random.Should().NotContain("=");


        var pathTreeBuilder = new PathTreeBuilder();
        var mazeConverter = new MazeConverter();

        Console.WriteLine(random);

        var maze = mazeConverter.Parse(random);

        var tree = pathTreeBuilder.BuildTree(maze);
        var paths = tree.GetPaths(tree);

        var mazeText = mazeConverter.ToText(maze);

        var hasExitPath = false;

        foreach (var path in paths)
        {
            var pathFormatted = pathTreeBuilder.GetPathString(mazeText, path);

            if (path[^1].IsExit)
            {
                Console.WriteLine(pathFormatted);
                hasExitPath = true;
                break;
            }

        }

        hasExitPath.Should().BeTrue();

    }
}