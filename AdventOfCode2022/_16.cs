//using System.Runtime.CompilerServices;

namespace AdventOfCode2022;
public class _16 : Base
{
    protected override void Action()
    {
        //UseExample();
        Dictionary<string, Valve> valves = new();
        foreach (string line in InputLines)
        {
            string[] split = line.Split(' ');
            string name = split[1];
            int flow = int.Parse(split[4].Split('=')[1].Split(';')[0]);
            var ns = split[9..].Select(n => n[..2]);
            valves.Add(name, new(name, flow, ns));
        }

        State start = new State(valves);
        EditableStack<State> stack = new();
        stack.Push(start);

        int max = -1;
        State? currentState = null;
        uint round = 0;
        while (stack.Count > 0)
        {
            round++;
            if (round % 10000 == 0)
                Console.WriteLine($"Round {round} - stack size: {stack.Count}");
            currentState = stack.Pop();
            if (currentState.TotalFlow > max)
                max = currentState.TotalFlow;
            foreach (State n in currentState.GetNeighbourStates())
                stack.Push(n);
            //List<int> toRemove = new();
            //for (int i = 0; i < stack.Count; i++)
            //{
            //    for (int j = i + 1; j < stack.Count; j++)
            //    {
            //        if (stack[i].CurrentValve == stack[j].CurrentValve)
            //        {
            //            State a = stack[i];
            //            State b = stack[j];
            //            if (InconclusiveFlow(a, b))
            //                continue;
            //            if (ABetterThanB(a, b))
            //                toRemove.Add(j);
            //            else if (ABetterThanB(b, a))
            //                toRemove.Add(i);
            //        }
            //    }
            //}
            //toRemove.Sort((a, b) => b - a);
            //foreach (int i in toRemove)
            //    stack.Remove(i);
        }
        currentState!.WriteHistory(WriteLine);

        WriteLine(max);

        B();


    }

    //public bool InconclusiveFlow(State a, State b) =>
    //    (a.TotalFlow > b.TotalFlow && a.FlowRate < b.FlowRate) ||
    //    (a.TotalFlow < b.TotalFlow && a.FlowRate > b.FlowRate);
    //public bool EqualFlow(State a, State b) => a.TotalFlow == b.TotalFlow && a.FlowRate == b.FlowRate;
    //public bool ABetterFlowThanB(State a, State b) => (a.TotalFlow > b.TotalFlow || a.FlowRate > b.FlowRate);
    //public bool EqualMinutes(State a, State b) => a.Minutes == b.Minutes;
    //public bool ABetterMinutesThanB(State a, State b) => a.Minutes < b.Minutes;
    //public bool ABetterThanB(State a, State b) =>
    //    (ABetterMinutesThanB(a, b) && (EqualFlow(a, b) || ABetterFlowThanB(a, b))) ||
    //    (ABetterFlowThanB(a, b) && (EqualMinutes(a, b) || ABetterMinutesThanB(a, b)));
}

public class State
{
    public const int MAX_MINUTES = 30;
    public int Minutes { get; private set; }
    public int TotalFlow { get; private set; }
    public int FlowRate { get; private set; }
    public Dictionary<string, bool> Opened { get; private set; }
    public Valve CurrentValve { get; private set; }

    private Dictionary<string, Valve> valves;
    private List<HistEntry> history;

    public State(Dictionary<string, Valve> valves,
        int? minutes = null,
        int? totalFlow = null,
        int? flowRate = null,
        Dictionary<string, bool>? opened = null,
        Valve? currentValve = null,
        List<HistEntry>? history = null)
    {
        Minutes = minutes ?? 0;
        TotalFlow = totalFlow ?? 0;
        FlowRate = flowRate ?? 0;
        if (opened == null)
        {
            Opened = new();
            foreach (var key in valves.Keys)
                Opened.Add(key, false);
        }
        else
            Opened = opened;
        this.valves = valves;
        this.CurrentValve = currentValve ?? valves["AA"];
        this.history = history ?? new();
    }

    public List<State> GetNeighbourStates()
    {
        List<State> neighbours = new();
        if (Minutes >= MAX_MINUTES)
            return neighbours;
        if (Minutes == MAX_MINUTES - 1 || Opened.Values.All(o => o))
        {
            neighbours.Add(this.DoNothing());
            return neighbours;
        }
        var neighValves = CurrentValve.GetNeighbours(valves);
        foreach (Valve n in neighValves)
        {
            State ns = this.WalkTo(n);
            if (!history.Any(h => h.EqualOrBetter(n, ns.TotalFlow, ns.FlowRate)))
                neighbours.Add(ns);
        }
        if (!Opened[CurrentValve.Name] && CurrentValve.Flow > 0)
            neighbours.Add(this.Open());
        return neighbours;
    }

    public State DoNothing()
    {
        List<HistEntry> history = new(this.history);
        history.Add(new(CurrentValve, TotalFlow, FlowRate));
        return new State(valves, Minutes + 1, TotalFlow + FlowRate, FlowRate, Opened, CurrentValve, history);
    }

    public State Open()
    {
        Dictionary<string, bool> opened = Opened.ToDictionary(e => e.Key, e => e.Value);
        opened[CurrentValve.Name] = true;
        List<HistEntry> history = new(this.history);
        history.Add(new(CurrentValve, TotalFlow, FlowRate));
        return new State(valves, Minutes + 1, TotalFlow + FlowRate, FlowRate + CurrentValve.Flow, opened, CurrentValve, history);
    }

    public State WalkTo(Valve valve)
    {
        Dictionary<string, bool> opened = Opened.ToDictionary(e => e.Key, e => e.Value);
        List<HistEntry> history = new(this.history);
        history.Add(new(CurrentValve, TotalFlow, FlowRate));
        return new State(valves, Minutes + 1, TotalFlow + FlowRate, FlowRate, opened, valve, history);
    }

    public void WriteHistory(Action<string> WriteLine)
    {
        foreach (var h in history) WriteLine(h.ToString());
    }

    public override string ToString()
        => $"{CurrentValve.Name} - {Minutes}m - {TotalFlow}f - {FlowRate}f/m";
}

public class HistEntry
{
    public Valve Valve { get; private set; }
    public int TotalFlow { get; private set; }
    public int FlowRate { get; private set; }

    public HistEntry(Valve valve, int totalFlow, int flowRate)
    {
        Valve = valve;
        TotalFlow = totalFlow;
        FlowRate = flowRate;
    }

    public bool EqualOrBetter(Valve valve, int totalFlow, int flowRate)
        => Valve == valve && FlowRate >= flowRate;

    public override string ToString()
        => $"{Valve.Name} - {TotalFlow}f - {FlowRate}f/m";

}

public class Valve
{
    public string Name { get; private set; }
    public int Flow { get; private set; }

    private IEnumerable<string> neighbours;

    public Valve(string name, int flow, IEnumerable<string> neighbours)
    {
        Name = name;
        Flow = flow;
        this.neighbours = neighbours;
    }

    public IEnumerable<Valve> GetNeighbours(Dictionary<string, Valve> dict)
        => neighbours.Select(n => dict[n]).OrderByDescending(v => v.Flow);

    public override string ToString() => this.Name;
}
