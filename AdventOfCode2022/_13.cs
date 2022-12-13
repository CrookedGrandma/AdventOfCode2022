using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.SymbolStore;

namespace AdventOfCode2022;
public class _13 : Base
{
    protected override void Action()
    {
        //UseExample();

        List<dynamic> inputs = new();
        List<int> indices = new();
        for (int i = 0; i * 3 < InputLines.Count; i++)
        {
            dynamic left = JsonConvert.DeserializeObject(InputLines[i * 3])!;
            dynamic right = JsonConvert.DeserializeObject(InputLines[i * 3 + 1])!;
            inputs.Add(left);
            inputs.Add(right);
            bool? co = CorrectOrder(left.First, right.First);
            if (co == true)
                indices.Add(i + 1);
            if (co == null)
                throw new Exception("shit ain't right");
        }

        int answer = indices.Sum();
        WriteLine(answer);

        B();

        dynamic divider1 = JsonConvert.DeserializeObject("[[2]]")!;
        dynamic divider2 = JsonConvert.DeserializeObject("[[6]]")!;

        inputs.Add(divider1);
        inputs.Add(divider2);

        inputs.Sort(CompareScore);

        int i1 = inputs.IndexOf(divider1) + 1;
        int i2 = inputs.IndexOf(divider2) + 1;

        int answer2 = i1 * i2;
        WriteLine(answer2);
    }

    private int CompareScore(dynamic l, dynamic r)
    {
        bool? or = CorrectOrder(l, r);
        if (or == null)
            return 0;
        if (or == true)
            return -1;
        return 1;
    }

    private bool? CorrectOrder(dynamic l, dynamic r)
    {
        string le = JsonConvert.SerializeObject(l);
        string re = JsonConvert.SerializeObject(r);
        if (l == null && r == null)
            return null;
        if (l == null || r == null)
            return l == null && r != null;
        if (l!.Type == JTokenType.Integer && r!.Type == JTokenType.Integer)
        {
            if (l.Value != r.Value)
                return l.Value < r.Value;
            else
                return CorrectOrder(l.Next, r.Next);
        }
        if (l.Type == JTokenType.Integer)
            return CorrectOrder(ConvertToJArray(l).First, r);
        if (r.Type == JTokenType.Integer)
            return CorrectOrder(l, ConvertToJArray(r).First);

        bool? listCO = CorrectOrder(l.First, r.First);
        if (listCO == null)
            return CorrectOrder(l.Next, r.Next);
        return listCO;
    }

    private dynamic ConvertToJArray(dynamic o)
    {
        string pseudoList = $"[[{JsonConvert.SerializeObject(o)}],{JsonConvert.SerializeObject(o.Next)}]";
        return JsonConvert.DeserializeObject(pseudoList)!;
    }
}
