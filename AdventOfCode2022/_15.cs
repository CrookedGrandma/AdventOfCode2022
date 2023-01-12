namespace AdventOfCode2022;
public class _15 : Base
{
    private int minX = int.MaxValue, maxX = 0, minY = int.MaxValue, maxY = 0;
    private int width, height;

    protected override void Action()
    {
        //UseExample();
        //const int YROW = 10;
        const int YROW = 2_000_000;
        //const int MAX = 20;
        const int MAX = 4_000_000;
        List<Tile15> sensors = new();
        List<Tile15> beacons = new();
        foreach (string line in InputLines)
        {
            (Tile15 sensor, Tile15 beacon) = ParseLine(line);
            sensors.Add(sensor);
            beacons.Add(beacon);
            sensor.SetDistance(beacon);
            if (sensor.x - sensor.Range < minX) minX = sensor.x - sensor.Range;
            if (sensor.x + sensor.Range > maxX) maxX = sensor.x + sensor.Range;
            if (sensor.y - sensor.Range < minY) minY = sensor.y - sensor.Range;
            if (sensor.y + sensor.Range > maxY) maxY = sensor.y + sensor.Range;
            if (beacon.x - sensor.Range < minX) minX = beacon.x - sensor.Range;
            if (beacon.x + sensor.Range > maxX) maxX = beacon.x + sensor.Range;
            if (beacon.y - sensor.Range < minY) minY = beacon.y - sensor.Range;
            if (beacon.y + sensor.Range > maxY) maxY = beacon.y + sensor.Range;
        }
        width = maxX - minX + 1;
        height = maxY - minY + 1;

        Tile15[] y10 = new Tile15[width];
        for (int x = 0; x < y10.Length; x++)
            y10[x] = new Tile15(x + minX, YROW);
        for (int i = 0; i < sensors.Count; i++)
        {
            Console.WriteLine($"Using sensor {i + 1} of {sensors.Count}");
            var sensor = sensors[i];
            var beacon = beacons[i];
            if (sensor.y == YROW)
                y10[sensor.x - minX] = sensor;
            if (beacon.y == YROW)
                y10[beacon.x - minX] = beacon;
            int ydiff = Math.Abs(sensor.y - YROW);
            if (ydiff > sensor.Range)
                continue;
            int remainder = sensor.Range - ydiff;
            for (int d = 0; d <= remainder; d++)
            {
                int x1 = sensor.x - minX + d;
                int x2 = sensor.x - minX - d;
                y10[x1].SetContentIfUnknown(TileContent15.NoBeacon);
                y10[x2].SetContentIfUnknown(TileContent15.NoBeacon);
            }
        }

        int nobeacon = y10.Where(t => t.KnownNoBeacon).Count();
        WriteLine(nobeacon);

        B();

        int fx = -1, fy = -1;
        RangeMerger rm = new();
        for (int y = 0; y < MAX; y++)
        {
            if (y % 100000 == 0) Console.WriteLine($"Y: {y}");
            for (int i = 0; i < sensors.Count; i++)
            {
                var sensor = sensors[i];
                var beacon = beacons[i];
                if (beacon.y == y)
                    rm.AddRange(new(beacon.x, beacon.x));
                int ydiff = Math.Abs(sensor.y - y);
                if (ydiff > sensor.Range)
                    continue;
                int remainder = sensor.Range - ydiff;
                int left = sensor.x - remainder; // inclusive
                int right = sensor.x + remainder; // inclusive
                rm.AddRange(new(left, right));
            }
            int falseind = rm.MissingPositionBetween(0, MAX);
            if (falseind >= 0)
            {
                fx = falseind;
                fy = y;
                break;
            }
            rm.Reset();
        }

        Console.WriteLine("Answer found!");
        WriteLine($"x: {fx}");
        WriteLine($"y: {fy}");
        long answer = (long)fx * 4_000_000 + (long)fy;
        WriteLine(answer);
    }

    public (Tile15, Tile15) ParseLine(string line)
    {
        string[] split = line.Split(' ');
        int sx = int.Parse(split[2].Split('=')[1].Split(',')[0]);
        int sy = int.Parse(split[3].Split('=')[1].Split(':')[0]);
        int bx = int.Parse(split[8].Split('=')[1].Split(',')[0]);
        int by = int.Parse(split[9].Split('=')[1]);
        Tile15 sensor = new(sx, sy, TileContent15.Sensor);
        Tile15 beacon = new(bx, by, TileContent15.Beacon);
        return (sensor, beacon);
    }
}

public class RangeMerger
{
    private List<Tup<int, int>> ranges;
    public RangeMerger()
    {
        ranges = new();
    }

    public void AddRange(Tup<int, int> inclusiveRange) => ranges.Add(inclusiveRange);

    public int MissingPositionBetween(int min, int max)
    {
        if (ranges.Count == 0)
            throw new Exception("no range set uwu");
        ranges.Sort((a, b) =>
        {
            if (a.a == b.a)
                return a.b - b.b;
            return a.a - b.a;
        });
        if (ranges[0].a > min)
            return min;
        int right = ranges[0].b;
        for (int i = 1; i < ranges.Count; i++)
        {
            if (right < ranges[i].a)
                return right + 1;
            if (right < ranges[i].b)
                right = ranges[i].b;
        }
        return right >= max ? -1 : right + 1;
    }

    public void Reset() => ranges.Clear();
}

public static class Extensions15
{
    public static string RowString(this Tile15[] arr) => arr.Select(t => t.ToString()).Aggregate((a, b) => a + b);
}

public class Tile15
{
    public int x, y;
    public TileContent15 content;
    private int? distanceToBeacon;

    public int Range => distanceToBeacon ?? 0;

    public bool Known => content != TileContent15.Unknown;

    public bool KnownNoBeacon => Known && content != TileContent15.Beacon;

    public Tile15(int x, int y, TileContent15 content = TileContent15.Unknown)
    {
        this.x = x;
        this.y = y;
        this.content = content;
    }

    public void SetContentIfUnknown(TileContent15 c)
    {
        if (!Known)
            content = c;
    }

    public int GetDistanceTo(Tile15 other) => Math.Abs(other.x - x) + Math.Abs(other.y - y);

    public void SetDistance(int dist) => distanceToBeacon = dist;
    public void SetDistance(Tile15 other) => SetDistance(GetDistanceTo(other));

    public override string ToString() => content switch
    {
        TileContent15.Unknown => ".",
        TileContent15.NoBeacon => "#",
        TileContent15.Beacon => "B",
        _ => "S"
    };
}

public enum TileContent15
{
    Unknown,
    NoBeacon,
    Beacon,
    Sensor,
}
