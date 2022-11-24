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

    public void Run()
    {
        Action();
        OutputWriter.Close();
    }
}
