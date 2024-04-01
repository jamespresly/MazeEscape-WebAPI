using MazeEscape.Generator.Main;
using MazeEscape.Generator.Strategies.EdgeCases;
using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Struct;

namespace MazeEscape.Generator.Strategies;

public class GeneratorStrategyBuilder : IGeneratorStrategyBuilder
{
    private SharedState _sharedState;
    private MazeReader _mazeReader;
    private MazeWriter _mazeWriter;

    private readonly bool _debug;

    public GeneratorStrategyBuilder(bool debug = false)
    {
        _debug = debug;
    }

    public IGeneratorStrategy BuildStrategy()
    {
        _sharedState = new SharedState()
        {
            CorridorsToCheck = new List<Coordinate>(),
            Unvisited = new List<Coordinate>(),
            DebugSteps = new List<string>(),
            Debug = _debug
        };

        _mazeReader = new MazeReader();
        _mazeWriter = new MazeWriter(_sharedState);

        var edgeCases = new List<IEdgeCase>()
        {
            new WallCrossEdgeCase(_mazeWriter),
            new EnclosureEdgeCase(_mazeWriter),
            new PartEnclosureEdgeCase(_mazeWriter)
        };

        IWallBuildingStrategy wallBuildingStrategy = new RandomWalkStrategy(_mazeReader, _mazeWriter, _sharedState);
        IEdgeCaseManager edgeCaseManager = new EdgeCaseManager(edgeCases, _mazeReader, _sharedState);
        IGeneratorStrategy generatorStrategy = new GeneratorStrategy(wallBuildingStrategy, edgeCaseManager, _sharedState);
        
        return generatorStrategy;
    }


}