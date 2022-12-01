// See https://aka.ms/new-console-template for more information
using System.Reactive.Linq;

//Day1a();
await Day1b();

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