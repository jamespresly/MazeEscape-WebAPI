using MazeEscape.Driver.DTO;

namespace MazeEscape.Driver.Interfaces;

public interface IMazeCreator
{
    MazeCreated CreateRandomMaze(int width, int height);
    MazeCreated CreateCustomMaze(string customInput);
    MazeCreated CreatePresetMaze(string presetName);

    List<string> GetPresets();

}