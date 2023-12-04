using System.Numerics;
using System.Text.RegularExpressions;
using Humanizer;

//Console.WriteLine("1a:"+Day1a());
//Console.WriteLine("1b:"+Day1b());
//Console.WriteLine("2a:"+Day2a());
//Console.WriteLine("2b:"+Day2b());
//Console.WriteLine("3a:"+Day3a());
//Console.WriteLine("3b:"+Day3b());
//Console.WriteLine("4a:"+Day4a());
Console.WriteLine("4b:"+Day4b());

static int Day4a()=> File.ReadAllLines(Path.Combine(GetInputPath(),"day4.txt"))
        .Select(l=>l.Split(new []{':', '|'}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(ls=>!ls.StartsWith("Card"))
                .Select(ls=>new HashSet<int>(ls.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ils=>int.Parse(ils))))
                .ToArray()
               )
        .Select(blocks1n2=>blocks1n2[0].Intersect(blocks1n2[1]).Count())
        .Sum(winningCnt=>winningCnt == 0 ? 0 : (int)BigInteger.Pow(new BigInteger(2), winningCnt-1));

static int Day4b()
{
    char[] splitChars=[':', '|'];
    var cardWinningsLookup=File.ReadAllLines(Path.Combine(GetInputPath(),"day4.txt"))
        .Select(l=>l.Split(splitChars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        .Select(ls => new{ GameNo=int.Parse(ls[0].Replace("Card ", string.Empty)), 
                          Blocks=ls.Skip(1)
                                 .Select(numblock=>new HashSet<int>(numblock.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                                            .Select(ils=>int.Parse(ils)))
                                        )
                                 .ToArray()})
        .Select(lx=>new {lx.GameNo, W=lx.Blocks[0].Intersect(lx.Blocks[1]).Count()})
        .ToDictionary(lxx=>lxx.GameNo, lxx=>lxx.W);
    
    int cnt=cardWinningsLookup.Count;
    int[] newborns= [.. cardWinningsLookup.Keys];
    do
    {
        newborns=newborns.SelectMany(i=>cardWinningsLookup[i]==0 ? 
                                        Enumerable.Empty<int>() : 
                                        Enumerable.Range(i+1, cardWinningsLookup[i]))
                         .ToArray();        
        cnt+=newborns.Length;
    }while(newborns.Length > 0);
    return cnt;
}


static int Day3a()
{
    var charMap=File.ReadAllLines(Path.Combine(GetInputPath(),"day3.txt"))
                .Select(l=>l.ToArray()).ToArray();
     
     var tmp=File.ReadAllLines(Path.Combine(GetInputPath(),"day3.txt"))
        .Select((l,li)=>new {Line=l, LineIndex = li, Parts=Regex.Matches(l, @"\d+")
                                        .Select(m=>new {Match=m, IsPart=Enumerable.Range(m.Index, m.Length)
                                                                     .SelectMany(xr=>new []
                                                                        {(li,xr-1)
                                                                         ,(li,xr+1)
                                                                         ,(li-1,xr-1)
                                                                         ,(li-1,xr+1)
                                                                         ,(li-1,xr)
                                                                         ,(li+1,xr-1)
                                                                         ,(li+1,xr+1)
                                                                         ,(li+1,xr)
                                                                        }.Where(xrr=>xrr.Item1>=0 && xrr.Item1<charMap.Length && xrr.Item2 >= 0 && xrr.Item2 < charMap[0].Length))
                                                                        .Select(xrr=>charMap[xrr.Item1][xrr.Item2])
                                                                        .Any(cx=>cx != '.' && !char.IsDigit(cx))
                                                                        })})   
        .Sum(x2=>x2.Parts.Where(xp=>xp.IsPart).Sum(xp=>int.Parse(xp.Match.Value)));        
    return tmp;
}

static int Day3b()
{
    var charMap=File.ReadAllLines(Path.Combine(GetInputPath(),"day3.txt"))
                .Select(l=>l.ToArray()).ToArray();
     
     var tmp=File.ReadAllLines(Path.Combine(GetInputPath(),"day3.txt"))
        .Select((l,li)=>new {Line=l, LineIndex = li, Parts=Regex.Matches(l, @"\d+")
                                        .Select(m=>new {Match=m, Stars=Enumerable.Range(m.Index, m.Length)
                                                                     .SelectMany(xr=>new []
                                                                        {(li,xr-1)
                                                                         ,(li,xr+1)
                                                                         ,(li-1,xr-1)
                                                                         ,(li-1,xr+1)
                                                                         ,(li-1,xr)
                                                                         ,(li+1,xr-1)
                                                                         ,(li+1,xr+1)
                                                                         ,(li+1,xr)
                                                                        }.Where(xrr=>xrr.Item1>=0 && xrr.Item1<charMap.Length && xrr.Item2 >= 0 && xrr.Item2 < charMap[0].Length))
                                                                        .Distinct()
                                                                        .Where(xrr=>charMap[xrr.Item1][xrr.Item2]=='*')
                                                                        })})
        .SelectMany(x2=>x2.Parts.Where(p=>p.Stars.Any())).ToArray();
        var tmp2= 
            tmp.SelectMany(x=>x.Stars).Distinct()
               .Select(xs=>tmp.Where(xx=>xx.Stars.Contains(xs)).ToArray())
               .Where(xss=>xss.Length == 2)
               .Sum(xss=>int.Parse(xss[0].Match.Value)*int.Parse(xss[1].Match.Value));
    return tmp2;
}

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
    