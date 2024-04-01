using MazeEscape.Generator.Struct;

namespace MazeEscape.Generator.Interfaces;

internal interface IWallBuildingStrategy
{
    void Init(Coordinate startPosition);
    void NavigateAndBuild(char[][] mazeChars);
}