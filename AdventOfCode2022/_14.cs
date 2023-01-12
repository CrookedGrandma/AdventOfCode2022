using System.Text.RegularExpressions;

namespace AdventOfCode2022;
public class _14 : Base
{
    private int minX = int.MaxValue, maxX = 0, maxY = 0;
    private int width, height;
    private Tile14[,] grid;

    private Dictionary<int, Tile14[]> grid2 = new();

    protected override void Action()
    {
        //UseExample();

        List<List<(int, int)>> parsedLines = new();
        foreach (string line in InputLines)
        {
            var parsed = ParseLine(line);
            parsedLines.Add(parsed);
            foreach ((int x, int y) in parsed)
            {
                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
            }
        }
        width = maxX - minX + 1;
        height = maxY + 1;

        grid = new Tile14[width, height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                grid[x, y] = new(x + minX, y);

        foreach (List<(int, int)> line in parsedLines)
        {
            for (int i = 0; i < line.Count - 1; i++)
            {
                (int fx, int fy) = line[i];
                (int tx, int ty) = line[i + 1];
                Tile14 from = grid[fx - minX, fy];
                Tile14 to = grid[tx - minX, ty];
                List<Tile14> range = SelectFromTo(from, to);
                foreach (Tile14 t in range)
                    t.SetRock();
            }
        }

        int count = 0;
        while (true)
        {
            var pos = GetFinalPosition(500 - minX, 0);
            if (pos == null)
                break;
            grid[pos.a, pos.b].SetSand();
            count++;
        }

        WriteLine(count);
        WriteLine();

        PrintGrid();

        B();

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                DefinitelyAdd(x, new(x + minX, y));

        foreach (List<(int, int)> line in parsedLines)
        {
            for (int i = 0; i < line.Count - 1; i++)
            {
                (int fx, int fy) = line[i];
                (int tx, int ty) = line[i + 1];
                Tile14 from = grid2[fx - minX][fy];
                Tile14 to = grid2[tx - minX][ty];
                List<Tile14> range = SelectFromTo2(from, to);
                foreach (Tile14 t in range)
                    t.SetRock();
            }
        }

        int count2 = 0;
        while (true)
        {
            var pos = GetFinalPosition2(500 - minX, 0);
            if (pos.a == 500 - minX && pos.b == 0)
                break;
            DefinitelyAdd(pos.a, new Tile14(pos.a + minX, pos.b, TileContent14.Sand));
            count2++;
        }

        WriteLine(count2 + 1);
        WriteLine();

        PrintGrid2();
    }

    public void DefinitelyAdd(int x, Tile14 t)
    {
        if (!grid2.ContainsKey(x))
            grid2.Add(x, new Tile14[maxY + 2]);
        grid2[x][t.y] = t;
    }

    private Tup<int, int>? GetFinalPosition(int x, int y)
    {
        if (y > maxY) return null;
        if (x < 0) return null;
        if (x >= width) return null;
        if (y + 1 > maxY) return null;
        if (grid[x, y + 1].Open)
            return GetFinalPosition(x, y + 1);
        if (x - 1 < 0) return null;
        if (grid[x - 1, y + 1].Open)
            return GetFinalPosition(x - 1, y + 1);
        if (x + 1 >= width) return null;
        if (grid[x + 1, y + 1].Open)
            return GetFinalPosition(x + 1, y + 1);
        return new Tup<int, int>(x, y);
    }

    private Tup<int, int> GetFinalPosition2(int x, int y)
    {
        if (y > maxY + 2 || y + 1 > maxY + 2) throw new Exception("under floor");
        if (Open2(x, y + 1))
            return GetFinalPosition2(x, y + 1);
        if (Open2(x - 1, y + 1))
            return GetFinalPosition2(x - 1, y + 1);
        if (Open2(x + 1, y + 1))
            return GetFinalPosition2(x + 1, y + 1);
        return new Tup<int, int>(x, y);
    }

    private bool Open2(int x, int y)
    {
        if (y == maxY + 2) return false;
        if (!grid2.ContainsKey(x)) return true;
        Tile14? tile = grid2[x][y];
        if (tile == null) return true;
        return tile.Open;
    }

    private List<Tile14> SelectFromTo(Tile14 from, Tile14 to)
    {
        if (from.x == to.x)
            return Extensions.RangeIterator(from.y, to.y, Math.Sign(to.y - from.y)).Select(y => grid[from.x - minX, y]).ToList();
        else
            return Extensions.RangeIterator(from.x, to.x, Math.Sign(to.x - from.x)).Select(x => grid[x - minX, from.y]).ToList();
    }

    private List<Tile14> SelectFromTo2(Tile14 from, Tile14 to)
    {
        if (from.x == to.x)
            return Extensions.RangeIterator(from.y, to.y, Math.Sign(to.y - from.y)).Select(y => grid2[from.x - minX][y]).ToList();
        else
            return Extensions.RangeIterator(from.x, to.x, Math.Sign(to.x - from.x)).Select(x => grid2[x - minX][from.y]).ToList();
    }

    private void PrintGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
                Write(grid[x, y].Char);
            WriteLine();
        }
    }

    private void PrintGrid2()
    {
        for (int y = 0; y < maxY + 2; y++)
        {
            var sort = grid2.Keys.ToList().OrderBy(x => x).ToList();
            int prev = sort.First() - 1;
            foreach (int key in sort)
            {
                if (key - prev > 1)
                    foreach (int x in Extensions.RangeIterator(prev + 1, key - 1))
                        Write('.');
                Tile14 t = grid2[key][y];
                Write(t == null ? '.' : t.Char);
                prev = key;
            }
            WriteLine();
        }
    }

    private List<(int, int)> ParseLine(string line)
        => Regex.Split(line, " -> ").Select(x => x.Split(',')).Select(x => (int.Parse(x[0]), int.Parse(x[1]))).ToList();
}

public class Tile14
{
    public int x;
    public int y;

    private TileContent14 content;

    public Tile14(int x, int y, TileContent14 content = TileContent14.Open)
    {
        this.x = x;
        this.y = y;
        this.content = content;
    }

    public bool Open => content == TileContent14.Open;

    public void SetSand() => content = TileContent14.Sand;

    public void SetRock() => content = TileContent14.Rock;

    public override string ToString()
        => $"[{x}, {y}] ({content})";

    public char Char => (x == 500 && y == 0) ? '+' : content switch
    {
        TileContent14.Open => '.',
        TileContent14.Rock => '#',
        TileContent14.Sand => 'o',
        _ => throw new Exception("lmao"),
    };
}

public enum TileContent14
{
    Open,
    Rock,
    Sand,
}
