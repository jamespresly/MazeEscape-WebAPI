using System.Text.Json;
using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Enums;

namespace MazeEscape.WebAPI.Hypermedia.Definitions;

public static class ActionLinkBodyDefinitions
{
    private static readonly string CreateParamsPreset = nameof(CreateParams.Preset).ToCamelCase();
    private static readonly string BuildPresetName = nameof(BuildPreset.PresetName).ToCamelCase();
    private static readonly string CreateParamsCustom = nameof(CreateParams.Custom).ToCamelCase();
    private static readonly string BuildCustomMazeTest = nameof(BuildCustom.MazeText).ToCamelCase();
    private static readonly string CreateParamsRandom = nameof(CreateParams.Random).ToCamelCase();

    private static readonly string MazeStateMazeToken = nameof(MazeState.MazeToken).ToCamelCase();


    public static Dictionary<ActionLinkType, Dictionary<string, object>> ActionBodyMap = new()
    {
        { ActionLinkType.CreatePresetMaze, new() {{ CreateParamsPreset, new BuildPreset(){ PresetName = "{" +BuildPresetName + "}" }}}},
        { ActionLinkType.CreateCustomMaze, new() {{ CreateParamsCustom, new BuildCustom(){ MazeText = "{" + BuildCustomMazeTest + "}" }}}},
        { ActionLinkType.CreateRandomMaze, new() {{ CreateParamsRandom, new { width = "{width}", height = "{height}" }}}},
          
        { ActionLinkType.PostPlayer, new() {{ MazeStateMazeToken, "{" + MazeStateMazeToken + "}" }}},
          
        { ActionLinkType.PlayerTurnLeft, new() {{ MazeStateMazeToken, "{" + MazeStateMazeToken + "}" }}},
        { ActionLinkType.PlayerTurnRight, new() {{ MazeStateMazeToken, "{" + MazeStateMazeToken + "}"}}},
        { ActionLinkType.PlayerMoveForward, new() {{ MazeStateMazeToken, "{" + MazeStateMazeToken + "}"}}}
    };

    public static string ToCamelCase(this string name)
    {
        return JsonNamingPolicy.CamelCase.ConvertName(name);
    }
}