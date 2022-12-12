using System.Security.Permissions;

namespace AdventOfCode2022;

public class _12 : Base
{
    protected override void Action()
    {
        //UseExample();
        Square[,] grid = new Square[InputLines[0].Length, InputLines.Count];
        Square start = null!, end = null!;
        for (int y = 0; y < InputLines.Count; y++)
        {
            string line = InputLines[y];
            for (int x = 0; x < line.Length; x++)
            {
                Square s = new Square(x, y, line[x]);
                grid[x, y] = s;
                if (line[x] == 'S')
                {
                    s.inTree = true;
                    start = s;
                }
                if (line[x] == 'E')
                    end = s;
                if (x > 0)
                {
                    grid[x - 1, y].SetNeighbour(Ind.RIGHT, s);
                    s.SetNeighbour(Ind.LEFT, grid[x - 1, y]);
                }
                if (y > 0)
                {
                    grid[x, y - 1].SetNeighbour(Ind.DOWN, s);
                    s.SetNeighbour(Ind.UP, grid[x, y - 1]);
                }
            }
        }

        Queue<Square> toExpand = new();
        toExpand.Enqueue(start);

        Queue<Square> nextRound = new();

        int steps = 0;

        bool searching = true;
        while (searching)
        {
            steps++;
            while (searching && toExpand.Count > 0)
            {
                Square s = toExpand.Dequeue();
                List<Square> neighbours = s.GetViableNeighbours();
                foreach (Square n in neighbours)
                {
                    n.cameFrom = s;
                    n.inTree = true;
                    if (n.end)
                    {
                        searching = false;
                        break;
                    }
                    nextRound.Enqueue(n);
                }
            }
            while (searching && nextRound.Count > 0)
            {
                toExpand.Enqueue(nextRound.Dequeue());
            }
        }

        Square path = end;
        while (path.cameFrom != null)
        {
            path.cameFrom.wentTo = path;
            path = path.cameFrom;
        }


        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                Write(grid[x, y].DirectionString());
            }
            WriteLine();
        }

        WriteLine(steps);

        B();

        List<Square> starts = grid.Cast<Square>().Where(s => s.height == 'a').ToList();

        List<int> dists = new();

        int c = 0;
        foreach (Square thisstart in starts)
        {
            c++;
            Console.WriteLine($"Using start {c} of {starts.Count}");
            foreach (Square s in grid)
            {
                s.cameFrom = null;
                s.wentTo = null;
                s.inTree = false;
            }
            thisstart.inTree = true;

            Queue<Square> toExpand2 = new();
            toExpand2.Enqueue(thisstart);

            Queue<Square> nextRound2 = new();

            int steps2 = 0;

            bool searching2 = true;
            while (searching2)
            {
                steps2++;
                while (searching2 && toExpand2.Count > 0)
                {
                    Square s = toExpand2.Dequeue();
                    List<Square> neighbours = s.GetViableNeighbours();
                    foreach (Square n in neighbours)
                    {
                        n.cameFrom = s;
                        n.inTree = true;
                        if (n.end)
                        {
                            searching2 = false;
                            break;
                        }
                        nextRound2.Enqueue(n);
                    }
                }
                while (searching2 && nextRound2.Count > 0)
                {
                    toExpand2.Enqueue(nextRound2.Dequeue());
                }
                if (nextRound2.Count == 0 && toExpand2.Count == 0)
                {
                    steps2 = int.MaxValue;
                    break;
                }
            }
            dists.Add(steps2);
        }
        WriteLine(dists.Min());
    }
}

public class Square
{
    public bool inTree = false;
    public char height;
    public bool start, end;
    public int x, y;

    public Square? cameFrom, wentTo;

    private Dictionary<int, Square?> neighbours = new();

    public Square(int x, int y, char c)
    {
        this.x = x;
        this.y = y;

        height = c switch
        {
            'S' => 'a',
            'E' => 'z',
            _ => c
        };
        start = c == 'S';
        end = c == 'E';

        for (int i = 0; i < 4; i++)
            neighbours.Add(i, null);
    }

    public void SetNeighbour(int dir, Square n) => neighbours[dir] = n;

    public List<Square> GetNeighbours() => neighbours.Values.Where(s => s != null).Select(s => s!).ToList();

    public List<Square> GetNeighboursNotInTree() => GetNeighbours().Where(s => !s.inTree).ToList();

    public List<Square> GetViableNeighbours() => GetNeighboursNotInTree().Where(s => s.height <= this.height + 1).ToList();

    public string DirectionString()
    {
        if (end)
            return ToString();
        if (wentTo == null)
            return " ";
        if (wentTo == neighbours[Ind.LEFT])
            return "<";
        if (wentTo == neighbours[Ind.UP])
            return "^";
        if (wentTo == neighbours[Ind.RIGHT])
            return ">";
        if (wentTo == neighbours[Ind.DOWN])
            return "V";
        throw new Exception("bruh?");
    }

    public override string ToString()
    {
        if (start)
            return "█";
        if (end)
            return "■";
        return height.ToString();
    }
}
