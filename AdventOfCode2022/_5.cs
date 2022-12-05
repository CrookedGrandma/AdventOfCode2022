namespace AdventOfCode2022;
public class _5 : Base {
    protected override void Action() {
        //UseExample();
        Stack<char>[] stacks = new Stack<char>[(InputLines[0].Length + 1) / 4];
        int breakline = 0;
        while (!string.IsNullOrWhiteSpace(InputLines[breakline]))
            breakline++;
        for (int i = breakline - 2; i >= 0; i--) {
            for (int s = 0; s < stacks.Length; s++) {
                if (stacks[s] == null)
                    stacks[s] = new();
                char c = InputLines[i][s * 4 + 1];
                if (c != ' ')
                    stacks[s].Push(c);
            }
        }

        //foreach (string line in InputLines.ToArray()[(breakline + 1)..]) {
        //    string[] split = line.Split(' ');
        //    int num = int.Parse(split[1]);
        //    int from = int.Parse(split[3]) - 1;
        //    int to = int.Parse(split[5]) - 1;
        //    for (int i = 0; i < num; i++) {
        //        char c = stacks[from].Pop();
        //        stacks[to].Push(c);
        //    }
        //}

        //string tops = "";
        //foreach (Stack<char> s in stacks) {
        //    tops += s.Peek();
        //}

        //WriteLine(tops);

        B();

        foreach (string line in InputLines.ToArray()[(breakline + 1)..]) {
            string[] split = line.Split(' ');
            int num = int.Parse(split[1]);
            int from = int.Parse(split[3]) - 1;
            int to = int.Parse(split[5]) - 1;
            Stack<char> q = new();
            for (int i = 0; i < num; i++) {
                q.Push(stacks[from].Pop());
            }
            for (int i = 0; i < num; i++) {
                char c = q.Pop();
                stacks[to].Push(c);
            }
        }

        string tops = "";
        foreach (Stack<char> s in stacks) {
            tops += s.Peek();
        }

        WriteLine(tops);

    }
}
