using MazeEscape.WebAPI.Enums;

namespace MazeEscape.WebAPI.DTO
{
    public class CreateParams
    {
        public CreateMode CreateMode { get; set; }
        public BuildPreset? Preset { get; set; }
        public BuildCustom? Custom { get; set; }
        public BuildRandom? Random { get; set; }
    }
}
