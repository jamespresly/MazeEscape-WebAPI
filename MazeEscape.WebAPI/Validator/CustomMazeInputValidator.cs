using MazeEscape.Model.Constants;
using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Interfaces;

namespace MazeEscape.WebAPI.Validator;

public class CustomMazeInputValidator : IMazeInputValidator
{
    public void Validate(CreateParams createParams)
    {
        var mazeText = createParams.Custom?.MazeText;

        if (string.IsNullOrEmpty(mazeText))
            throw new ArgumentException("mazeText is required");

        var allowedChars = new char[] { MazeChars.Wall, MazeChars.Corridor, MazeChars.PlayerStart, MazeChars.Exit, '\n' };


        var chars = mazeText.ToCharArray();

        foreach (var c in chars)
        {
            if (!allowedChars.Contains(c))
            {
                throw new ArgumentException("mazeText format is incorrect. "
                                            + $"Must contain only '{MazeChars.Wall}' for walls, '{MazeChars.Corridor}' for corridor, '{MazeChars.PlayerStart}' for start point, '{MazeChars.Exit}' for end point and '\\n' only."
                                            + $" e.g. "
                                            + $"\n{MazeChars.Wall}{MazeChars.Exit}{MazeChars.Wall}"
                                            + $"\n{MazeChars.Wall}{MazeChars.Corridor}{MazeChars.Wall}"
                                            + $"\n{MazeChars.Wall}{MazeChars.PlayerStart}{MazeChars.Wall}"
                                            + $"\n{MazeChars.Wall}{MazeChars.Wall}{MazeChars.Wall}");
            }
        }

    }


}