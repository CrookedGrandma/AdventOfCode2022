namespace AdventOfCode2022;
public class _4 : Base
{
    protected override void Action()
    {
        //UseExample();
        List<Tup<Ran, Ran>> pairs = new();
        foreach (string line in InputLines)
        {
            string[] elves = line.Split(',');
            Ran e1 = new Ran(elves[0]);
            Ran e2 = new Ran(elves[1]);
            pairs.Add(new(e1, e2));
        }

        int contained = 0;
        foreach (var pair in pairs)
        {
            if ((pair.a.start <= pair.b.start && pair.a.end >= pair.b.end) ||
                (pair.b.start <= pair.a.start && pair.b.end >= pair.a.end))
            {
                contained++;
            }
        }
        WriteLine(contained);

        B();

        int overlapping = 0;
        foreach (var pair in pairs)
        {
            if ((pair.a.start <= pair.b.end && pair.a.end >= pair.b.start) ||
                (pair.b.start <= pair.a.end && pair.b.end >= pair.a.start))
            {
                overlapping++;
            }
        }
        WriteLine(overlapping);
    }
}

public class Ran
{
    public int start;
    public int count;
    public int end;

    public Ran(string dashed)
    {
        string[] split = dashed.Split('-');
        int start = int.Parse(split[0]);
        int end = int.Parse(split[1]);
        this.start = start;
        this.count = end - start + 1;
        this.end = end;
    }
}
