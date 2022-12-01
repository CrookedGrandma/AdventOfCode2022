namespace AdventOfCode2022;
public class _1 : Base {
    protected override void Action() {
        List<int> cals = new();
        int localSum = 0;
        foreach (string line in InputLines) {
            if (string.IsNullOrWhiteSpace(line)) {
                cals.Add(localSum);
                localSum = 0;
            } else localSum += int.Parse(line);
        }
        WriteLine(cals.Max());

        B();

        cals.Sort((a, b) => b.CompareTo(a));

        localSum = 0;
        for (int i = 0; i < 3; i++) {
            WriteLine(cals[i]);
            localSum += cals[i];
        }
        WriteLine();
        WriteLine(localSum);
    }
}
