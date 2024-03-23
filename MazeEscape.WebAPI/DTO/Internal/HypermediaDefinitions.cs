using MazeEscape.WebAPI.Enums;

namespace MazeEscape.WebAPI.DTO.Internal;

public class HypermediaDefinitions
{
    public Dictionary<ActionLinkType, string> Actions { get; set; }
    public Dictionary<LinkType, string> Links { get; set; }

}