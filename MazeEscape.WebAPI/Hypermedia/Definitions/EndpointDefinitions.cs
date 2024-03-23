using MazeEscape.WebAPI.Controllers;
using MazeEscape.WebAPI.DTO.Internal;
using MazeEscape.WebAPI.Enums;

namespace MazeEscape.WebAPI.Hypermedia.Definitions;

public static class EndpointDefinitions
{

    public static Dictionary<string, HypermediaDefinitions> HypermediaDefinitions = new()
    {
        {
            nameof(MazesController.GetMazes), new()
            {
                Actions = new()
                {
                    { ActionLinkType.CreatePresetMaze, nameof(MazesController.CreateMaze)},
                    { ActionLinkType.CreateCustomMaze, nameof(MazesController.CreateMaze)},
                    { ActionLinkType.CreateRandomMaze, nameof(MazesController.CreateMaze)}
                },
                Links = new()
                {
                    { LinkType.GetMazeRoot, nameof(MazesController.GetMazes)},
                    { LinkType.GetPresetsList, nameof(MazesController.GetPresets)}
                }
            }
        },
        {
            nameof(MazesController.CreateMaze), new()
            {
                Actions = new()
                {
                    { ActionLinkType.PostPlayer, nameof(MazesController.PostPlayer) }
                },
                Links = new()
                {
                    {LinkType.GetMazeRoot, nameof(MazesController.GetMazes)}
                }
            }
        },
        {
            nameof(MazesController.GetPresets), new()
            {
                Actions = new()
                {
                    { ActionLinkType.CreatePresetMaze, nameof(MazesController.CreateMaze) }
                },
                Links = new()
                {
                    { LinkType.GetMazeRoot, nameof(MazesController.GetMazes)}
                }
            }
        },
        {
            nameof(MazesController.PostPlayer), new()
            {
                Actions = new()
                {
                    { ActionLinkType.PostPlayer,  nameof(MazesController.PostPlayer) },
                    { ActionLinkType.PlayerTurnLeft, nameof(MazesController.PostPlayer) },
                    { ActionLinkType.PlayerTurnRight, nameof(MazesController.PostPlayer) },
                    { ActionLinkType.PlayerMoveForward, nameof(MazesController.PostPlayer) }
                },
                Links = new()
                {
                    { LinkType.GetMazeRoot, nameof(MazesController.GetMazes) }
                }
            }
        }
        
    };
}