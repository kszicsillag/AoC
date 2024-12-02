//Console.WriteLine(nameof(Day1a)+":"+Day1a());
//Console.WriteLine(nameof(Day1b)+":"+Day1b());
//Console.WriteLine(nameof(Day2a)+":"+Day2a());
Console.WriteLine(nameof(Day2b)+":"+Day2b());

static int Day2a()
{
    return File.ReadLines(@"input/day2.txt")
        .Select(l=>l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        .Select(ll=>ll.Select(int.Parse))
        .Select(ll=>ll.Buffer(2,1).Where(il=>il.Count() == 2).Select(il=>new {Element=il.ElementAt(0), Next= il.ElementAt(1)}).ToArray())
        .Count(b=>b[0].Element < b[0].Next ? b.All(e=>e.Element < e.Next && Math.Abs(e.Element-e.Next) < 4) : b.All(e=>e.Element > e.Next && Math.Abs(e.Element-e.Next) < 4) )
        ;

}

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