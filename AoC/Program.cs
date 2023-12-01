using Humanizer;

Console.WriteLine(Day1a());
Console.WriteLine(Day1b());

static string GetInputPath() =>  Path.Combine(Environment.GetEnvironmentVariable("CODESPACE_VSCODE_FOLDER") ?? string.Empty, "inputs");

static int Day1a() =>
    File.ReadAllLines(Path.Combine(GetInputPath(),"day1.txt"))
        .Sum(l=>int.Parse(new string(l.First(c=>char.IsDigit(c)),1))*10+int.Parse(new string(l.Last(c=>char.IsDigit(c)),1)));    
    
static int Day1b()
{
    var digitMap = Enumerable.Range(0,10).Select(e => new {S=e.ToString(), I=e})
                                                .Concat(Enumerable.Range(1,9).Select(e2 => new {S=e2.ToWords(), I=e2}))
                                                .ToDictionary(x=>x.S, x=>x.I);
      File.ReadAllLines(Path.Combine(GetInputPath(),"day1.txt"))
        .Select(l=>digitMap.Select(d=>new {KVP=d, IF=l.IndexOf(d.Key)}).Where(x=>x.IF>-1).OrderBy(x=>x.IF))
        .Sum(lx=>lx.First().IF);    

    foreach(var kvp in digitMap)
    {
        Console.WriteLine(kvp.Key+":"+kvp.Value);
    }
    return 0;
}
    