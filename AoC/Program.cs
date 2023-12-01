using Humanizer;

Console.WriteLine("1a:"+Day1a());
Console.WriteLine("1b:"+Day1b());

static string GetInputPath() =>  Path.Combine(Directory.GetCurrentDirectory(), "inputs");

static int Day1a() =>
    File.ReadAllLines(Path.Combine(GetInputPath(),"day1.txt"))
        .Sum(l=>int.Parse(new string(l.First(c=>char.IsDigit(c)),1))*10+int.Parse(new string(l.Last(c=>char.IsDigit(c)),1)));    
    
static int? Day1b()
{
    var digitMap = Enumerable.Range(0,10).Select(e => new {S=e.ToString(), I=e})
                                                .Concat(Enumerable.Range(1,9).Select(e2 => new {S=e2.ToWords(), I=e2}))
                                                .ToDictionary(x=>x.S, x=>x.I);
    return File.ReadAllLines(Path.Combine(GetInputPath(),"day1.txt"))
        .Select(l=>digitMap.Select(d=>new {KVP=d, IF1=l.IndexOf(d.Key), IF2=l.LastIndexOf(d.Key)}))
        .Sum(lx=>lx.Where(x=>x.IF1>-1).MinBy(x=>x.IF1)?.KVP.Value*10+lx.Where(x=>x.IF2>-1).MaxBy(x=>x.IF2)?.KVP.Value);    
    
}
    