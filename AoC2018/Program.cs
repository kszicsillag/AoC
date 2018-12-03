using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            Console.WriteLine($"3b:{Day3b}");


            Console.ReadLine();
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
