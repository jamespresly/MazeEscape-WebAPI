using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;

namespace MazeEscape.WebAPI.Hypermedia.Definitions
{
    public static class LinkDefinitions
    {

        public static Dictionary<LinkType, Link> LinksMap = new()
        {
            { LinkType.GetMazeRoot, new() { Description = "get-mazes-root", Method = "GET" }},
            { LinkType.GetPresetsList,  new() { Description = "get-mazes-presets-list", Method = "GET" }}
        };
    }
}
