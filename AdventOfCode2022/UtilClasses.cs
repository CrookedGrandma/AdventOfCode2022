namespace AdventOfCode2022;

public static class Ind
{
    public const int LEFT = 0;
    public const int UP = 1;
    public const int RIGHT = 2;
    public const int DOWN = 3;
}

public class Tup<T1, T2>
{
    public T1 a;
    public T2 b;

    public Tup(T1 a, T2 b)
    {
        this.a = a;
        this.b = b;
    }
}

public class Pos
{
    public int x;
    public int y;

    public Pos() : this(0, 0) { }
    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Pos LeftDown() => new(x - 1, y - 1);
    public Pos Left() => new(x - 1, y);
    public Pos LeftUp() => new(x - 1, y + 1);
    public Pos Up() => new(x, y + 1);
    public Pos RightUp() => new(x + 1, y + 1);
    public Pos Right() => new(x + 1, y);
    public Pos RightDown() => new(x + 1, y - 1);
    public Pos Down() => new(x, y - 1);

    public Pos Add(int dx, int dy)
    {
        x += dx;
        y += dy;
        return this;
    }

    public override string ToString() => $"({x}, {y})";

    public static bool operator ==(Pos a, Pos b) => a.x == b.x && a.y == b.y;
    public static bool operator !=(Pos a, Pos b) => !(a == b);

    public static Pos operator +(Pos a, Pos b) => new(a.x + b.x, a.y + b.y);
    public static Pos operator -(Pos a, Pos b) => new(a.x - b.x, a.y - b.y);

    public override bool Equals(object? obj) => this == (Pos)obj!;

    public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode();
}
