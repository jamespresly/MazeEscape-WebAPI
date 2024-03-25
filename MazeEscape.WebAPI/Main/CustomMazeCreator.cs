using MazeEscape.Model.Constants;
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

        var allowedChars = new char[] { MazeChars.Wall, MazeChars.Corridor, MazeChars.PlayerStart, MazeChars.Exit, '\n' };


        var chars = mazeText.ToCharArray();

        for (var i = 0; i < chars.Length; i++)
        {
            if (!allowedChars.Contains(chars[i]))
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

        return mazeText;
    }
}