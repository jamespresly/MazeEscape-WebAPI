using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;

namespace MazeEscape.WebAPI.Interfaces;

public class FakeMazeManager : IMazeManager
{
    public List<string> GetPresets()
    {
        return new List<string>() { "spiral" };
    }

    public string CreateMaze(CreateMode createMode, CreateParams createParams)
    {
        if (createMode == CreateMode.Preset)
        {
            var presetName = createParams.Preset?.PresetName;

            if (string.IsNullOrEmpty(presetName))
                throw new ArgumentException("presetName is required");


            if (!GetPresets().Contains(presetName))
            {
                throw new FileNotFoundException("Preset:" + presetName + " not found");
            }
            else
            {
                return "fakemazetoken";
            }
                
        }

        if (createMode == CreateMode.Custom)
        {
            var mazeText = createParams.Custom?.MazeText;

            if (string.IsNullOrEmpty(mazeText))
                throw new ArgumentException("mazeText is required");

            var allowedChars = new char[]{ '+', ' ', 'S', 'E', '\n' };



            var chars = mazeText.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (!allowedChars.Contains(chars[i]))
                {
                    throw new ArgumentException("mazeText format is incorrect. "
                                                + "Must contain only '+' for walls, ' ' for corridor, 'S' for start point, 'E' for end point and '\\n' only." +
                                                " e.g. \n+E+\n+ +\n+S+\n+++");
                }
            }
            

            return "fakemazetoken";
        }

        if (createMode == CreateMode.Random)
        {
            var height = createParams.Random?.Height;
            var width = createParams.Random?.Width;

            if (height == null || width == null)
                throw new ArgumentException("width and height are required");

            if (height < 3 || width < 3)
                throw new ArgumentException("width and height must be between 3 and 100");

            if(height > 100 || width > 100)
                throw new ArgumentException("width and height must be between 3 and 100");


            return "fakemazetoken";
        }

        return null;
    }

    public PlayerInfo GetPlayerInfo(MazeState? mazeState)
    {
        var token = mazeState?.MazeToken;

        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("mazeToken is required");


        return new PlayerInfo();

    }
}