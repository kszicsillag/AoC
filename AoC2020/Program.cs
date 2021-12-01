using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2020
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(await Day1aAsync());
            //Console.WriteLine(await Day1bAsync());
            //Console.WriteLine(await Day2aAsync());
            //Console.WriteLine(await Day3aAsync());
            //Console.WriteLine(await Day3bAsync());
            //Console.WriteLine(Day4a());
            //Console.WriteLine(Day4b());
            //Console.WriteLine(Day5a());
            //Console.WriteLine(Day5b());
            Console.WriteLine(Day6b());
            Console.WriteLine("Hello World!");
        }

        private static int Day6a()
        {
            var lines= File.ReadAllLines("input/day6.txt");
            HashSet<char> chars= new HashSet<char>();
            int r=0;
            foreach(string l in lines)
            {
                if(l.Length==0)
                {   
                    r+=chars.Count;
                    chars.Clear();
                }
                else
                {
                    chars.UnionWith(l);
                }
            }
            return r+chars.Count;
        }

        private static int Day6b()
        {
            var lines= File.ReadAllLines("input/day6.txt");
            HashSet<char> defaultSet= new HashSet<char>{'0'};
            HashSet<char> chars= new HashSet<char>{'0'};
            int r=0;
            foreach(string l in lines)
            {
                if(l.Length==0)
                {   
                    r+=chars.Count;
                    chars.Clear();
                    chars.UnionWith(defaultSet);                    
                }
                else
                {
                    if(chars.SetEquals(defaultSet))
                    {
                        chars.Clear();
                        chars.UnionWith(l);
                    }
                    else chars.IntersectWith(l);
                }
            }
            return r+chars.Count;
        }

        private static int Day5a()
        {
            return File.ReadAllLines("input/day5.txt").Max(l=>Convert.ToInt32(l.Replace('F','0').Replace('B','1').Replace('R','1').Replace('L','0'),2));
        }

        private static int Day5b()
        {
            var ids = File.ReadAllLines("input/day5.txt")
                        .Select(l=>Convert.ToInt32(l.Replace('F','0').Replace('B','1').Replace('R','1').Replace('L','0'),2));
            var minid=ids.Min();
            return Enumerable.Range(minid,ids.Max()-ids.Min()).Except(ids).Single();
        }

        private static int Day4b()
        {
            Regex hcrg = new Regex(@"[0-9a-f]{6}");
            Regex pprg = new Regex(@"\d{9}");
            string[] ecls = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            var exp = File.ReadAllLines("input/day4.txt");
            HashSet<string> allKeys = new HashSet<string> { "byr", "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" };
            int lc = 0, gc = 0;

            do
            {
                HashSet<string> keyset = new HashSet<string>();
                bool isValid = true;
                while (lc < exp.Length && exp[lc].Length > 0)
                {
                    if (isValid)
                    {
                        var kys = exp[lc].Split(' ').Select(kvps => kvps.Split(':')).Select(x => new KeyValuePair<string, string>(x[0], x[1]));
                        foreach (KeyValuePair<string, string> kvp in kys)
                        {
                            Func<string, bool> isValidF = kvp.Key switch
                            {
                                "byr" => (v) => int.TryParse(v, out int vi) ? vi >= 1920 && vi <= 2002 : false,
                                "iyr" => (v) => int.TryParse(v, out int vi) ? vi >= 2010 && vi <= 2020 : false,
                                "eyr" => (v) => int.TryParse(v, out int vi) ? vi >=2020 && vi <= 2030 : false,
                                "hgt" => (v) =>
                                {
                                    if(v.Length<3) return false;
                                    if(!int.TryParse(v.Substring(0, v.Length - 2), out int hgtv)) return false;
                                    string hgtu = v.Substring(v.Length - 2);
                                    if (hgtu == "cm")
                                        return hgtv >= 150 && hgtv <= 193;
                                    else if (hgtu == "in")
                                        return hgtv >= 59 && hgtv <= 76;
                                    return false;
                                }
                                ,
                                "hcl" => (v) => v.Length == 7 && hcrg.IsMatch(v),
                                "ecl" => (v) => ecls.Contains(v),
                                "pid" => (v) => v.Length == 9 && pprg.IsMatch(v),
                                "cid" => (v) => true,
                                _ => (v) => false
                            };
                            isValid = isValidF(kvp.Value);
                            if (!isValid)
                                break;
                        }
                        keyset.UnionWith(kys.Select(x => x.Key));
                    }
                    lc++;
                }
                if(isValid)
                {
                    var exc = allKeys.Except(keyset).ToArray();
                    if (exc.Length == 0 || (exc.Length == 1 && exc.First() == "cid"))
                        gc++;
                }
                lc++;
            } while (lc < exp.Length);
            return gc;
        }

        private static int Day4a()
        {
            var exp = File.ReadAllLines("input/day4.txt");
            HashSet<string> allKeys = new HashSet<string> { "byr", "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" };
            int lc = 0, gc = 0;
            do
            {
                HashSet<string> keyset = new HashSet<string>();
                while (lc < exp.Length && exp[lc].Length > 0)
                {
                    keyset.UnionWith(exp[lc].Split(' ').Select(kvps => kvps.Split(':')[0]));
                    lc++;
                }
                var exc = allKeys.Except(keyset).ToArray();
                if (exc.Length == 0 || (exc.Length == 1 && exc.First() == "cid"))
                    gc++;
                lc++;
            } while (lc < exp.Length);
            return gc;
        }

        private static int Day3a()
        {
            var exp = File.ReadAllLines("input/day3.txt");
            int cols = exp[0].Length;
            int x = 0, y = 0, treecount = 0;
            do
            {
                treecount += exp[y][x] == '#' ? 1 : 0;
                x = (x + 3) % cols;
                y++;
            } while (y < exp.Length);
            return treecount;
        }

        private static long Day3b()
        {
            var exp = File.ReadAllLines("input/day3.txt").ToArray();
            int cols = exp[0].Length;
            (int dx, int dy)[] deltas = { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
            long tcpr = 1;
            foreach (var d in deltas)
            {
                int x = 0, y = 0, treecount = 0;
                do
                {
                    treecount += exp[y][x] == '#' ? 1 : 0;
                    x = (x + d.dx) % cols;
                    y += d.dy;
                } while (y < exp.Length);
                tcpr *= treecount;
            }
            return tcpr;
        }

        private static int Day2a()
        {
            Regex rgx = new Regex(@"(\d+)-(\d+) (\w): (\w+)");
            var r = File.ReadAllLines("input/day2.txt")
            .Select(l => rgx.Match(l))
            .Select(m => new { m, cc = m.Groups[4].Value.Count(c => c == m.Groups[3].Value.First()) })
            .Count(x => x.cc >= int.Parse(x.m.Groups[1].Value) && x.cc <= int.Parse(x.m.Groups[2].Value));

            return r;
        }

        private static int Day2b()
        {
            Regex rgx = new Regex(@"(\d+)-(\d+) (\w): (\w+)");
            var r = File.ReadAllLines("input/day2.txt")
            .Select(l => rgx.Match(l))
            .Count(x => (x.Groups[4].Value[int.Parse(x.Groups[1].Value) - 1] == x.Groups[3].Value[0]) ^ (x.Groups[4].Value[int.Parse(x.Groups[2].Value) - 1] == x.Groups[3].Value[0]));
            return r;
        }

         private static int Day1a()
        {
            var exp = File.ReadAllLines("input/day1.txt").Select((l, i) => new { i, E = int.Parse(l) }).ToArray();
            var r = from e1 in exp
                    from e2 in exp
                    where e2.i > e1.i && (e1.E + e2.E == 2020)
                    select e1.E * e2.E;
            return r.First();
        }

        private static int Day1b()
        {
            var exp = File.ReadAllLines("input/day1.txt").Select((l, i) => new { i, E = int.Parse(l) }).ToArray();
            var r = from e1 in exp
                    from e2 in exp
                    from e3 in exp
                    where e2.i > e1.i && e3.i > e2.i && (e1.E + e2.E + e3.E == 2020)
                    select e1.E * e2.E * e3.E;
            return r.First();
        }

    }
}
