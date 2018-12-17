using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC2018
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine($"1a:{Day1a}");
            //Console.WriteLine($"1b:{Day1b}");
            //Console.WriteLine($"2a:{Day2a}");
            //Console.WriteLine($"2b:{Day2b}");
            //Console.WriteLine($"3a:{Day3a}");
            //Console.WriteLine($"3b:{Day3b}");
            //Console.WriteLine($"4a:{Day4a}");
            //Console.WriteLine($"4b:{Day4b}");
            //Console.WriteLine($"5a:{Day5a}");
            //Console.WriteLine($"5b:{Day5b}");
            //Console.WriteLine($"6a:{Day6a}");
            //Console.WriteLine($"6b:{Day6b}");
            //Console.WriteLine($"7a:{Day7a}");
            //Console.WriteLine($"7b:{Day7b}");
            //Console.WriteLine($"8a:{Day8a}");
            //Console.WriteLine($"8b:{Day8b}");
            Console.WriteLine($"9a:{Day9a}");
            Console.WriteLine($"9b:{Day9b}");
            Console.ReadLine();
        }


        public static long Day9b
        {
            get
            {
                int[] intData = File.ReadAllText("input9.txt").Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s, out int ires) ? ires : (int?)null)
                    .Where(x => x.HasValue)
                    .Select(x => x.Value)
                    .Take(2)
                    .ToArray();
                int playerNum = intData[0];
                int maxMarbleNum = intData[1] * 100;
                int playingMarble = 0;
                int curridx = 0;
                Dictionary<int, long> scores = new Dictionary<int, long>();
                List<int> marbles = new List<int>(maxMarbleNum) { playingMarble };
                do
                {
                    playingMarble++;
                    if (playingMarble % 23 == 0)
                    {
                        curridx -= 7;
                        if (curridx < 0)
                            curridx += marbles.Count;
                        int playerid = playingMarble % playerNum;
                        if (!scores.ContainsKey(playerid))
                            scores.Add(playerid, 0);
                        scores[playerid] += (playingMarble + marbles[curridx]);
                        marbles.RemoveAt(curridx);
                    }
                    else
                    {
                        curridx = ((curridx + 1) % marbles.Count) + 1;
                        marbles.Insert(curridx, playingMarble);
                    }

                } while (playingMarble < maxMarbleNum);

                long max = scores.Max(kvp => kvp.Value);
                return max;

            }
        }

        public static long Day9a
        {
            get
            {
                int[] intData = File.ReadAllText("input9.txt").Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s=>int.TryParse(s, out int ires) ? ires: (int?)null)
                    .Where(x=>x.HasValue)
                    .Select(x=>x.Value)
                    .Take(2)
                    .ToArray();
                int playerNum = intData[0];
                int maxMarbleNum = intData[1];
                int playingMarble = 0;
                int curridx = 0;
                Dictionary<int, long> scores= new Dictionary<int, long>();
                List<int> marbles = new List<int>(maxMarbleNum) { playingMarble};
                do
                {
                    playingMarble++;
                    if (playingMarble % 23 == 0)
                    {
                        curridx -= 7;
                        if (curridx < 0)
                            curridx += marbles.Count;
                        int playerid = playingMarble % playerNum;
                        if (!scores.ContainsKey(playerid))
                            scores.Add(playerid, 0);
                        scores[playerid] +=(playingMarble + marbles[curridx]);
                        marbles.RemoveAt(curridx);
                    }
                    else
                    {
                        curridx = ((curridx + 1) % marbles.Count) + 1;
                        marbles.Insert(curridx, playingMarble);
                    }

                } while (playingMarble < maxMarbleNum);

                long max = scores.Max(kvp => kvp.Value);
                return max;
               
            }
        }

        public static long Day8b
        {
            get
            {
                int[] steps = File.ReadAllText("input8.txt").Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s))
                    .ToArray();
                Span<int> mem = steps.AsSpan();
                var stepsIndexed = steps
                     .Select((x, i) => new { val = x, idx = i });
                Dictionary<int, List<int>> childSums = new Dictionary<int, List<int>>();
                List<int> childSumList;
                int mysum;
                do
                {
                    var zeroeridx = stepsIndexed.Where(x => x.val != -1)
                        .Select((x, i) => new { x, fidx = i })
                        .First(xx => xx.fidx % 2 == 0 && xx.x.val == 0).x.idx;
                    var nextleafidx = stepsIndexed.Skip(zeroeridx + 2).First(x => x.val != -1).idx;                    
                    mysum = (!childSums.TryGetValue(zeroeridx, out childSumList)) ?
                        mem.Slice(nextleafidx, steps[zeroeridx + 1]).ToArray().Sum():
                        mem.Slice(nextleafidx, steps[zeroeridx + 1]).ToArray()
                            .Select(i => i > childSumList.Count ? 0 : childSumList[i - 1]).Sum();

                    mem.Slice(nextleafidx, steps[zeroeridx + 1]).Fill(-1);
                    mem.Slice(zeroeridx, 2).Fill(-1);
                    if (!steps.Any(x => x != -1))
                       break;
                    int notzerolastidx = stepsIndexed.Take(zeroeridx).Last(i => i.val != -1).idx - 1;
                    steps[notzerolastidx]--;
                    if (!childSums.TryGetValue(notzerolastidx, out childSumList))
                        childSums.Add(notzerolastidx, new List<int> { mysum});
                    else
                        childSumList.Add(mysum);

                } while (steps.Any(x => x != -1));

                return mysum;
            }
        }

        public static long Day8a
        {
            get
            {
                int[] digits = File.ReadAllText("input8.txt").Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s))                    
                    .ToArray();
                Span<int> mem = digits.AsSpan();
                var digitsIndexed = digits
                     .Select((x,i) => new {val=x, idx=i });
                long sum = 0;
                do
                {
                    var zeroeridx = digitsIndexed.Where(x=>x.val != -1)
                        .Select((x,i)=>new { x, fidx=i })
                        .First(xx => xx.fidx % 2 == 0 && xx.x.val == 0).x.idx;
                    var nextleafidx = digitsIndexed.Skip(zeroeridx + 2).First(x => x.val != -1).idx;
                    sum += mem.Slice(nextleafidx, digits[zeroeridx + 1]).ToArray().Sum();
                    mem.Slice(nextleafidx, digits[zeroeridx + 1]).Fill(-1);
                    mem.Slice(zeroeridx, 2).Fill(-1);
                    if (!digits.Any(x => x != -1))
                        break;
                    int notzerolastidx = digitsIndexed.Take(zeroeridx).Last(i => i.val != -1).idx - 1;
                    digits[notzerolastidx]--;
                } while (digits.Any(x=>x!=-1));

                return sum;
            }
        }


        public static int Day7b
        {
            get
            {
                var steps = File.ReadAllLines("input7.txt")
                    .Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries).Where(s => s.Length == 1).ToArray())
                    .Select(ls => new { Step = ls[1][0], Pre = ls[0][0] });

                var tasks = steps.SelectMany(s => new[] { s.Pre, s.Step }).Distinct()
                    .Select(s => new Day7DO { Task=s, Goal= s - 'A' + 61})
                    .ToArray();

                var rules= steps
                    .GroupBy(x => x.Step)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.Pre).ToList());

               
                var nopre = rules.SelectMany(kvp => kvp.Value).Distinct()
                                        .Except(rules.Select(kvp2 => kvp2.Key))
                                        .OrderBy(c => c);

                //var xx=tasks.Where(t => nopre.Contains(t.task)).ToArray();
                tasks.Where(t => nopre.Contains(t.Task)).ForEach(t => t.State=1);
                tasks.Where(t => t.State == 1).OrderBy(t=>t.Task).Take(5).ForEach(t => t.State=2);

                int elapsed = 0;
                do
                {
                    elapsed++;
                    //working
                    tasks.Where(t => t.State == 2).ForEach(t => t.Worked++);
                    //set completed
                    tasks.Where(t => t.State == 2 && t.Worked==t.Goal).ForEach(t => t.State=3);
                    //set avail
                    var completed= tasks.Where(t => t.State == 3).Select(t=>t.Task).ToArray();
                    tasks.Where(t => t.State == 0 && !rules[t.Task].Except(completed).Any()).ForEach(t => t.State = 1);
                    //start new work
                    var freeworkers = 5 - tasks.Count(t => t.State == 2);
                    tasks.Where(t => t.State == 1).OrderBy(t => t.Task).Take(freeworkers).ForEach(t => t.State = 2);

                } while (tasks.Any(t=>t.State != 3));


                return elapsed;
            }
        }



        public static string Day7a
        {
            get
            {
              

                var steps = File.ReadAllLines("input7.txt")
                    .Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries).Where(s=>s.Length==1).ToArray())
                    .Select(ls => new {Step=ls[1][0], Pre=ls[0][0]  })
                    .GroupBy (x=> x.Step)
                    .ToDictionary(g=>g.Key, g=>g.Select(x=>x.Pre).ToList());
                
                List<char> avails = new List<char>();
                var nopre=steps.SelectMany(kvp => kvp.Value).Distinct()
                                        .Except(steps.Select(kvp2 => kvp2.Key))
                                        .OrderBy(c=>c)
                                        .ToArray();
                char newlydone=nopre[0];
                List<char> orderedsteps = new List<char> { newlydone };
                avails.AddRange(nopre.Skip(1));
                do
                {                    
                    steps.ForEach(kvp=>kvp.Value.Remove(newlydone));
                    var canstarts=steps.Where(kvp => !kvp.Value.Any()).Select(kvp=>kvp.Key).ToArray();
                    canstarts.ForEach(s => steps.Remove(s));
                    avails.AddRange(canstarts);
                    newlydone = avails.OrderBy(c => c)
                                        .First();
                    orderedsteps.Add(newlydone);
                    avails.Remove(newlydone);

                } while (steps.Count > 0);
                

                return new string(orderedsteps.ToArray());
            }
        }

        public static int Day6b
        {
            get
            {
                Point[] points = File.ReadAllLines("input6.txt").Select(l => l.Split(", ", StringSplitOptions.RemoveEmptyEntries))
                    .Select(ls => new Point(int.Parse(ls[0]), int.Parse(ls[1])))
                    .ToArray();
                Point centroid = new Point((int)points.Average(p => p.X), (int)points.Average(p => p.Y));
                Point[] neighbourOffsets = Enumerable.Range(-1, 3)
                                                .SelectMany(p => Enumerable.Range(-1, 3), (x, y) => new Point(x, y))
                                                .Where(p => p.X != 0 || p.Y != 0)
                                                .ToArray();
                HashSet<Point> pointsVisited = new HashSet<Point> { centroid };
                int numofNewGoodPoints;
                int goodPoints=1;
                do
                {
                    var pDists = pointsVisited.SelectMany(p => neighbourOffsets, (p, n) => new Point(p.X + n.X, p.Y + n.Y))
                        .Where(po => !pointsVisited.Contains(po))
                        .Distinct()
                        .Select(po => new
                            {
                                po,
                                distsum = points.Sum(p => Math.Abs(p.X - po.X) + Math.Abs(p.Y - po.Y))                                      
                            })
                         .ToArray();
                    pDists.Select(x => x.po).ForEach(x => pointsVisited.Add(x));
                    numofNewGoodPoints=pDists.Count(x => x.distsum < 10000);
                    goodPoints += numofNewGoodPoints;
                        
                } while (numofNewGoodPoints > 0);

                return goodPoints;
            }
        }

        public static int Day6a
        {
            get
            {
                Point[] points=File.ReadAllLines("input6.txt").Select(l => l.Split(", ", StringSplitOptions.RemoveEmptyEntries))
                    .Select(ls => new Point(int.Parse(ls[0]), int.Parse(ls[1])))
                    .ToArray();
                var orderX = points.OrderBy(p => p.X);
                var orderY = points.OrderBy(p => p.Y);
                Point loc = new Point(orderX.First().X, orderY.First().Y);
                Point far = new Point(orderX.Last().X, orderY.Last().Y);
                Rectangle r = new Rectangle(loc.X, loc.Y, far.X - loc.X, far.Y - loc.Y);
                Dictionary<Point, int?> areas = points.ToDictionary(p=>p, p=>(int?)1);
                Point[] neighbourOffsets = Enumerable.Range(-1, 3)
                                                .SelectMany(p => Enumerable.Range(-1, 3), (x, y) => new Point(x, y))
                                                .Where(p=>p.X!=0 || p.Y!=0)
                                                .ToArray();
                HashSet<Point> pointsVisited = new HashSet<Point>(points);
                
                bool added;
                do
                {                   
                    var pDists = pointsVisited.SelectMany(p => neighbourOffsets, (p, n) => new Point(p.X + n.X, p.Y + n.Y))
                        .Where(po => !pointsVisited.Contains(po) && r.Contains(po))
                        .Distinct()
                        .Select(po => new
                        {
                            po,
                            onedge= po.X == loc.X || po.X ==far.X || po.Y==loc.Y || po.Y== far.Y,
                            dists = points.Select(p => new { p, md = Math.Abs(p.X - po.X) + Math.Abs(p.Y - po.Y) })
                                      .OrderBy(x => x.md)
                                      .Take(2)
                                      .ToArray()
                        })
                       .Select(x=> new {x.po, x.onedge, nearest= x.dists[0].md != x.dists[1].md ? x.dists[0].p : (Point?)null })
                       .ToArray();
                    pDists.Where(x=>x.nearest.HasValue).ToList().ForEach(
                                x => 
                                {
                                    if(areas[x.nearest.Value].HasValue)
                                    {
                                        areas[x.nearest.Value] = x.onedge ? null : areas[x.nearest.Value]+1;
                                    }
                                }                                    
                             );
                    pDists.Select(x => x.po).ForEach(x=>pointsVisited.Add(x));
                    added = pDists.Any();
                } while (added);

                int maxArea = areas.Max(kvp => kvp.Value).Value;
                return maxArea;
            }
        }

        public static int Day5b
        {
            get
            {
                const char nullchar = '0';
                string poly = File.ReadAllText("input5.txt").Trim();
                var dchars = poly.ToLower().Distinct();                
                int minLReduced=poly.Length;                                  
                foreach(char remchar in dchars)
                {
                    StringBuilder sb= new StringBuilder(poly);
                    sb.Replace(remchar, nullchar);
                    sb.Replace(char.ToUpper(remchar), nullchar);
                    bool replaced;
                    do
                    {
                        replaced = false;
                        int? ci = null;
                        for (int i = 0; i < sb.Length; i++)
                        {
                            if (sb[i] == nullchar) continue;
                            if (!ci.HasValue)
                                ci = i;
                            else
                            {
                                char c1 = sb[ci.Value];
                                char c2 = sb[i];
                                if (char.IsLower(c1) != char.IsLower(c2) && char.ToLower(c1) == char.ToLower(c2))
                                {
                                    sb[ci.Value] = sb[i] = nullchar;
                                    replaced = true;
                                    ci = null;

                                }
                                else ci = i;
                            }
                        }
                    } while (replaced);
                    int lreduced = sb.ToString().Count(c => c != nullchar);
                    minLReduced = Math.Min(lreduced, minLReduced);
                }
                return minLReduced;
            }
        }

        
        public static int Day5a
        {
            get
            {
                const char nullchar = '0';
                string poly = File.ReadAllText("input5.txt").Trim();             
                StringBuilder sb= new StringBuilder(poly);
                bool replaced;
                do
                {
                    replaced = false;
                    int? ci = null;
                    for (int i = 0; i < sb.Length; i++)
                    {
                        if (sb[i] == nullchar) continue;
                        if (!ci.HasValue)
                            ci = i;
                        else
                        {
                            char c1 = sb[ci.Value];
                            char c2 = sb[i];
                            if (char.IsLower(c1) != char.IsLower(c2) && char.ToLower(c1) == char.ToLower(c2))
                            {
                                sb[ci.Value] = sb[i] = nullchar;
                                replaced = true;
                                ci = null;

                            }
                            else ci = i;
                        }
                    }                    
                } while (replaced);
                return sb.ToString().Count(c=>c!=nullchar);
            }
        }
        
        public static int Day4b
        {
            get
            {
                const string guardkey = "Guard";
                const string sleepkey = "falls asleep";
                const string awakekey = "wakes up";
                string[] keys= new []{guardkey,sleepkey,awakekey};
                
                var source = File.ReadLines("input4.txt")
                    .OrderBy(l=>DateTime.Parse(l.Substring(1,16)))                   
                    .ToObservable();
                                                                                
                var sharedSource = source.Buffer(2,1).Publish().RefCount();
                var closingSignal = sharedSource
                    .Where(x => x.Last().Contains(guardkey));                                               

                var rx = sharedSource.Select(l=>l.First())
                    .Window(() => closingSignal)
                    .Select(o => o.Aggregate
                    (
                        new {gid=string.Empty, sleepmins = new int[60], timestamp = default(DateTime)},
                        (acc, l) =>
                        {
                            string key = keys.FirstOrDefault(l.Contains);
                            DateTime ts = DateTime.Parse(l.Substring(1, 16));
                            switch (key)
                            {
                                case guardkey:
                                {
                                    string gid_parsed= l.Split(' ').First(s => s.StartsWith('#'));
                                    return new {gid = gid_parsed, acc.sleepmins, timestamp = ts};
                                }
                                case sleepkey:
                                    return new {acc.gid, acc.sleepmins, timestamp = ts};
                                case awakekey:
                                    for (int i = acc.timestamp.Minute; i < ts.Minute; i++)
                                    {
                                        acc.sleepmins[i] = 1;
                                    }                                            
                                    return new{acc.gid, acc.sleepmins, timestamp = ts};
                            }
                            return null;
                        }
                    ))
                    .Switch();
                                
                var sleepiest=
                    rx.ToEnumerable()
                      .GroupBy(x => x.gid)
                      .Select(g=>new {g.Key, sleepmins=g.Aggregate(new int[60].AsEnumerable(), (acc, sm) => acc.Zip(sm.sleepmins, (x, y) => x + y))})
                      .MaxBy(x=>x.sleepmins.Max())
                      .First()
                    ;

                int gid = int.Parse(sleepiest.Key.TrimStart('#'));
                int  sleepiestMin=sleepiest.sleepmins
                    .Select((m,i)=>new {m, i})
                    .MaxBy(x=>x.m)
                    .First().i;
                return gid*sleepiestMin;
            }
        }

        public static int Day4a
        {
            get
            {
                const string guardkey = "Guard";
                const string sleepkey = "falls asleep";
                const string awakekey = "wakes up";
                string[] keys= new []{guardkey,sleepkey,awakekey};
                
                var source = File.ReadLines("input4.txt")
                    .OrderBy(l=>DateTime.Parse(l.Substring(1,16)))                   
                    .ToObservable();
                                                                                
                var sharedSource = source.Buffer(2,1).Publish().RefCount();
                var closingSignal = sharedSource
                    .Where(x => x.Last().Contains(guardkey));                                               

                var rx = sharedSource.Select(l=>l.First())
                    .Window(() => closingSignal)
                    .Select(o => o.Aggregate
                    (
                        new {gid=string.Empty, sleepmins = new int[60], timestamp = default(DateTime)},
                        (acc, l) =>
                        {
                            string key = keys.FirstOrDefault(l.Contains);
                            DateTime ts = DateTime.Parse(l.Substring(1, 16));
                            switch (key)
                            {
                                case guardkey:
                                {
                                    string gid_parsed= l.Split(' ').First(s => s.StartsWith('#'));
                                    return new {gid = gid_parsed, acc.sleepmins, timestamp = ts};
                                }
                                case sleepkey:
                                    return new {acc.gid, acc.sleepmins, timestamp = ts};
                                case awakekey:
                                    for (int i = acc.timestamp.Minute; i < ts.Minute; i++)
                                    {
                                        acc.sleepmins[i] = 1;
                                    }                                            
                                    return new{acc.gid, acc.sleepmins, timestamp = ts};
                            }
                            return null;
                        }
                    ))
                    .Switch();
                                
                var sleepiest=
                    rx.ToEnumerable()
                      .GroupBy(x => x.gid)
                      .MaxBy(g => g.SelectMany(gg => gg.sleepmins).Sum())
                      .First();

                int gid = int.Parse(sleepiest.Key.TrimStart('#'));
                int  sleepiestMin=sleepiest
                    .Aggregate(new int[60].AsEnumerable(), (acc, sm) => acc.Zip(sm.sleepmins, (x, y) => x + y))
                    .Select((m,i)=>new {m, i})
                    .MaxBy(x=>x.m)
                    .First().i;
                return gid*sleepiestMin;
            }
        }

        public static int Day3a
        {
            get
            {
                var rects = File.ReadLines("input3.txt")
                    .Select(l =>
                        l.Replace(" ", string.Empty)
                            .Split(new[] {'#', '@', ':', ',', 'x'}, StringSplitOptions.RemoveEmptyEntries))
                    .Select(lp=>lp.Select(int.Parse).ToArray())
                    .Select(lp => new {ID = lp[0], R=new Rectangle(lp[1],lp[2],lp[3],lp[4])})
                    .ToArray();
                 
                /**/
                int numofpixels = (from x in Enumerable.Range(0, 1000)
                    from y in Enumerable.Range(0, 1000)
                    from r in rects
                    where r.R.Contains(x, y)
                    group r by new {x, y} into g
                    select g).Count(g=>g.Count() > 1);

                return numofpixels;
            }
        }

        public static int Day3b
        {
            get
            {
                var rects = File.ReadLines("input3.txt")
                    .Select(l =>
                        l.Replace(" ", string.Empty)
                            .Split(new[] { '#', '@', ':', ',', 'x' }, StringSplitOptions.RemoveEmptyEntries))
                    .Select(lp => lp.Select(int.Parse).ToArray())
                    .Select(lp => new { ID = lp[0], R = new Rectangle(lp[1], lp[2], lp[3], lp[4]) })
                    .ToArray();

                var island=rects.First(r => !rects.Any(r2 => r2.R.IntersectsWith(r.R) && r2.ID != r.ID));                                              
                return island.ID;
            }
        }

        public static int Day2a
        {
            get
            {
                var countpair = File.ReadLines("input2.txt")
                    .Select(l => l.GroupBy(c => c))
                    .Select(gs => new { g2 = gs.Any(g => g.Count() == 2) ? 1 : 0, g3 = gs.Any(g => g.Count() == 3) ? 1 : 0 })
                    .Aggregate((acc, x) => new { g2 = acc.g2 + x.g2, g3 = acc.g3 + x.g3 });
                return countpair.g2 * countpair.g3;
            }
        }

        public static string Day2b
        {
            get
            {
                var lines = File.ReadLines("input2.txt").ToArray();
                var pair = (from l1 in lines
                        from l2 in lines
                        select
                            new {l1, l2, d = Fastenshtein.Levenshtein.Distance(l1, l2)}
                    ).First(x => x.d == 1);
                return new string(pair.l1.Where((c, i) => c == pair.l2[i]).ToArray());
            }
        }

        public static int Day1a => File.ReadLines("input1.txt").Select(int.Parse).Sum();

        public static int? Day1b => File.ReadLines("input1.txt").Select(int.Parse)
            .Cycle()
            .Scan(new { sum = 0, added = true, sumList = new HashSet<int>() },
                (acc, item) =>
                {
                    int newsum = acc.sum + item;
                    bool added = acc.sumList.Add(newsum);
                    return new { sum = newsum, added, acc.sumList };
                }
            )
            .First(x => !x.added).sum;

    }
}
