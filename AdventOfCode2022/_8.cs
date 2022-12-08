namespace AdventOfCode2022;
public class _8 : Base
{
    protected override void Action()
    {
        //UseExample();
        int gridSize = InputLines[0].Length;
        Tree[,] grid = new Tree[gridSize, gridSize];
        for (int y = 0; y < gridSize; y++)
        {
            string line = InputLines[y];
            for (int x = 0; x < gridSize; x++)
            {
                int h = line[x] - '0';
                grid[x, y] = new(h, x, y);
            }
        }
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Tree twig = grid[x, y];
                twig.SetTreesLeft(grid.GetRangeX(y, 0, x).ToList());
                twig.SetTreesRight(grid.GetRangeX(y, x + 1, gridSize - x - 1).ToList());
                twig.SetTreesUp(grid.GetRangeY(x, 0, y).ToList());
                twig.SetTreesDown(grid.GetRangeY(x, y + 1, gridSize - y - 1).ToList());
            }
        }

        int answer = grid.Cast<Tree>().Where(t => t.Visible).Count();
        WriteLine(answer);

        B();

        int score = grid.Cast<Tree>().Select(x => x.Score).Max();
        WriteLine(score);
    }
}

public class Tree
{
    public int h; // height
    public int x, y;
    public List<Tree> left, right, up, down;
    public bool visibleLeft, visibleRight, visibleUp, visibleDown;

    public Tree(int h, int x, int y)
    {
        this.h = h;
        this.x = x;
        this.y = y;
        this.left = new List<Tree>();
        this.right = new List<Tree>();
        this.up = new List<Tree>();
        this.down = new List<Tree>();
    }

    public int Score
    {
        get
        {
            var dists = new List<List<Tree>>
            {
                Enumerable.Reverse(left).ToList(),
                right,
                Enumerable.Reverse(up).ToList(),
                down,
            }.Select(l => ViewDist(l)).ToList();
            return dists.Aggregate((a, b) => a * b);
        }
    }

    private int ViewDist(List<Tree> trees)
    {
        int d = 0;
        foreach (Tree tree in trees)
        {
            d++;
            if (tree.h >= this.h) break;
        }
        return d;
    }

    public void SetTreesLeft(List<Tree> trees)
    {
        this.left = trees;
        this.visibleLeft = trees.All(t => t.h < this.h);
    }
    public void SetTreesRight(List<Tree> trees)
    {
        this.right = trees;
        this.visibleRight = trees.All(t => t.h < this.h);
    }
    public void SetTreesUp(List<Tree> trees)
    {
        this.up = trees;
        this.visibleUp = trees.All(t => t.h < this.h);
    }
    public void SetTreesDown(List<Tree> trees)
    {
        this.down = trees;
        this.visibleDown = trees.All(t => t.h < this.h);
    }

    public bool Visible { get => visibleLeft || visibleRight || visibleUp || visibleDown; }
}
