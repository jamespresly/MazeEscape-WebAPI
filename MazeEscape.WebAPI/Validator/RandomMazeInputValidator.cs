using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Interfaces;

namespace MazeEscape.WebAPI.Validator;

public class RandomMazeInputValidator : IMazeInputValidator
{

    public void Validate(CreateParams createParams)
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

    }
}