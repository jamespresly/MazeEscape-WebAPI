using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Interfaces;

namespace MazeEscape.WebAPI.Validator;

public class PresetMazeInputValidator : IMazeInputValidator
{
   
    public void Validate(CreateParams createParams)
    {
        var presetName = createParams.Preset?.PresetName;

        if (string.IsNullOrEmpty(presetName))
            throw new ArgumentException("presetName is required");

    }

   
}