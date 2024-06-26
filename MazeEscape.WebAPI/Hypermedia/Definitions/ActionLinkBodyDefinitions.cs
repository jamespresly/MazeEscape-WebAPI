﻿using System.Text.Json;
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

    private static readonly string MazeStateMazeToken = nameof(PlayerParams.MazeToken).ToCamelCase();


    public static Dictionary<ActionLinkType, Dictionary<string, object>> ActionBodyMap = new()
    {
        { ActionLinkType.CreatePresetMaze, new()
        {
            { nameof(CreateMode), CreateMode.Preset.ToString().ToCamelCase()}, 
            { CreateParamsPreset, new BuildPreset(){ PresetName = "{" +BuildPresetName + "}" }}
        }},
        { ActionLinkType.CreateCustomMaze, new()
        {
            { nameof(CreateMode), CreateMode.Custom.ToString().ToCamelCase()},
            { CreateParamsCustom, new BuildCustom(){ MazeText = "{" + BuildCustomMazeTest + "}" }}
        }},
        { ActionLinkType.CreateRandomMaze, new()
        {
            { nameof(CreateMode), CreateMode.Random.ToString().ToCamelCase()},
            { CreateParamsRandom, new { width = "{width}", height = "{height}" }}
        }},
          
        { ActionLinkType.PostPlayer, new() {{ MazeStateMazeToken, "{" + MazeStateMazeToken + "}" }}},
          
        { ActionLinkType.PlayerTurnLeft, new() {{ MazeStateMazeToken, "{" + MazeStateMazeToken + "}" }, { nameof(PlayerMove), PlayerMove.TurnLeft.ToString().ToCamelCase()}}},
        { ActionLinkType.PlayerTurnRight, new() {{ MazeStateMazeToken, "{" + MazeStateMazeToken + "}"}, { nameof(PlayerMove), PlayerMove.TurnRight.ToString().ToCamelCase()}}},
        { ActionLinkType.PlayerMoveForward, new() {{ MazeStateMazeToken, "{" + MazeStateMazeToken + "}"}, { nameof(PlayerMove), PlayerMove.Forward.ToString().ToCamelCase()}}}
    };

    public static string ToCamelCase(this string name)
    {
        return JsonNamingPolicy.CamelCase.ConvertName(name);
    }
}