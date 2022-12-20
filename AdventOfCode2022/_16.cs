//using System.Runtime.CompilerServices;

using static AdventOfCode2022.State2;

namespace AdventOfCode2022;
public class _16 : Base
{
    protected override void Action()
    {
        UseExample();
        Dictionary<string, Valve> valves = new();
        foreach (string line in InputLines)
        {
            string[] split = line.Split(' ');
            string name = split[1];
            int flow = int.Parse(split[4].Split('=')[1].Split(';')[0]);
            var ns = split[9..].Select(n => n[..2]);
            valves.Add(name, new(name, flow, ns));
        }

        /*State start = new State(valves);
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
        }
        currentState!.WriteHistory(WriteLine);

        WriteLine(max);*/

        B();

        State2 start2 = new State2(valves);
        EditableStack<State2> stack2 = new();
        stack2.Push(start2);

        int max = -1;
        State2? currentState2 = null;
        uint round = 0;
        while (stack2.Count > 0)
        {
            round++;
            if (round % 10000 == 0)
                Console.WriteLine($"Round {round} - stack size: {stack2.Count}");
            currentState2 = stack2.Pop();
            if (currentState2.TotalFlow > max)
                max = currentState2.TotalFlow;
            foreach (State2 n in currentState2.GetNeighbourStates2())
                stack2.Push(n);
        }
    }
}

public class State2 : State
{
    public override int MAX_MINUTES => 26;

    public Valve CurrentValveEl { get; private set; }

    public State2(Dictionary<string, Valve> valves,
        int? minutes = null,
        int? totalFlow = null,
        int? flowRate = null,
        Dictionary<string, bool>? opened = null,
        Valve? currentValve = null,
        Valve? currentValveEl = null,
        List<HistEntry>? history = null) : base(valves, minutes, totalFlow, flowRate, opened, currentValve, history)
    {
        CurrentValveEl = currentValveEl ?? valves["AA"];
    }

    public List<State2> GetNeighbourStates2()
    {
        List<State2> neighbours = new();
        if (Minutes >= MAX_MINUTES)
            return neighbours;
        if (Minutes == MAX_MINUTES - 1 || Opened.Values.All(o => o))
        {
            neighbours.Add(this.DoNothing());
            return neighbours;
        }
        var neighValvesH = CurrentValve.GetNeighbours(valves);
        var neighValvesE = CurrentValveEl.GetNeighbours(valves);
        foreach (Valve nh in neighValvesH)
        {
            foreach (Valve ne in neighValvesE)
            {
                if (ne == nh) continue;
                State2 ns = this.ElephantWalksTo(ne).WalkTo(nh);
                if (!history.Any(h => h.EqualOrBetter(nh, ns.FlowRate)))
                    neighbours.Add(ns);
                if (!Opened[CurrentValve.Name] && CurrentValve.Flow > 0)
                    neighbours.Add(this.ElephantWalksTo(ne).Open());
            }
            if (!Opened[CurrentValveEl.Name] && CurrentValveEl.Flow > 0)
                neighbours.Add(this.ElephantOpens().WalkTo(nh));
        }
        return neighbours;
    }

    public State2 ElephantOpens()
    {
        Dictionary<string, bool> opened = Opened.ToDictionary(e => e.Key, e => e.Value);
        opened[CurrentValveEl.Name] = true;
        history.Add(new(CurrentValveEl, TotalFlow, FlowRate));
        return new State2(valves, Minutes, TotalFlow, FlowRate + CurrentValve.Flow, opened, CurrentValve, CurrentValveEl, history);
    }

    public State2 ElephantWalksTo(Valve valve)
    {
        Dictionary<string, bool> opened = Opened.ToDictionary(e => e.Key, e => e.Value);
        List<HistEntry> history = new(this.history);
        history.Add(new(CurrentValve, TotalFlow, FlowRate));
        return new State2(valves, Minutes, TotalFlow, FlowRate, opened, CurrentValve, valve, history);
    }

    public override State2 DoNothing()
    {
        List<HistEntry> history = new(this.history);
        history.Add(new(CurrentValve, TotalFlow, FlowRate));
        return new State2(valves, Minutes + 1, TotalFlow + FlowRate, FlowRate, Opened, CurrentValve, CurrentValveEl, history);
    }

    public override State2 Open()
    {
        Dictionary<string, bool> opened = Opened.ToDictionary(e => e.Key, e => e.Value);
        opened[CurrentValve.Name] = true;
        List<HistEntry> history = new(this.history);
        history.Add(new(CurrentValve, TotalFlow, FlowRate));
        return new State2(valves, Minutes + 1, TotalFlow + FlowRate, FlowRate + CurrentValve.Flow, opened, CurrentValve, CurrentValveEl, history);
    }

    public override State2 WalkTo(Valve valve)
    {
        Dictionary<string, bool> opened = Opened.ToDictionary(e => e.Key, e => e.Value);
        List<HistEntry> history = new(this.history);
        history.Add(new(CurrentValve, TotalFlow, FlowRate));
        return new State2(valves, Minutes + 1, TotalFlow + FlowRate, FlowRate, opened, valve, CurrentValveEl, history);
    }
}

public class State
{
    public virtual int MAX_MINUTES => 30;
    public int Minutes { get; private set; }
    public int TotalFlow { get; private set; }
    public int FlowRate { get; private set; }
    public Dictionary<string, bool> Opened { get; private set; }
    public Valve CurrentValve { get; private set; }

    protected Dictionary<string, Valve> valves;
    protected List<HistEntry> history;

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

    public virtual List<State> GetNeighbourStates()
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
            if (!history.Any(h => h.EqualOrBetter(n, ns.FlowRate)))
                neighbours.Add(ns);
        }
        if (!Opened[CurrentValve.Name] && CurrentValve.Flow > 0)
            neighbours.Add(this.Open());
        return neighbours;
    }

    public virtual State DoNothing()
    {
        List<HistEntry> history = new(this.history);
        history.Add(new(CurrentValve, TotalFlow, FlowRate));
        return new State(valves, Minutes + 1, TotalFlow + FlowRate, FlowRate, Opened, CurrentValve, history);
    }

    public virtual State Open()
    {
        Dictionary<string, bool> opened = Opened.ToDictionary(e => e.Key, e => e.Value);
        opened[CurrentValve.Name] = true;
        List<HistEntry> history = new(this.history);
        history.Add(new(CurrentValve, TotalFlow, FlowRate));
        return new State(valves, Minutes + 1, TotalFlow + FlowRate, FlowRate + CurrentValve.Flow, opened, CurrentValve, history);
    }

    public virtual State WalkTo(Valve valve)
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

    public bool EqualOrBetter(Valve valve, int flowRate)
        => Valve == valve && FlowRate >= flowRate;

    public override string ToString()
        => $"{Valve.Name} - {TotalFlow}f - {FlowRate}f/m";

}
public class HistEntry2 : HistEntry
{
    public Valve ValveElephant { get; private set; }
    
    public HistEntry2(Valve valve, Valve valveElephant, int totalFlow, int flowRate) : base(valve, totalFlow, flowRate)
    {
        ValveElephant = valveElephant;
    }

    public bool EqualOrBetter(Valve valve, Valve valveElephant, int flowRate)
        => (Valve == valve || Valve == valveElephant) && (ValveElephant == valve || ValveElephant == valveElephant) && FlowRate >= flowRate;
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
