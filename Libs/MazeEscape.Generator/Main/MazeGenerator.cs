using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Reference;
using MazeEscape.Model.Constants;

namespace MazeEscape.Generator.Main;

public class MazeGenerator : IMazeGenerator
{
    private readonly IGeneratorStrategyBuilder _strategyBuilder;

    public List<string> Steps { get; set; }

    public MazeGenerator(IGeneratorStrategyBuilder strategyBuilder)
    {
        _strategyBuilder = strategyBuilder;
    }

    public string GenerateRandom(int width, int height)
    {
        var generatorStrategy = _strategyBuilder.BuildStrategy();

        var sharedState = generatorStrategy.SharedState;


        sharedState.MazeChars = generatorStrategy.CreateEmpty(width, height);

        var start = generatorStrategy.GetPlayerStart(sharedState.MazeChars);

        sharedState.MazeChars = generatorStrategy.BuildWalls(start, sharedState.MazeChars);

        var exit = generatorStrategy.GetExit(sharedState.MazeChars);

        sharedState.MazeChars = generatorStrategy.FinishMaze(start, exit);

        Steps = sharedState.DebugSteps;

        return MazeToString(sharedState.MazeChars);
    }

    
    private string MazeToString(char[][] mazeChars)
    {
        var concat = string.Join("\n", mazeChars.Select(x => string.Concat(x)));
        var final = concat.Replace(GeneratorConsts.BorderChar.ToString(), MazeChars.Wall.ToString());

        return final;
    }

}