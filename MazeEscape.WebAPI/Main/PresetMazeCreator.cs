using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.DTO.Internal;
using MazeEscape.WebAPI.Interfaces;

namespace MazeEscape.WebAPI.Main;

public class PresetMazeCreator : IMazeCreator, IPresetFileManager
{
    private readonly MazeManagerConfig _managerConfig;

    public PresetMazeCreator(MazeManagerConfig managerConfig)
    {
        _managerConfig = managerConfig;
    }
    public string CreateMaze(CreateParams createParams)
    {
        var presetName = createParams.Preset?.PresetName;

        if (string.IsNullOrEmpty(presetName))
            throw new ArgumentException("presetName is required");


        if (!GetPresetFileNames().Contains(presetName))
        {
            throw new FileNotFoundException("Preset:" + presetName + " not found");
        }

        var mazeText = File.ReadAllText(_managerConfig.FullPresetsPath + "\\" + presetName + ".txt");

        return mazeText;

    }

    public List<string> GetPresetFileNames()
    {
        var directoryInfo = new DirectoryInfo(_managerConfig.FullPresetsPath);
        var files = directoryInfo.GetFiles();

        var fileNames = files.Select(x => Path.GetFileNameWithoutExtension(x.Name));

        return fileNames.ToList();
    }
}