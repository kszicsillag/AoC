// See https://aka.ms/new-console-template for more information
using System.Reactive.Linq;

//Day1a();
//await Day1b();
Day2b();


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
