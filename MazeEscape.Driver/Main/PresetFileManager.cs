using MazeEscape.Driver.Interfaces;

namespace MazeEscape.Driver.Main
{
    internal class PresetFileManager : IPresetFileManager
    {
        private readonly string _fullPresetsPath;


        public PresetFileManager(string fullPresetsPath)
        {
            _fullPresetsPath = fullPresetsPath;
        }
        public List<string> GetPresetFileNames()
        {
            var directoryInfo = new DirectoryInfo(_fullPresetsPath);
            var files = directoryInfo.GetFiles();

            var fileNames = files.Select(x => Path.GetFileNameWithoutExtension(x.Name));

            return fileNames.ToList();
        }
    }
}
