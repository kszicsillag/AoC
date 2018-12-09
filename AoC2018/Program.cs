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
            Console.WriteLine($"5a:{Day5a}");
            Console.WriteLine($"5b:{Day5b}");

            //Console.ReadLine();
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
