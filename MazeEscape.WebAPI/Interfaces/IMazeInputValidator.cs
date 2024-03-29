using MazeEscape.WebAPI.DTO;

namespace MazeEscape.WebAPI.Interfaces;

public interface IMazeInputValidator
{
    void Validate(CreateParams createParams);
}