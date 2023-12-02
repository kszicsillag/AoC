using Humanizer;

//Console.WriteLine("1a:"+Day1a());
//Console.WriteLine("1b:"+Day1b());
Console.WriteLine("2a:"+Day2a());
Console.WriteLine("2b:"+Day2b());

static int Day2a() =>
    File.ReadAllLines(Path.Combine(GetInputPath(),"day2.txt"))
        .Select(l=>new { Line= l, SplitLine= l.Split( new []{':',';'}, StringSplitOptions.RemoveEmptyEntries)})
        .Select(sl=>new{ Game= int.Parse(sl.SplitLine.First().Split(' ', StringSplitOptions.RemoveEmptyEntries).Last())
                                                    , Draw=sl, SplittedDraws=sl.SplitLine
                                                                            .Where(sll=>!sll.StartsWith("Game"))
                                                                            .Select(sll=>sll.Split(',', StringSplitOptions.RemoveEmptyEntries))})

        .Select(x=>new{x.Game, Source=x, SplittedDraws=x.SplittedDraws.Select(sll=>
                                new {R=int.TryParse(sll.FirstOrDefault(sd=>sd.Contains("red"))?.Split(' ', StringSplitOptions.RemoveEmptyEntries).First(), out var r) ? r : default
                                    , G=int.TryParse(sll.FirstOrDefault(sd=>sd.Contains("green"))?.Split(' ', StringSplitOptions.RemoveEmptyEntries).First(), out var g) ? g :default
                                    , B=int.TryParse(sll.FirstOrDefault(sd=>sd.Contains("blue"))?.Split(' ', StringSplitOptions.RemoveEmptyEntries).First(), out var b) ? b : default
                                }                                
                               )                               
                      }
               )
        .Select(xx=>new{xx.Game, xx.Source, xx.SplittedDraws, OK=xx.SplittedDraws.All(d=>d.R<=12 && d.G<=13 && d.B<=14)})
        .Where(xxx=>xxx.OK)
        .Sum(xxx=>xxx.Game);     
    
static int? Day2b() =>
    File.ReadAllLines(Path.Combine(GetInputPath(),"day2.txt"))
        .Select(l=>new { Line= l, SplitLine= l.Split( new []{':',';'}, StringSplitOptions.RemoveEmptyEntries)})
        .Select(sl=>new{ Game= int.Parse(sl.SplitLine.First().Split(' ', StringSplitOptions.RemoveEmptyEntries).Last())
                                                    , Draw=sl, SplittedDraws=sl.SplitLine
                                                                            .Where(sll=>!sll.StartsWith("Game"))
                                                                            .Select(sll=>sll.Split(',', StringSplitOptions.RemoveEmptyEntries))})

        .Select(x=>new{x.Game, Source=x, SplittedDraws=x.SplittedDraws.Select(sll=>
                                new {R=int.TryParse(sll.FirstOrDefault(sd=>sd.Contains("red"))?.Split(' ', StringSplitOptions.RemoveEmptyEntries).First(), out var r) ? r : default
                                    , G=int.TryParse(sll.FirstOrDefault(sd=>sd.Contains("green"))?.Split(' ', StringSplitOptions.RemoveEmptyEntries).First(), out var g) ? g :default
                                    , B=int.TryParse(sll.FirstOrDefault(sd=>sd.Contains("blue"))?.Split(' ', StringSplitOptions.RemoveEmptyEntries).First(), out var b) ? b : default
                                })                                                               
                      }
               )
        .Select(xx=>new{xx.Game, xx.Source, xx.SplittedDraws, MaxP= xx.SplittedDraws.Max(sd=>sd.R) * xx.SplittedDraws.Max(sd=>sd.B) * xx.SplittedDraws.Max(sd=>sd.G)  })
        .Sum(xxx=>xxx.MaxP);   
    
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
    