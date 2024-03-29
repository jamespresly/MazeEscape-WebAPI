using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;

namespace MazeEscape.WebAPI.Hypermedia.Definitions;

public static class ActionLinkDefinitions
{
    public static Dictionary<ActionLinkType, ActionLink> ActionsMap = new()
    {
        { ActionLinkType.CreatePresetMaze, new() { Description = "create-maze-from-preset", Method = "POST"}},
        { ActionLinkType.CreateCustomMaze, new() { Description = "create-maze-from-text", Method = "POST"}},
        { ActionLinkType.CreateRandomMaze, new() { Description = "create-random-maze", Method = "POST"}},

        { ActionLinkType.PostPlayer, new() { Description = "post-player", Method = "POST" }},
        
        { ActionLinkType.PlayerTurnLeft, new() { Description = "player-turn-left", Method = "POST" }},
        { ActionLinkType.PlayerTurnRight, new(){ Description = "player-turn-right", Method = "POST" }},
        { ActionLinkType.PlayerMoveForward, new(){ Description = "player-move-forward", Method = "POST" }},
    };
}