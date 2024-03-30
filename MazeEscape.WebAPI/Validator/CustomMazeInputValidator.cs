using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Interfaces;

namespace MazeEscape.WebAPI.Validator;

public class CustomMazeInputValidator : IMazeInputValidator
{
    private const char Wall = '+';
    private const char Corridor = ' ';
    private const char Start = 'S';
    private const char Exit = 'E';

    public void Validate(CreateParams createParams)
    {
        var mazeText = createParams.Custom?.MazeText;

        if (string.IsNullOrEmpty(mazeText))
            throw new ArgumentException("mazeText is required");


        var allowedChars = new char[] { Wall, Corridor, Start, Exit, '\n' };

        var chars = mazeText.ToCharArray();

        foreach (var c in chars)
        {
            if (!allowedChars.Contains(c))
            {
                throw new ArgumentException("mazeText format is incorrect. "
                                            + $"Must contain only '{Wall}' for walls, '{Corridor}' for corridor, '{Start}' for start point, '{Exit}' for end point and '\\n' only."
                                            + $" e.g. "
                                            + $"\n{Wall}{Exit}{Wall}"
                                            + $"\n{Wall}{Corridor}{Wall}"
                                            + $"\n{Wall}{Start}{Wall}"
                                            + $"\n{Wall}{Wall}{Wall}");
            }
        }

    }


}