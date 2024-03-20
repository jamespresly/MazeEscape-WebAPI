using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Interfaces;

namespace MazeEscape.WebAPI.Main;

public class CustomMazeCreator : IMazeCreator
{
    public string CreateMaze(CreateParams createParams)
    {
        var mazeText = createParams.Custom?.MazeText;

        if (string.IsNullOrEmpty(mazeText))
            throw new ArgumentException("mazeText is required");

        var allowedChars = new char[] { '+', ' ', 'S', 'E', '\n' };


        var chars = mazeText.ToCharArray();

        for (var i = 0; i < chars.Length; i++)
        {
            if (!allowedChars.Contains(chars[i]))
            {
                throw new ArgumentException("mazeText format is incorrect. "
                                            + "Must contain only '+' for walls, ' ' for corridor, 'S' for start point, 'E' for end point and '\\n' only." +
                                            " e.g. \n+E+\n+ +\n+S+\n+++");
            }
        }

        return mazeText;
    }
}