using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2022;
public class _3 : Base {
    protected override void Action() {
        //UseExample();
        List<(Dictionary<char, int>, Dictionary<char, int>)> sacks = new();
        foreach (string line in InputLines) {
            int half = line.Length / 2;
            string first = line[..half];
            string second = line[half..];
            sacks.Add((new(), new()));
            int i = sacks.Count - 1;
            foreach (char c in first) {
                if (sacks[i].Item1.ContainsKey(c))
                    sacks[i].Item1[c]++;
                else
                    sacks[i].Item1.Add(c, 1);
            }
            foreach (char c in second) {
                if (sacks[i].Item2.ContainsKey(c))
                    sacks[i].Item2[c]++;
                else
                    sacks[i].Item2.Add(c, 1);
            }
        }

        int prioSum = 0;
        foreach (var sack in sacks) {
            foreach (char key in sack.Item1.Keys) {
                if (sack.Item2.ContainsKey(key)) {
                    prioSum += Priority(key);
                    //WriteLine($"{key}: {Priority(key)}");
                    break;
                }
            }
        }
        WriteLine(prioSum);

        B();

        prioSum = 0;
        for (int i = 0; i < sacks.Count; i += 3) {
            List<List<char>> group = sacks.GetRange(i, 3).Select(tup => tup.Item1.Keys.Concat(tup.Item2.Keys).ToList()).ToList();
            foreach (char c in group[0]) {
                if (group[1].Contains(c) && group[2].Contains(c)) {
                    prioSum += Priority(c);
                }
            }
        }

        WriteLine(prioSum);
    }

    private int Priority(char c) {
        return (int)c switch {
            > 96 => c - 96,
            _ => c - 64 + 26
        };
    }
}
