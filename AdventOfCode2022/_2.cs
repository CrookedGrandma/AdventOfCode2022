namespace AdventOfCode2022;
public class _2 : Base
{
    protected override void Action()
    {
        List<int> scores = new();
        foreach (string line in InputLines)
        {
            int elf = line[0] - 64;
            int you = line[2] - 'W';
            scores.Add(Score(elf, you));
        }
        WriteLine(scores.Sum());

        B();

        List<int> scoresB = new();
        foreach (string line in InputLines)
        {
            int elf = line[0] - 64;
            int outcome = line[2] - 'W';
            scoresB.Add(ScoreB(elf, outcome));
        }
        WriteLine(scoresB.Sum());
    }

    private int Score(int elf, int you)
    {
        int outcome = 0;
        if (elf == you)
            outcome = 3;
        if (you - elf == 1 || you - elf == -2)
            outcome = 6;
        return you + outcome;
    }

    private int ScoreB(int elf, int outcome)
    {
        outcome = outcome switch
        {
            1 => 0,
            2 => 3,
            _ => 6,
        };
        int you = outcome switch
        {
            0 => (elf - 1),
            3 => elf,
            _ => (elf + 1),
        };
        if (you < 1) you += 3;
        if (you > 3) you -= 3;
        return you + outcome;
    }
}
