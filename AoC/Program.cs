
Day1a();

void Day1a()
{
}

/*
async Task Day1b()
{
    IObservable<string> fo=File.ReadLines("input/day1.txt").ToObservable().Publish().RefCount();
    var x=await fo.Buffer(fo.Where(l=>l==string.Empty)).Select(lb=>lb.Where(l=>l!=string.Empty).Select(int.Parse).Sum())
        .Aggregate(new int[3],(a,x)=>a.Union(new int[]{x}).OrderByDescending(x=>x).Take(3).ToArray()).FirstAsync();
    System.Console.WriteLine(x.Sum());
}
*/