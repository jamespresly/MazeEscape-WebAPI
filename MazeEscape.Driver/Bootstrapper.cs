using MazeEscape.Driver.DTO;
using MazeEscape.Driver.Interfaces;
using MazeEscape.Driver.Main;
using MazeEscape.Encoder;
using MazeEscape.Encoder.Interfaces;
using MazeEscape.Engine;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Generator.Interfaces;
using MazeEscape.Generator.Main;

namespace MazeEscape.Driver
{
    internal class Bootstrapper
    {
        public static IMazeOperator GetMazeOperator(MazeManagerConfig config)
        {
            IMazeEncoder mazeEncoder = new MazeEncoder();
            IMazeConverter mazeConverter = new MazeConverter();

            IPlayerNavigator playerNavigator = new PlayerNavigator();
            IMazeEngine mazeEngine = new MazeEngine(mazeConverter, playerNavigator);

            IMazeOperator mazeOperator = new MazeOperator(mazeEngine, mazeConverter, mazeEncoder, config);

            return mazeOperator;
        }
        public static IMazeCreator GetMazeCreator(MazeManagerConfig config)
        {

            IMazeGenerator mazeGenerator = new MazeGenerator();
            IMazeEncoder mazeEncoder = new MazeEncoder();
            IMazeConverter mazeConverter = new MazeConverter();

            IPresetFileManager presetFileManager = new PresetFileManager(config.FullPresetsPath);

            IMazeCreator mazeCreator = new MazeCreator(mazeGenerator, mazeEncoder, mazeConverter, presetFileManager, config);

            return mazeCreator;
        }
    }
}
