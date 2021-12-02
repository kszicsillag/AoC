using System;
using System.Linq;

//Console.WriteLine("Hello, World!");
Console.WriteLine($"{nameof(Day1A)}:{Day1A()}");
Console.WriteLine($"{nameof(Day1B)}:{Day1B()}");
//Day1b();

static int Day1A() =>
    System.IO.File.ReadAllLines("input/day1.txt")
            .Select(l => int.Parse(l))
        .Buffer(2,1)
            .Where(b=>b.Count==2)
        .Count(b=>b[0] < b[1]);

static int Day1B() =>
    System.IO.File.ReadAllLines("input/day1.txt")
        .Select(l => int.Parse(l))
        .Buffer(3, 1)
        .Where(xx => xx.Count == 3)
        .Select(b => b.Sum())
        .Buffer(2, 1)
        .Where(bb => bb.Count == 2)
        .Count(bb => bb[0] < bb[1]);
