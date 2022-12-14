namespace AdventOfCode2022;

public static class Extensions
{
    public static T[] GetRow<T>(this T[,] matrix, int y)
        => matrix.GetRangeX(y, 0, matrix.GetLength(0));

    public static T[] GetCol<T>(this T[,] matrix, int x)
        => matrix.GetRangeY(x, 0, matrix.GetLength(1));

    public static T[] GetRangeX<T>(this T[,] matrix, int y, int xstart, int xcount)
        => Enumerable.Range(xstart, xcount).Select(x => matrix[x, y]).ToArray();

    public static T[] GetRangeY<T>(this T[,] matrix, int x, int ystart, int ycount)
        => Enumerable.Range(ystart, ycount).Select(y => matrix[x, y]).ToArray();

    /// <summary>
    /// Range from <paramref name="start"/> (inclusive) to <paramref name="stop"/> (inclusive)
    /// </summary>
    public static IEnumerable<int> RangeIterator(int start, int stop, int step = 1)
    {
        int x = start;

        do
        {
            yield return x;
            x += step;
            if (step < 0 && x < stop || 0 < step && stop < x)
                break;
        }
        while (true);
    }
}
