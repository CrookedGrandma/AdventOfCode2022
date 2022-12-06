namespace AdventOfCode2022;
public class _6 : Base
{
    protected override void Action()
    {
        //UseExample();
        string input = InputLines[0];

        int buffPos = -1;
        for (int i = 0; i < input.Length; i++)
        {
            if (i < 4) continue;
            char[] last4 = input[(i - 4)..i].ToCharArray();
            if (last4.Distinct().Count() == 4)
            {
                buffPos = i;
                break;
            }
        }
        WriteLine(buffPos);

        B();

        buffPos = -1;
        for (int i = 0; i < input.Length; i++)
        {
            if (i < 14) continue;
            char[] last4 = input[(i - 14)..i].ToCharArray();
            if (last4.Distinct().Count() == 14)
            {
                buffPos = i;
                break;
            }
        }
        WriteLine(buffPos);
    }
}
