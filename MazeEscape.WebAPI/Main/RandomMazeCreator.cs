using MazeEscape.Engine.Interfaces;
using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Interfaces;

namespace MazeEscape.WebAPI.Main;

public class RandomMazeCreator : IMazeCreator
{
    private readonly IMazeGenerator _mazeGenerator;

    public RandomMazeCreator(IMazeGenerator mazeGenerator)
    {
        _mazeGenerator = mazeGenerator;
    }
    public string GetMazeInputText(CreateParams createParams)
    {
        var width = createParams?.Random?.Width;
        var height = createParams?.Random?.Height;

        if (width == null || height == null)
        {
            throw new ArgumentException("width and height are required parameters");
        }

        if (width < 10 || height < 10 || width > 50 || height > 50)
        {
            throw new ArgumentException("width and height must be between 10 and 50");
        }

        var maze = _mazeGenerator.GenerateRandom((int)width, (int)height);

        return maze;
    }
}