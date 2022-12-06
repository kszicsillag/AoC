using System.Reactive.Linq;
using MoreLinq;

//Day1a();
//await Day1b();
//Day2a();
//Day2b();
//Day3a();
//Day3b();
//Day4a();
//Day4b();
//Day5a();
//Day5b();
Day6(4);
Day6(14);

void Day6(int windowSize)
{
    IEnumerable<char> ReadChars(string path)
    {
        using StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
            yield return (char)reader.Read();
    }

    var ret=ReadChars("input/day6.txt")
        .Select((c,i)=>(c,i+1))
        .Window(windowSize)
        .First(x=>x.Select(xx=>xx.c).Distinct().Count()==windowSize);                       
    System.Console.WriteLine(ret.Last().Item2);  
}


void Day5b()
{

     var stacks=File.ReadLines("input/day5.txt")
            .TakeWhile(l=>!l.StartsWith(" 1 "))
            .SelectMany((l,rowi)=>l.Chunk(4).Select((c,coli)=>(c[1],coli,rowi)))
            .GroupBy(x=>x.coli)
            .ToDictionary(g=>g.Key,g=>new Stack<char>(g.Where(x=>x.Item1 != ' ').OrderByDescending(x=>x.rowi).Select(x=>x.Item1)));
    System.Console.WriteLine(stacks);      

    var moves=File.ReadLines("input/day5.txt")
            .SkipWhile(l=>!l.StartsWith("move"))
            .Select(l=>l.Split(' '))
            .Select(l=>(int.Parse(l[1]),int.Parse(l[3]),int.Parse(l[5])));

    Stack<char> tmpstak= new Stack<char>();
    foreach(var x in moves)
    {
        tmpstak.Clear();
        for(int i=0; i<x.Item1;i++)
        {
            tmpstak.Push(stacks[x.Item2-1].Pop());
        }
        while(tmpstak.Count > 0)
        {
            stacks[x.Item3-1].Push(tmpstak.Pop());
        }
    }

    var result=new string(stacks.Select(s=>s.Value.Peek()).ToArray());
    System.Console.WriteLine(result);
}


void Day5a()
{

     var stacks=File.ReadLines("input/day5.txt")
            .TakeWhile(l=>!l.StartsWith(" 1 "))
            .SelectMany((l,rowi)=>l.Chunk(4).Select((c,coli)=>(c[1],coli,rowi)))
            .GroupBy(x=>x.coli)
            .ToDictionary(g=>g.Key,g=>new Stack<char>(g.Where(x=>x.Item1 != ' ').OrderByDescending(x=>x.rowi).Select(x=>x.Item1)));
    System.Console.WriteLine(stacks);      

    var moves=File.ReadLines("input/day5.txt")
            .SkipWhile(l=>!l.StartsWith("move"))
            .Select(l=>l.Split(' '))
            .Select(l=>(int.Parse(l[1]),int.Parse(l[3]),int.Parse(l[5])));

    foreach(var x in moves)
    {
        for(int i=0; i<x.Item1;i++)
        {
            stacks[x.Item3-1].Push(stacks[x.Item2-1].Pop());
        }
    }

    var result=new string(stacks.Select(s=>s.Value.Peek()).ToArray());
    System.Console.WriteLine(result);
}



void Day4b()
{
     var x=File.ReadLines("input/day4.txt")
            .Select(l=>l.Split(',','-').Select(int.Parse).ToArray())
            .Count(la=> la[0] <= la[3] ? la[0]<=la[3] && la[2]<=la[1] : la[2]<=la[1] && la[0]<=la[3] );
    System.Console.WriteLine(x);  
}


void Day4a()
{
     var x=File.ReadLines("input/day4.txt")
            .Select(l=>l.Split(',','-').Select(int.Parse).ToArray())
            .Count(la=> la[1]-la[0] >= la[3]-la[2] ? la[0]<=la[2] && la[1]>=la[3] : la[2]<=la[0] && la[3]>=la[1]);
    System.Console.WriteLine(x);  
}


void Day3b()
{  

    var x=File.ReadLines("input/day3.txt")
            .Chunk(3)
            .Select(x3=>x3.Aggregate(Enumerable.Empty<char>(),(a,x)=>a.Any() ? a.Intersect(x) : x).FirstOrDefault())
            .Sum(x=> (int)x - (char.IsUpper(x) ? (int)'A'-26 : (int)'a')+1);
    System.Console.WriteLine(x);  
    
}


void Day3a()
{  
    var x=File.ReadLines("input/day3.txt")
            .Select(x=>x[..(x.Length/2)].Intersect(x[(x.Length/2)..]).First())
            .Sum(x=> (int)x - (char.IsUpper(x) ? (int)'A'-26 : (int)'a')+1);
    System.Console.WriteLine(x);      
}

void Day2a()
{
    var x=File.ReadLines("input/day2.txt")
        .Select(x=>x.Split(' '))
        .Sum(
            x=> x.Last() switch
            {
                "X" => 1,
                "Y" => 2,
                "Z" => 3,
                _ => 0            
            }
            +
            (x.First(), x.Last()) switch
            {
                ("A", "X") => 3,
                ("B", "Y") => 3,
                ("C", "Z") => 3,
                ("A", "Z") => 0,
                ("B", "X") => 0,
                ("C", "Y") => 0,
                ("A", "Y") => 6,
                ("B", "Z") => 6,
                ("C", "X") => 6,
                _ => 0            
            }
        );
        //x.TakeLast(50).ToList().ForEach(System.Console.WriteLine);
        System.Console.WriteLine(x);
}


void Day2b()
{
    var x=File.ReadLines("input/day2.txt")
        .Select(x=>x.Split(' '))
        .Sum(
            x=> x.Last() switch
            {
                "X" => 0,
                "Y" => 3,
                "Z" => 6,
                _ => 0                
            }
            +
            (x.First(), x.Last()) switch
            {
                ("A", "X") => 3,
                ("B", "Y") => 2,
                ("C", "Z") => 1,
                ("A", "Z") => 2,
                ("B", "X") => 1,
                ("C", "Y") => 3,
                ("A", "Y") => 1,
                ("B", "Z") => 3,
                ("C", "X") => 2,
                _ => 0   
            }
        );
        //x.TakeLast(50).ToList().ForEach(System.Console.WriteLine);
        System.Console.WriteLine(x);
}





void Day1a()
{
    IObservable<string> fo=File.ReadLines("input/day1.txt").ToObservable().Publish().RefCount();
    fo.Buffer(fo.Where(l=>l==string.Empty)).Max(lb=>lb.Where(l=>l!=string.Empty).Select(int.Parse).Sum()).Do(Console.WriteLine).Subscribe();
}


async Task Day1b()
{
    IObservable<string> fo=File.ReadLines("input/day1.txt").ToObservable().Publish().RefCount();
    var x=await fo.Buffer(fo.Where(l=>l==string.Empty)).Select(lb=>lb.Where(l=>l!=string.Empty).Select(int.Parse).Sum())
        .Aggregate(new int[3],(a,x)=>a.Union(new int[]{x}).OrderByDescending(x=>x).Take(3).ToArray()).FirstAsync();
    System.Console.WriteLine(x.Sum());
}
