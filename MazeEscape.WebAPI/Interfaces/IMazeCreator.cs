using MazeEscape.WebAPI.DTO;

namespace MazeEscape.WebAPI.Interfaces;

public interface IMazeCreator
{
    string GetMazeInputText(CreateParams createParams);
}