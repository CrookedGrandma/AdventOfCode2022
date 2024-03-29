﻿using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022;
public class _9 : Base
{
    protected override void Action()
    {
        //UseExample();
        List<Pos> tailPositions = new();
        Pos head = new(0, 0), tail = new(0, 0);

        foreach (string line in InputLines)
        {
            string[] split = line.Split(' ');
            char dir = split[0][0];
            for (int i = 0; i < int.Parse(split[1]); i++)
            {
                head = HeadAfterMove(head, dir);
                tail = TailPos(head, tail);
                tailPositions.Add(tail);
            }
        }

        var diff = tailPositions.Distinct().ToList();
        int count = diff.Count();

        WriteLine(count);

        B();

        head = new(0, 0);
        tailPositions.Clear();
        List<Pos> tails = Enumerable.Range(0, 9).Select(_ => new Pos(0, 0)).ToList();

        foreach (string line in InputLines)
        {
            string[] split = line.Split(' ');
            char dir = split[0][0];
            for (int i = 0; i < int.Parse(split[1]); i++)
            {
                head = HeadAfterMove(head, dir);

                tails[0] = TailPos(head, tails[0]);
                for (int j = 1; j < 9; j++)
                {
                    tails[j] = TailPos(tails[j - 1], tails[j]);
                }

                tailPositions.Add(tails[8]);
            }
        }

        diff = tailPositions.Distinct().ToList();
        count = diff.Count();

        WriteLine(count);
    }

    private Pos HeadAfterMove(Pos head, char dir)
    {
        return dir switch
        {
            'R' => head.Right(),
            'L' => head.Left(),
            'U' => head.Up(),
            'D' => head.Down(),
            _ => throw new Exception("nope move")
        };
    }

    private Pos TailPos(Pos head, Pos tail)
    {
        int xdif = head.x - tail.x;
        int ydif = head.y - tail.y;
        if (xdif == 0)
        {
            return ydif switch
            {
                > 2 => throw new Exception("nope up"),
                2 => tail.Up(),
                -2 => tail.Down(),
                < -2 => throw new Exception("nope down"),
                _ => tail,
            };
        }
        else if (ydif == 0)
        {
            return xdif switch
            {
                > 2 => throw new Exception("nope right"),
                2 => tail.Right(),
                -2 => tail.Left(),
                < -2 => throw new Exception("nope left"),
                _ => tail,
            };
        }
        else if (Math.Abs(xdif) > 1 || Math.Abs(ydif) > 1)
        {
            if (xdif > 0 && ydif > 0)
                return tail.Right().Up();
            if (xdif > 0 && ydif < 0)
                return tail.Right().Down();
            if (xdif < 0 && ydif > 0)
                return tail.Left().Up();
            if (xdif < 0 && ydif < 0)
                return tail.Left().Down();
        }
        return tail;
    }
}
