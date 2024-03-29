using MazeEscape.Model.Domain;

namespace MazeEscape.Tests.Helper;

public class MazeNode
{
    public MazeNode? Parent { get; set; }
    public MazeSquare Value { get; set; }
    public SortedDictionary<int, MazeNode> Children { get; private set; }
    public bool IsExitPath { get; set; }


    public MazeNode(MazeSquare value)
    {
        Parent = null;
        Value = value;
        Children = new SortedDictionary<int, MazeNode>();
    }

    public void AddChild(int key, MazeNode child)
    {
        child.Parent = this;
        Children.Add(key, child);
    }

    public void MarkAsExitPath()
    {
        IsExitPath = true;

        var node = Parent;

        while (node != null)
        {
            node.IsExitPath = true;
            node = node.Parent;
        }
    }

    public bool HasSquareInAncestors(MazeSquare value)
    {
        var node = this;

        while (node != null)
        {
            if (node.Value == value)
                return true;

            node = node.Parent;
        }
        return false;
    }

    public IEnumerable<List<MazeSquare>> GetPaths(MazeNode mazeNode)
    {
        
        foreach (var child in mazeNode.Children.Values)
        {
            if (!child.Children.Any())
            {
                var path = new List<MazeSquare>();
                var parent = child.Parent;

                path.Add(child.Value);

                while (parent != null)
                {
                    path.Add(parent.Value);
                    parent = parent.Parent;
                }

                path.Reverse();

                if (child.IsExitPath)
                {
                    //Console.WriteLine("exit path of length:" + path.Count);
                }

                yield return path;
            }
            else
            {
                var childPaths = GetPaths(child);

                foreach (var childPath in childPaths)
                {
                    yield return childPath;
                }
            }
        }
    }

    public void PrintTree(MazeNode node, string indent = "")
    {
        Console.WriteLine($"{indent}{node.Value.Location.XCoordinate}-{node.Value.Location.YCoordinate}");

        foreach (var child in node.Children.Values)
        {
            PrintTree(child, indent + "  ");
        }
    }

}