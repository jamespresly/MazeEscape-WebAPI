using MazeEscape.Driver.DTO;
using MazeEscape.Driver.Interfaces;
using MazeEscape.Encoder.Interfaces;
using MazeEscape.Encoder;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Engine;
using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Main;
using MazeEscape.Generator.Strategies;

namespace MazeEscape.Driver.Main;

public class MazeDriver : IMazeDriver
{
    
    private readonly MazeManagerConfig _config;

    public MazeDriver(MazeManagerConfig config)
    {
        _config = config;
    }
    public IMazeOperator InitMazeOperator()
    {
        IMazeEncoder mazeEncoder = new MazeEncoder();
        IMazeConverter mazeConverter = new MazeConverter();

        IPlayerNavigator playerNavigator = new PlayerNavigator();
        IMazeEngine mazeEngine = new MazeEngine(mazeConverter, playerNavigator);

        IMazeOperator mazeOperator = new MazeOperator(mazeEngine, mazeConverter, mazeEncoder, _config);

        return mazeOperator;
    }

    public IMazeCreator InitMazeCreator()
    {
        IGeneratorStrategyBuilder generatorStrategyBuilder = new GeneratorStrategyBuilder();
        IMazeGenerator mazeGenerator = new MazeGenerator(generatorStrategyBuilder);
        IMazeEncoder mazeEncoder = new MazeEncoder();
        IMazeConverter mazeConverter = new MazeConverter();

        IPresetFileManager presetFileManager = new PresetFileManager(_config.FullPresetsPath);

        IMazeCreator mazeCreator = new MazeCreator(mazeGenerator, mazeEncoder, mazeConverter, presetFileManager, _config);

        return mazeCreator;
    }
}