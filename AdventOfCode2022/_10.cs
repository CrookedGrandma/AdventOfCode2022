using System.Xml.Serialization;

namespace AdventOfCode2022;
public class _10 : Base {
    protected override void Action() {
        //UseExample();
        List<int> idxs = new List<int>() { 20, 60, 100, 140, 180, 220 };
        List<int> xs = new List<int>() { 1 };
        foreach (string line in InputLines) {
            string[] split = line.Split(' ');
            if (split[0] == "addx") {
                xs.Add(xs.Last());
                xs.Add(xs.Last() + int.Parse(split[1]));
            } else {
                xs.Add(xs.Last());
            }
        }

        List<int> strenghts = idxs.Select(i => xs[i - 1] * i).ToList();
        int total = strenghts.Sum();
        WriteLine(total);

        B();

        for (int i = 0; i < xs.Count; i++) {
            int pixx = i % 40;
            if (pixx == 0)
                WriteLine();
            int pos = xs[i];
            if (pixx >= pos - 1 && pixx <= pos + 1)
                Write("#");
            else
                Write(".");
        }
    }
}
