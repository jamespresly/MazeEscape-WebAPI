using MazeEscape.Generator.DTO;
using MazeEscape.Generator.Struct;

namespace MazeEscape.Generator.Interfaces;

public interface IGeneratorStrategy
{
    public SharedState SharedState { get; set; }

    char[][] CreateEmpty(int width, int height);

    char[][] BuildWalls(Coordinate startPosition, char[][] mazeChars);

    Coordinate GetPlayerStart(char[][] mazeChars);


    Coordinate GetExit(char[][] mazeChars);

    char[][] FinishMaze(Coordinate playerStart, Coordinate exit);
}