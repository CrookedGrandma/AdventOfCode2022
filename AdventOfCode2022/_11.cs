using System.Text.RegularExpressions;

namespace AdventOfCode2022;
public class _11 : Base {
    protected override void Action() {
        //UseExample();
        List<Monkey> monkeys = new();
        for (int i = 0; i < InputLines.Count; i += 7) {
            List<ulong> items = Regex.Split(InputLines[i + 1][18..], @", ").Select(s => ulong.Parse(s)).ToList();
            string operation = InputLines[i + 2][19..];
            ulong div = ulong.Parse(InputLines[i + 3][21..]);
            int mT = int.Parse(InputLines[i + 4][29..]);
            int mF = int.Parse(InputLines[i + 5][30..]);
            monkeys.Add(new(items, operation, div, mT, mF));
        }

        ulong modulo = monkeys.Select(m => m.divisibility).Aggregate((a, b) => a * b);
        for (int r = 0; r < 10000; r++) {
            foreach (Monkey m in monkeys) {
                m.ThrowItems(monkeys, modulo);
            }
        }

        var inspections = monkeys.Select(m => m.inspections).ToList();
        var sorted = monkeys.OrderByDescending(x => x.inspections).ToList();
        //int answer = sorted.Take(2).Select(x => x.inspections).Aggregate((a, b) => a * b);
        long answer = (long)sorted[0].inspections * (long)sorted[1].inspections;
        WriteLine(answer);

        B();


    }
}

public class Monkey {
    public long inspections = 0;
    public Queue<ulong> items = new();
    public Func<ulong, ulong> operation;

    public ulong divisibility;

    private int monkeTrue, monkeFalse;

    private bool squared;
    private char op;
    private ulong val;
    
    public Monkey(IEnumerable<ulong> items, string operation, ulong div, int mT, int mF) {
        foreach (ulong item in items)
            this.items.Enqueue(item);

        /*if (operation == "old * old")
            this.operation = x => x * x;
        else {
            char op = operation[4];
            ulong v = ulong.Parse(operation[6..]);
            this.operation = op switch {
                '+' => (x => x + v),
                '*' => (x => x * v),
                _ => throw new Exception("seek therapy")
            };
        }*/
        this.operation = x => 0; // Purely so that VS doesn't complain

        this.squared = operation == "old * old";
        this.op = operation[4];
        this.val = squared ? 0 : ulong.Parse(operation[6..]);

        this.divisibility = div;
        this.monkeTrue = mT;
        this.monkeFalse = mF;
    }

    private ulong Operation(ulong old) {
        if (squared)
            return old * old;
        if (op == '*')
            return old * this.val;
        if (op == '+')
            return old + this.val;
        throw new Exception("give me strength");
    }

    public void ThrowItems(List<Monkey> monkeys, ulong modulo) {
        while (items.Count > 0) {
            ThrowFirstItem2(monkeys, modulo);
        }
    }

    private void ThrowFirstItem(List<Monkey> monkeys) {
        ulong item = items.Dequeue();
        ulong worry = operation(item) / 3;
        this.inspections++;
        if (worry % divisibility == 0)
            monkeys[monkeTrue].items.Enqueue(worry);
        else
            monkeys[monkeFalse].items.Enqueue(worry);
    }

    private void ThrowFirstItem2(List<Monkey> monkeys, ulong modulo) {
        ulong item = items.Dequeue();
        ulong worry = Operation(item) % modulo;
        this.inspections++;
        if (worry % divisibility == 0)
            monkeys[monkeTrue].items.Enqueue(worry);
        else
            monkeys[monkeFalse].items.Enqueue(worry);
    }
}
