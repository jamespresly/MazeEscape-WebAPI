using MazeEscape.WebAPI.DTO;
using System.Text.Json;

namespace MazeEscape.WebAPI.Hypermedia
{
    public static class HypermediaDefinitions
    {

        public static Dictionary<LinkType, Link> LinksMap = new()
        {
            { LinkType.GetMazeRoot, new(){ Description = "get-mazes-root", Method = "GET"}},
            { LinkType.GetPresetsList,  new(){ Description = "get-mazes-presets-list", Method = "GET"}},

            { LinkType.CreatePresetMaze, new() { Description = "create-maze-from-preset", Method = "POST", QueryParams = "?createMode=preset"}},
            { LinkType.CreateCustomMaze, new() {Description = "create-maze-from-text", Method = "POST", QueryParams = "?createMode=custom" }},
            { LinkType.CreateRandomMaze, new() {Description = "create-random-maze", Method = "POST", QueryParams = "?createMode=random" }},

            { LinkType.PostPlayer, new(){Description = "post-player", Method = "POST"}},
            { LinkType.PlayerTurnLeft, new(){Description = "player-turn-left", Method = "POST", QueryParams = "?playerMove=turnLeft" }},
            { LinkType.PlayerTurnRight, new(){Description = "player-turn-right", Method = "POST", QueryParams = "?playerMove=turnRight" }},
            { LinkType.PlayerMoveForward, new(){Description = "player-move-forward", Method = "POST", QueryParams = "?playerMove=forward" }},
        };

        private static readonly string CreateParamsPreset = nameof(CreateParams.Preset).ToCamelCase();
        private static readonly string BuildPresetName = nameof(BuildPreset.PresetName).ToCamelCase();

        private static readonly string CreateParamsCustom = nameof(CreateParams.Custom).ToCamelCase();
        private static readonly string BuildCustomMazeTest = nameof(BuildCustom.MazeText).ToCamelCase();

        private static readonly string CreateParamsRandom = nameof(CreateParams.Random).ToCamelCase();

        private static readonly string MazeStateMazeToken = nameof(MazeState.MazeToken).ToCamelCase();


        public static Dictionary<LinkType, Dictionary<string, object>> BodyMap = new()
        {
            { LinkType.CreatePresetMaze, new() 
                {
                    { CreateParamsPreset, new BuildPreset(){ PresetName = "{" +BuildPresetName + "}"}}
                }
            },
            { LinkType.CreateCustomMaze, new()
                {
                    { CreateParamsCustom, new BuildCustom(){ MazeText = "{" + BuildCustomMazeTest + "}"}}
                }
            },
            { LinkType.CreateRandomMaze, new()
                {
                    { CreateParamsRandom, new BuildRandom()}
                }
            },
            { LinkType.PostPlayer, new()
                {
                    { MazeStateMazeToken, "{" + MazeStateMazeToken + "}"}
                }
            },
            { LinkType.PlayerTurnLeft, new()
                {
                    { MazeStateMazeToken, "{" + MazeStateMazeToken + "}"}
                }
            },
            { LinkType.PlayerTurnRight, new()
                {
                    { MazeStateMazeToken, "{" + MazeStateMazeToken + "}"}
                }
            },
            { LinkType.PlayerMoveForward, new()
                {
                    { MazeStateMazeToken, "{" + MazeStateMazeToken + "}"}
                }
            }
        };


        public static string ToCamelCase(this string name)
        {
            return JsonNamingPolicy.CamelCase.ConvertName(name);
        }
    }

   
}
