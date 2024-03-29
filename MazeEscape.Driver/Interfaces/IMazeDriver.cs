namespace MazeEscape.Driver.Interfaces;

public interface IMazeDriver
{
    IMazeOperator InitMazeOperator();

    IMazeCreator InitMazeCreator();

}