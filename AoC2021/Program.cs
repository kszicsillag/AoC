using System;
using System.Linq;

//Console.WriteLine("Hello, World!");
Console.WriteLine($"{nameof(Day2A)}:{Day2A()}");
Console.WriteLine($"{nameof(Day2B)}:{Day2B()}");
//Console.WriteLine($"{nameof(Day1A)}:{Day1A()}");
//Console.WriteLine($"{nameof(Day1B)}:{Day1B()}");
//Day1b();


static int Day2A() => System.IO.File.ReadAllLines("input/day2.txt")
    .Select(l => l.Split(' '))
    .Where(p => p.Length == 2)
    .Select(p => new { D = p[0], U = int.Parse(p[1]) })
    .Select(x => new
    {
        H = x.D == "forward" ? x.U : 0,
        V = x.D switch
        {
            "down" => x.U,
            "up" => -x.U,
            _ => 0
        }
    })
    .GroupBy(x => true)
    .Select(g => new { SH = g.Sum(x => x.H), SV = g.Sum(x => x.V) })
    .Select(gg => gg.SH * gg.SV)
    .First();

static int Day2B() => System.IO.File.ReadAllLines("input/day2.txt")
    .Select(l => l.Split(' '))
    .Where(p => p.Length == 2)
    .Select(p => new { D = p[0], U = int.Parse(p[1]) })
    .Scan( new {SH=0, SV=0, SA=0}, (
            a,s)=> 
            new
            {
                SH= s.D == "forward" ? a.SH + s.U : a.SH,
                SV= s.D == "forward" ? a.SV + a.SA * s.U : a.SV,
                SA = s.D switch
                {
                    "down" => a.SA + s.U,
                    "up" => a.SA - s.U,
                    _ => a.SA
                }
            })
    .Select(gg => gg.SH * gg.SV)
    .Last();

static int Day1A() =>
    System.IO.File.ReadAllLines("input/day1.txt")
        .Select(l => int.Parse(l))
        .Buffer(2, 1)
        .Where(b => b.Count == 2)
        .Count(b => b[0] < b[1]);

static int Day1B() =>
    System.IO.File.ReadAllLines("input/day1.txt")
        .Select(l => int.Parse(l))
        .Buffer(3, 1)
        .Where(xx => xx.Count == 3)
        .Select(b => b.Sum())
        .Buffer(2, 1)
        .Where(bb => bb.Count == 2)
        .Count(bb => bb[0] < bb[1]);