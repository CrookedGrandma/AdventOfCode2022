namespace AdventOfCode2022;
public abstract class Base
{
    protected List<string> InputLines { get; private set; }
    protected StreamWriter OutputWriter { get; private set; }
    protected string Day { get; private set; }

    private readonly string _outputFilePath;

    public Base()
    {
        Day = GetType().Name[1..];
        InputLines = File.ReadAllLines($"../../../Inputs/{Day}.txt").ToList();
        _outputFilePath = $"../../../Outputs/{Day}.txt";
        OutputWriter = new StreamWriter(_outputFilePath) { AutoFlush = true };
    }

    protected abstract void Action();

    protected void WriteLine(object x) {
        WriteLine(x.ToString() ?? "");
    }
    protected void WriteLine(string x = "") {
        Console.WriteLine(x);
        OutputWriter.WriteLine(x);
    }

    protected void Write(object x) {
        Write(x.ToString() ?? "");
    }
    protected void Write(string x = "") {
        Console.Write(x);
        OutputWriter.Write(x);
    }

    protected void UseExample() {
        InputLines = File.ReadAllLines($"../../../Inputs/example{Day}.txt").ToList();
    }

    protected void B() {
        WriteLine();
        WriteLine("============== B ==============");
        WriteLine();
    }

    public void Run()
    {
        Action();
        OutputWriter.Close();
    }
}

public class Tup<T1, T2>
{
    public T1 a;
    public T2 b;

    public Tup(T1 a, T2 b)
    {
        this.a = a;
        this.b = b;
    }
}
