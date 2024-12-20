﻿using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
//Console.WriteLine(nameof(Day1a)+":"+Day1a());
//Console.WriteLine(nameof(Day1b)+":"+Day1b());
//Console.WriteLine(nameof(Day2a)+":"+Day2a());
//Console.WriteLine(nameof(Day2b)+":"+Day2b());
//Console.WriteLine(nameof(Day3a)+":"+Day3a());
//Console.WriteLine(nameof(Day3b)+":"+Day3b());

//Console.WriteLine(nameof(Day4a)+":"+Day4a());
Console.WriteLine(nameof(Day4b)+":"+Day4b());



static int Day4a()
{
   var input=File.ReadAllLines(@"input/day4.txt").SelectMany((l, ridx)=>l.ToArray().Select((c,cidx)=>new {c,ridx,cidx})).ToArray();
   var h = input.GroupBy(x=>x.ridx).Select(g=>new string(g.OrderBy(gx=>gx.cidx).Select(gx=>gx.c).ToArray()));
   var v = input.GroupBy(x=>x.cidx).Select(g=>new string(g.OrderBy(gx=>gx.ridx).Select(gx=>gx.c).ToArray()));
   var d = input.GroupBy(x=>x.cidx-x.ridx).Select(g=>new string(g.OrderBy(gx=>gx.cidx).Select(gx=>gx.c).ToArray()));
   var ad = input.GroupBy(x=>x.cidx+x.ridx).Select(g=>new string(g.OrderBy(gx=>gx.ridx).Select(gx=>gx.c).ToArray()));
   return h.Concat(v).Concat(d).Concat(ad).Sum(s=>AoC.Utils.Day4a1Regex().Matches(s).Count+AoC.Utils.Day4a2Regex().Matches(s).Count);
   // return 0;
}

static int Day4b()
{
   var input=File.ReadAllLines(@"input/day4.txt").Select(l=>l.ToArray()).ToArray();
   int cnt=0;
   for(int x=1; x<input.Length-1;x++)
    for(int y=1; y<input[x].Length-1;y++)
    {
        if(input[x][y]=='A')
        {
            if((input[x-1][y-1]=='M' && input[x+1][y+1]=='S' && input[x+1][y-1]=='M' && input[x-1][y+1]=='S') //MSMS
                || (input[x-1][y-1]=='S' && input[x+1][y+1]=='M' && input[x+1][y-1]=='M' && input[x-1][y+1]=='S') //SMMS
                || (input[x-1][y-1]=='S' && input[x+1][y+1]=='M' && input[x+1][y-1]=='S' && input[x-1][y+1]=='M') //SMSM
                || (input[x-1][y-1]=='M' && input[x+1][y+1]=='S' && input[x+1][y-1]=='S' && input[x-1][y+1]=='M') //MSSM
              )
              cnt++;
        }        
    }    
    return cnt;
}


static int Day3a()
{
   var input=File.ReadAllText(@"input/day3.txt");
   return AoC.Utils.Day3aRegex().Matches(input).Sum(m=>int.Parse(m.Groups[1].Value)*int.Parse(m.Groups[2].Value));
}

static int Day3b()
{
   var input=File.ReadAllText(@"input/day3.txt");
   var control= new SortedSet<(string value, int index)>(AoC.Utils.Day3bRegex().Matches(input).Select(m=>m.Groups.Cast<Group>().First(g=>g.Success))
                                     .Select(g=>(g.Value, g.Index))
                                    , Comparer<(string, int)>.Create((g1, g2)=> g1.Item2.CompareTo(g2.Item2)));
   return AoC.Utils.Day3aRegex().Matches(input)
   .Select(m=>new {m, MaxControl=control.GetViewBetween((string.Empty,0),(string.Empty,m.Index)).LastOrDefault()})
   .Where(x=>x.MaxControl == default || x.MaxControl.value == "do()")
   .Sum(x=>int.Parse(x.m.Groups[1].Value)*int.Parse(x.m.Groups[2].Value));

}


static int Day2a() => File.ReadLines(@"input/day2.txt")
        .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        .Select(ll => ll.Select(int.Parse))
        .Select(ll => ll.Buffer(2, 1).Where(il => il.Count() == 2).Select(il => new { Element = il.ElementAt(0), Next = il.ElementAt(1) }).ToArray())
        .Count(b => b[0].Element < b[0].Next ? b.All(e => e.Element < e.Next && Math.Abs(e.Element - e.Next) < 4) : b.All(e => e.Element > e.Next && Math.Abs(e.Element - e.Next) < 4));

static int Day2b()
{
    var remaining = File.ReadLines(@"input/day2.txt")
        .Select(l=>l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        .Select(ll=>ll.Select(int.Parse).ToArray())
        .Select(ll=>new { Nums=ll, Orig=ll })
        .ToArray();       
    int retVal = 0;
    int it=-1;

    do{

       //var xxx= remaining.Select(b=>b.Nums.Where((p, idx)=> idx != it).ToArray()).ToArray();
       var eval=remaining.Select(b=>new{Nums=b.Nums.Where((p, idx)=> idx != it).ToArray(), b.Orig})
                 .Select(ll=>new {Nums=ll.Nums.Buffer(2,1).Where(il=>il.Count == 2).Select(il=>new {Element=il.ElementAt(0), Next= il.ElementAt(1)}).ToArray(), ll.Orig})
                 .Select(bx=> new {bx, SAFU=bx.Nums[0].Element < bx.Nums[0].Next ? bx.Nums.All(e=>e.Element < e.Next && Math.Abs(e.Element-e.Next) < 4) : bx.Nums.All(e=>e.Element > e.Next && Math.Abs(e.Element-e.Next) < 4), bx.Orig})
                 .ToArray();
       retVal+= eval.Count(x=>x.SAFU);
       remaining = eval.Where(x=>!x.SAFU && it < x.Orig.Length).Select(x=> new {Nums=x.Orig, Orig = x.Orig}).ToArray();
       it++;
    }while(remaining.Any());
    return retVal;
}


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
                                     .Select(g=>g.Select(x=>x.Item1).ToArray())
                                     .ToArray();    

    var histoLookup= ordered2sets[1].GroupBy(i=>i).ToDictionary(g=>g.Key,g=>g.Count());
    return Enumerable.Range(0,ordered2sets[0].Length).Sum(i=>ordered2sets[0][i]*histoLookup.GetValueOrDefault(ordered2sets[0][i]));
}