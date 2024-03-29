using MazeEscape.Driver.DTO;
using MazeEscape.Driver.Interfaces;

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
        return Bootstrapper.GetMazeOperator(_config);
    }

    public IMazeCreator InitMazeCreator()
    {
        return Bootstrapper.GetMazeCreator(_config);
    }
}