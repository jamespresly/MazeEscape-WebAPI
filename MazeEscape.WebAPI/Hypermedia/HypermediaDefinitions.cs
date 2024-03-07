using MazeEscape.WebAPI.DTO;
using System.Text.Json;

namespace MazeEscape.WebAPI.Hypermedia
{
    public static class HypermediaDefinitions
    {
        public static Dictionary<LinkType, Link> LinksMap = new()
        {
            { LinkType.GetMazeRoot, new (){ Description = "get-mazes-root", Method = "GET"}},
            { LinkType.GetPresetsList,  new (){ Description = "get-mazes-presets-list", Method = "GET"}},

            { LinkType.CreatePresetMaze, new() { Description = "create-maze-from-preset", Method = "POST", QueryParams = "?createMode=preset"}},
            { LinkType.CreateCustomMaze, new Link() {Description = "create-maze-from-text", Method = "POST", QueryParams = "?createMode=custom" }},
            { LinkType.CreateRandomMaze, new Link() {Description = "create-random-maze", Method = "POST", QueryParams = "?createMode=random" }},

            { LinkType.PostPlayer, new Link(){Description = "post-player", Method = "POST"}},
            { LinkType.PlayerTurnLeft, new Link(){Description = "player-turn-left", Method = "POST", QueryParams = "?playerMove=turnLeft" }},
            { LinkType.PlayerTurnRight, new Link(){Description = "player-turn-right", Method = "POST", QueryParams = "?playerMove=turnRight" }},
            { LinkType.PlayerMoveForward, new Link(){Description = "player-move-forward", Method = "POST", QueryParams = "?playerMove=forward" }},
        };

        public static Dictionary<LinkType, Dictionary<string, object>> BodyMap = new()
        {
            { LinkType.CreatePresetMaze, new Dictionary<string, object>() 
                {
                    { nameof(CreateParams.Preset).ToCamelCase(), new BuildPreset(){ PresetName = "{" + nameof(BuildPreset.PresetName).ToCamelCase() + "}"}}
                }
            },
            { LinkType.CreateCustomMaze, new Dictionary<string, object>()
                {
                    { nameof(CreateParams.Custom).ToCamelCase(), new BuildCustom(){ MazeText = "{" + nameof(BuildCustom.MazeText).ToCamelCase() + "}"}}
                }
            },
            { LinkType.CreateRandomMaze, new Dictionary<string, object>()
                {
                    { nameof(CreateParams.Random).ToCamelCase(), new BuildRandom()}
                }
            },
            { LinkType.PostPlayer, new Dictionary<string, object>()
                {
                    { nameof(MazeState.MazeToken).ToCamelCase(), "{" + nameof(MazeState.MazeToken).ToCamelCase() + "}"}
                }
            },{ LinkType.PlayerTurnLeft, new Dictionary<string, object>()
                {
                    { nameof(MazeState.MazeToken).ToCamelCase(), "{" + nameof(MazeState.MazeToken).ToCamelCase() + "}"}
                }
            },{ LinkType.PlayerTurnRight, new Dictionary<string, object>()
                {
                    { nameof(MazeState.MazeToken).ToCamelCase(), "{" + nameof(MazeState.MazeToken).ToCamelCase() + "}"}
                }
            },{ LinkType.PlayerMoveForward, new Dictionary<string, object>()
                {
                    { nameof(MazeState.MazeToken).ToCamelCase(), "{" + nameof(MazeState.MazeToken).ToCamelCase() + "}"}
                }
            },
        };

        public static string ToCamelCase(this string name)
        {
            return JsonNamingPolicy.CamelCase.ConvertName(name);
        }
    }

   
}
