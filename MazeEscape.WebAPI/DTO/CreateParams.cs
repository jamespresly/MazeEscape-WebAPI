namespace MazeEscape.WebAPI.DTO
{
    public class CreateParams
    {
        public BuildPreset Preset { get; set; }
        public BuildCustom Custom { get; set; }
        public BuildRandom Random { get; set; }
    }
}
