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

public struct Pos
{
    public int x;
    public int y;

    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Pos Right() => new Pos(x + 1, y);
    public Pos Left() => new Pos(x - 1, y);
    public Pos Up() => new Pos(x, y + 1);
    public Pos Down() => new Pos(x, y - 1);

    public override string ToString() => $"({x}, {y})";

    public static bool operator ==(Pos a, Pos b) => a.x == b.x && a.y == b.y;

    public static bool operator !=(Pos a, Pos b) => !(a == b);

    public override bool Equals(object? obj) => this == (Pos)obj!;

    public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode();
}

public class EditableStack<T>
{
    private readonly List<T> items = new();

    public T this[int index] => items[index];

    public int Count => items.Count;

    public void Push(T item) => items.Add(item);

    public T Pop()
    {
        if (items.Count > 0)
        {
            T temp = items[Count - 1];
            items.RemoveAt(Count - 1);
            return temp;
        }
        throw new IndexOutOfRangeException("EditableStack contains no more elements");
    }

    public void Remove(int itemAtPosition) => items.RemoveAt(itemAtPosition);
}
