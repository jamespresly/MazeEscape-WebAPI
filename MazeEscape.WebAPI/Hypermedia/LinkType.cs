namespace MazeEscape.WebAPI.Hypermedia;

public enum LinkType
{
    GetMazeRoot,
    GetPresetsList,
    CreatePresetMaze,
    CreateCustomMaze,
    CreateRandomMaze,
    PostPlayer,
    PlayerTurnLeft,
    PlayerTurnRight,
    PlayerMoveForward
}