using System.Collections;

namespace AdventOfCode2022;
public class _17 : Base
{

    private const int NBLOCKS = 2022;

    protected override void Action()
    {
        UseExample();
        List<Pos> edge = new();
        List<Func<Block>> blocks = BlockTypes();
        int nBlockTypes = blocks.Count;
        List<int> wind = InputLines[0].Select(JetDX).ToList();
        int nWind = wind.Count;

        int j = 0;
        Pos topRock = new(0, -1);
        for (int i = 0; i < NBLOCKS; i++)
        {
            if (i % 100 == 0)
                Console.WriteLine($"Simulating block {i + 1} / {NBLOCKS}");
            Block blockType = blocks[i % nBlockTypes]();
            List<Pos> left = blockType.left, right = blockType.right, bottom = blockType.bottom;
            var block = blockType.Select(r => r.Add(2, topRock.y + 4)).ToList();
            bool falling = true;
            while (falling)
            {
                // Jets
                int jetDX = wind[j];
                var checkJet = jetDX == -1 ? left.Select(r => r.Left()) : right.Select(r => r.Right());
                if (!checkJet.Any(p => p.x < 0 || p.x > 6 || RockExistsAt(p, edge)))
                    foreach (Pos p in block)
                        p.Add(jetDX, 0);
                j = (j + 1) % nWind;

                // Gravity
                var checkGrav = bottom.Select(r => r.Down());
                if (!checkGrav.Any(p => p.y < 0 || RockExistsAt(p, edge)))
                    foreach (Pos p in block)
                        p.Add(0, -1);
                else
                    falling = false;
            }
            foreach (Pos p in block)
                edge.Add(p);
            //edge = Edge(edge);
            topRock = edge.MaxBy(r => r.y)!;
        }
        PrintEdge(edge);

        int height = edge.Max(r => r.y) + 1;
        WriteLine(height);

        B();


    }

    private List<Pos> Edge(List<Pos> rocks)
    {
        List<Pos> edge = new();
        Pos start = rocks.Where(r => r.y == 0).MinBy(r => r.x)!;
        edge.Add(start);
        Pos from = start, current = start.LeftDown();
        for (int i = 0; i < 8; i++)
        {
            Pos next = NextPos(from, current);
            if (next.y < 0)
                return edge;
            if (next.x >= 0 && next.x <= 6 && RockExistsAt(next, rocks))
            {
                edge.Add(next);
                current = from;
                from = next;
                i = -1;
            }
            else
                current = next;
        }
        return edge;
    }

    private bool RockExistsAt(Pos pos, List<Pos> rocks)
        => rocks.Any(r => r == pos);

    private Pos NextPos(Pos from, Pos current)
    {
        Pos dif = current - from;
        int dx = 0, dy = 0;
        if (dif.y > 0 && dif.x <= 0)
            dx = 1;
        if (dif.y < 0 && dif.x >= 0)
            dx = -1;
        if (dif.x < 0 && dif.y <= 0)
            dy = 1;
        if (dif.x > 0 && dif.y >= 0)
            dy = -1;
        return new(from.x + dif.x + dx, from.y + dif.y + dy);    
    }

    private void PrintEdge(IEnumerable<Pos> rocks)
    {
        char[,] grid = new char[9, rocks.Max(r => r.y) + 2];
        for (int y = 1; y < grid.GetLength(1); y++)
        {
            grid[0, y] = '|';
            grid[8, y] = '|';
        }
        for (int x = 1; x < 8; x++)
            grid[x, 0] = '-';
        grid[0, 0] = '+';
        grid[8, 0] = '+';
        foreach (Pos rock in rocks)
            grid[rock.x + 1, rock.y + 1] = '#';
        for (int y = grid.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < 9; x++)
                Write(grid[x, y] == '\0' ? '.' : grid[x, y]);
            WriteLine();
        }
        WriteLine();
    }

    private int JetDX(char c) => c switch
    {
        '>' => 1,
        '<' => -1,
        _ => throw new Exception("seek professional help")
    };

    private List<Func<Block>> BlockTypes() => new()
    {
        () => new(new()
        {
            new(),
            new(1, 0),
            new(2, 0),
            new(3, 0),
        }),
        () => new(new()
        {
            new(1, 0),
            new(0, 1),
            new(1, 1),
            new(2, 1),
            new(1, 2),
        }),
        () => new(new()
        {
            new(0, 0),
            new(1, 0),
            new(2, 0),
            new(2, 1),
            new(2, 2),
        }),
        () => new(new()
        {
            new(),
            new(0, 1),
            new(0, 2),
            new(0, 3),
        }),
        () => new(new()
        {
            new(),
            new(1, 0),
            new(0, 1),
            new(1, 1),
        }),
    };
}

public class Block : IEnumerable<Pos>
{
    public List<Pos> rocks;
    public List<Pos> left = new(), right = new(), bottom = new();

    public Block(List<Pos> relativeRocks)
    {
        rocks = relativeRocks;
        var xs = rocks.Select(r => r.x).Distinct().OrderBy(x => x);
        var ys = rocks.Select(r => r.y).Distinct().OrderBy(y => y);
        foreach (int y in ys)
        {
            left.Add(rocks.Where(r => r.y == y).MinBy(r => r.x));
            right.Add(rocks.Where(r => r.y == y).MaxBy(r => r.x));
        }
        foreach (int x in xs)
            bottom.Add(rocks.Where(r => r.x == x).MinBy(r => r.y));

    }

    public Pos this[int i] => rocks[i];

    IEnumerator<Pos> IEnumerable<Pos>.GetEnumerator() => rocks.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => rocks.GetEnumerator();
}
