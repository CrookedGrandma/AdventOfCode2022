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
                if (d % 1000 == 0)
                    Console.WriteLine($"Sensor {i + 1} - distance {d} / {remainder}");
                int x1 = sensor.x - minX + d;
                int x2 = sensor.x - minX - d;
                y10[x1].SetContentIfUnknown(TileContent15.NoBeacon);
                y10[x2].SetContentIfUnknown(TileContent15.NoBeacon);
            }
        }

        //foreach (var t in y10)
        //    Write(Math.Abs(t.x % 10));
        //WriteLine();
        //WriteLine(y10.RowString());
        int nobeacon = y10.Where(t => t.KnownNoBeacon).Count();
        WriteLine(nobeacon);

        B();


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
