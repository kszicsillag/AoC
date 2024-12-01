Console.WriteLine(nameof(Day1a)+":"+Day1a());
Console.WriteLine(nameof(Day1b)+":"+Day1b());

static int Day1a()
{
    var ordered2sets=File.ReadLines(@"input/day1.txt").Select(l=>l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                                     .SelectMany(la=>new []{(int.Parse(la[0]),0),(int.Parse(la[1]),1)})
                                     .GroupBy(x=>x.Item2)
                                     .Select(g=>g.OrderBy(x=>x.Item1).Select(x=>x.Item1).ToArray())
                                     .ToArray();    

    return Enumerable.Range(0,ordered2sets[0].Length).Sum(i=>Math.Abs(ordered2sets[0][i]-ordered2sets[1][i]));
}


static int Day1b()
{
    var ordered2sets=File.ReadLines(@"input/day1.txt").Select(l=>l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                                     .SelectMany(la=>new []{(int.Parse(la[0]),0),(int.Parse(la[1]),1)})
                                     .GroupBy(x=>x.Item2)
                                     .Select(g=>g.OrderBy(x=>x.Item1).Select(x=>x.Item1).ToArray())
                                     .ToArray();    

    var histoLookup= ordered2sets[1].GroupBy(i=>i).ToDictionary(g=>g.Key,g=>g.Count());
    return Enumerable.Range(0,ordered2sets[0].Length).Sum(i=>ordered2sets[0][i]*histoLookup.GetValueOrDefault(ordered2sets[0][i]));
}