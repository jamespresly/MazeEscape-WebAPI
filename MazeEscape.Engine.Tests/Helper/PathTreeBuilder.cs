using MazeEscape.Engine.Enums;
using MazeEscape.Engine.Model;

namespace MazeEscape.Engine.Tests.Helper
{
    internal class PathTreeBuilder
    {
        
        public MazeNode BuildTree(Maze maze)
        {
            var playerLocation = maze.Player.Location;
            var playerSquare = maze.Squares.FirstOrDefault(x => x.Location.XCoordinate == playerLocation.XCoordinate && x.Location.YCoordinate == playerLocation.YCoordinate);

            var mazeNode = new MazeNode(playerSquare);
            var checkNext = new List<MazeNode>() { mazeNode };


            while (checkNext.Any())
            {
                var childNodes = new List<MazeNode>();

                foreach (var childNode in checkNext)
                {
                    var nodes = BreadthFirstSearch(maze, childNode);
                    childNodes.AddRange(nodes);
                }

                checkNext = childNodes;
            }


            return mazeNode;
        }

        public string GetPathString(string mazeText, List<MazeSquare> path)
        {
            var split = mazeText.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var charRows = split.Select(x => x.ToCharArray()).ToArray();

            foreach (var square in path)
            {
                var row = charRows[square.Location.YCoordinate];
                row[square.Location.XCoordinate] = '|';
            }

            var concat = string.Join("\n", charRows.Select(x => string.Concat(x)));

            return concat;
        }

        private List<MazeNode> BreadthFirstSearch(Maze maze, MazeNode node)
        {
            var surroundingSquares = GetUpDownLeftRight(maze, node.Value);

            var corridorCount = 0;

            var corridorNodes = new List<MazeNode>();

            foreach (var mazeSquare in surroundingSquares)
            {
                if (!node.HasSquareInAncestors(mazeSquare))
                {

                    if (mazeSquare.SquareType != SquareType.Wall)
                    {
                        var corridor = new MazeNode(mazeSquare);
                        node.AddChild(corridorCount, corridor);

                        if (mazeSquare.IsExit)
                        {
                            corridor.MarkAsExitPath();
                        }

                        corridorCount++;

                        corridorNodes.Add(corridor);
                    }
                }
            }

            return corridorNodes;
        }

        private List<MazeSquare> GetUpDownLeftRight(Maze maze, MazeSquare search)
        {
            var surrounding = new List<MazeSquare>();

            var index  = maze.Squares.IndexOf(search);

            if (search.Location.XCoordinate != 0)
            {
                var left = maze.Squares[index - 1];
                surrounding.Add(left);
            }

            var right = maze.Squares[index + 1];
            surrounding.Add(right);


            if (search.Location.YCoordinate > 0)
            {
                var up = maze.Squares[index - maze.Width];
                surrounding.Add(up);
            }

            if (index + maze.Width < maze.Squares.Count)
            {
                var down = maze.Squares[index + maze.Width];
                surrounding.Add(down);
            }

            return surrounding;
        }


       
    }
}
