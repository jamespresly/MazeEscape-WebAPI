namespace MazeEscape.TestClient
{
    internal class MazePrinter
    {

        private readonly Dictionary<string, char> _visionMap = new()
        {
            { "Wall", '█' },
            { "Corridor", ' ' },
            { "Exit", '▒'}
        };

        private readonly Dictionary<string, char> _facingMap = new()
        {
            { "North", '▲' },
            { "East", '►' },
            { "South", '▼' },
            { "West", '◄'}
        };

        private readonly Dictionary<string, Tuple<int, int>> _orientationOffsetMap = new()
        {
            { "North", new(0, -1) },
            { "East", new(1, 0) },
            { "South", new(0, 1) },
            { "West", new(-1, 0) },
        };


        private char[,] _maze = new char[0, 0];


        public void ClearMaze()
        {
            _maze = new char[0, 0];
        }

        public void PrintMaze(dynamic rootData)
        {
            RemovePlayerSymbolFromMaze();

            if (!(bool)rootData.isEscaped)
            {
                var x = (int)rootData.position.x;
                var y = (int)rootData.position.y;

                if (_maze.GetLength(0) < y + 2 || _maze.GetLength(1) < x + 2)
                {
                    IncreaseMazeSize(y, x);
                }

                UpdateMaze(rootData, y, x);
            }

            PrintMaze();
        }

        private void UpdateMaze(dynamic rootData, int y, int x)
        {
            var facing = rootData.facing.ToString();
            var ahead = rootData.vision.ahead.ToString();
            var left = rootData.vision.left.ToString();
            var right = rootData.vision.right.ToString();

            _maze[y, x] = _facingMap[facing];

            Tuple<int, int> forwardOffset = _orientationOffsetMap[facing];

            var offsetList = _orientationOffsetMap.ToList();

            var index = offsetList.IndexOf(new(facing, forwardOffset));

            var prevIndex = index == 0 ? offsetList.Count - 1 : index - 1;
            var nextIndex = (index + 1) % offsetList.Count;

            var leftOffset = offsetList[prevIndex].Value;
            var rightOffset = offsetList[nextIndex].Value;

            var xForwardOffset = forwardOffset.Item1;
            var yForwardOffset = forwardOffset.Item2;

            var xRightOffset = rightOffset.Item1;
            var yRightOffset = rightOffset.Item2;

            var xLeftOffset = leftOffset.Item1;
            var yLeftOffset = leftOffset.Item2;

            _maze[y + yForwardOffset, x + xForwardOffset] = _visionMap[ahead];
            _maze[y + yLeftOffset, x + xLeftOffset] = _visionMap[left];
            _maze[y + yRightOffset, x + xRightOffset] = _visionMap[right];
        }

        private void PrintMaze()
        {
            for (var i = 0; i < _maze.GetLength(0); i++)
            {
                for (int j = 0; j < _maze.GetLength(1); j++)
                {
                    Console.Write(_maze[i, j]);
                }

                Console.Write("\n");
            }
        }

        private void RemovePlayerSymbolFromMaze()
        {
            for (var i = 0; i < _maze.GetLength(0); i++)
            {
                for (int j = 0; j < _maze.GetLength(1); j++)
                {
                    if (_facingMap.Values.Contains(_maze[i, j]))
                    {
                        _maze[i, j] = ' ';
                    }
                }
            }
        }

        private void IncreaseMazeSize(int y, int x)
        {

            var newY = Math.Max(_maze.GetLength(0), y + 2);
            var newX = Math.Max(_maze.GetLength(1), x + 2);

            char[,] tmp = new char[newY, newX];

            for (int i = 0; i < tmp.GetLength(0); i++)
            {
                for (int j = 0; j < tmp.GetLength(1); j++)
                {
                    tmp[i, j] = '░';
                }
            }

            for (var i = 0; i < _maze.GetLength(0); i++)
            {
                for (int j = 0; j < _maze.GetLength(1); j++)
                {
                    tmp[i, j] = _maze[i, j];
                }
            }

            _maze = tmp;
        }

    }
}
