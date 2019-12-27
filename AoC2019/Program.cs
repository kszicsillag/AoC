using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Xml;
using MathNet.Numerics;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;

namespace AoC2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
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
            Console.WriteLine($"8b:{Day8b}");
            //Console.WriteLine($"9a:{Day9a}");
            //Console.WriteLine($"9b:{Day9b}");
            //Console.WriteLine($"10:{Day10}");
            //Console.WriteLine($"11a:{Day11a}");
            //Console.WriteLine($"11b:{Day11b}");
            //Console.WriteLine($"12a:{Day12a}");
            //Console.WriteLine($"12b:{Day12b}");
            //Console.WriteLine($"13a:{Day13a}");
            //Console.WriteLine($"13b:{Day13b}");
            //Console.WriteLine($"14a:{Day14a}");
            //Console.WriteLine($"14b:{Day14b}");
            Console.ReadLine();
        }

        public static int Day1a => 
            File.ReadLines("input1.txt").Select(int.Parse).Select(i=>((int)Math.Floor(i/3.0))-2).Sum();

        public static long Day1b
        {
            get
            {
                int[] masses = File.ReadLines("input1.txt").Select(int.Parse).ToArray();
                int cntr = 0;
                long totalFuel = 0;
                do
                {
                    int mass = masses[cntr];
                    do
                    {
                        int addFuel = (int) (Math.Floor(mass / 3.0)) - 2;
                        totalFuel += (addFuel < 0 ? 0 : addFuel);
                        mass = addFuel;
                    } while (mass > 0);
                    cntr++;
                } while (cntr < masses.Length);
                return totalFuel;
            }
        }

        public static long Day2a => Day2(12, 2);

        public static long Day2b
        {
            get
            {
                for (int n = 0; n < 100; n++)
                {
                    for (int v = 0; v < 100; v++)
                    {
                        if (Day2(n, v) == 19690720)
                            return n * 100 + v;
                    }
                }
                return -1;
            }
        }

        public static long Day3a
        {
            get
            {
                string[] _lines=File.ReadAllLines("input3.txt");
                List<Tuple<char, int>[]> directions = new List<Tuple<char, int>[]>();
                directions.Add(_lines[0].Split(',').Select(s=>new Tuple<char, int>(s[0], int.Parse(s.Substring(1))) ).ToArray());
                directions.Add(_lines[1].Split(',').Select(s => new Tuple<char, int>(s[0], int.Parse(s.Substring(1)))).ToArray());
                
                Point2D p0= new Point2D(0,0);
                List<List<LineSegment2D>> wires = new List<List<LineSegment2D>>();
                foreach (var d in directions)
                {
                    Point2D p = p0;
                    List<LineSegment2D> lines = new List<LineSegment2D>();
                    foreach (var (direction, distance) in d)
                    {
                        Point2D nextp =
                            direction switch
                            {
                                'R' => new Point2D(p.X + distance, p.Y),
                                'U' => new Point2D(p.X, p.Y + distance),
                                'L' => new Point2D(p.X - distance, p.Y),
                                'D' => new Point2D(p.X, p.Y - distance),
                                _ => throw new InvalidOperationException()
                            };
                        lines.Add(new LineSegment2D(p, nextp));
                        p = nextp;
                    }
                    wires.Add(lines);
                }

                var intersectPoints = (from l1 in wires[0]
                        from l2 in wires[1]
                        let isp= l1.TryIntersect(l2, out p0, Angle.FromDegrees(0.001))
                        where isp
                        select p0).ToArray();

                return (long)intersectPoints.Min(p => Distance.Manhattan(p.ToVector(), p0.ToVector()));
            }
        }


        public static long Day3b
        {
            get
            {
                string[] _lines = File.ReadAllLines("input3.txt");
                List<Tuple<char, int>[]> directions = new List<Tuple<char, int>[]>
                {
                    _lines[0].Split(',').Select(s => new Tuple<char, int>(s[0], int.Parse(s.Substring(1)))).ToArray(),
                    _lines[1].Split(',').Select(s => new Tuple<char, int>(s[0], int.Parse(s.Substring(1)))).ToArray()
                };

                Point2D p0 = new Point2D(0, 0);
                List<List<Tuple<LineSegment2D,long>>> wires = new List<List<Tuple<LineSegment2D, long>>>();
                foreach (var d in directions)
                {
                    long fulldistance = 0;
                    Point2D p = p0;
                    List<Tuple<LineSegment2D, long>> lines = new List<Tuple<LineSegment2D, long>>();
                    foreach (var (direction, distance) in d)
                    {
                        Point2D nextp =
                            direction switch
                            {
                                'R' => new Point2D(p.X + distance, p.Y),
                                'U' => new Point2D(p.X, p.Y + distance),
                                'L' => new Point2D(p.X - distance, p.Y),
                                'D' => new Point2D(p.X, p.Y - distance),
                                _ => throw new InvalidOperationException()
                            };
                        fulldistance += distance;
                        lines.Add(new Tuple<LineSegment2D, long>(new LineSegment2D(p, nextp), fulldistance));
                        p = nextp;
                    }
                    wires.Add(lines);
                }

                var intersectPoints = (from l1 in wires[0]
                                       from l2 in wires[1]
                                       let isp = l1.Item1.TryIntersect(l2.Item1, out p0, Angle.FromDegrees(0.001))
                                       where isp
                                       select new {l1, l2, pi=p0 }).ToArray();

                return (long)intersectPoints.Min(x => 
                    x.l1.Item2 - new LineSegment2D(x.pi,x.l1.Item1.EndPoint).Length +
                    x.l2.Item2 - new LineSegment2D(x.pi, x.l2.Item1.EndPoint).Length);
            }
        }

        public static long Day4a
        {
            get
            {
                int[] limits= File.ReadAllText("input4.txt").Split("-").Select(int.Parse).ToArray();
                
                return Enumerable.Range(limits[0], limits[1] - limits[0])
                    .Select(i => i.ToString().Buffer(2, 1).Where(b=>b.Count==2).ToArray())
                    .Count(isb =>
                        isb.Any(issb => issb[0] == issb[1]) &&
                        !isb.Any(issb => issb[0] > issb[1]));
            }
        }

        public static long Day4b
        {
            get
            {
                int[] limits = File.ReadAllText("input4.txt").Split("-").Select(int.Parse).ToArray();
                
                return Enumerable.Range(limits[0], limits[1] - limits[0])
                    .Select(i => i.ToString())
                    .Select(i => new { Buffers = i.Buffer(2, 1).Where(b => b.Count == 2).Select((isb, idx) => new { isb, idx }).ToArray(), Pw = i })
                    .Count(isb =>
                        isb.Buffers.Any(x => x.isb[0] == x.isb[1] && !((x.idx > 0 && isb.Pw[x.idx - 1] == x.isb[0]) || (x.idx < 4 && isb.Pw[x.idx + 2] == x.isb[0]))) &&
                        !isb.Buffers.Any(x => x.isb[0] > x.isb[1]));
            }
        }

        public static long Day5a => Day5(1);

        public static long Day5b => Day5(5);

        public static long Day6a
        {
            get
            {
                Dictionary<string, UniverseObject> uod= new Dictionary<string, UniverseObject>();
                var lines = File.ReadAllLines("input6.txt")
                    .Select(l=>l.Split(')'))
                    .Select(a => new { O1 = a[0], O2 = a[1] });
                
                foreach (var line in lines)
                {
                    if (!uod.TryGetValue(line.O1, out UniverseObject o1))
                    {
                        o1= new UniverseObject(line.O1);
                        uod.Add(o1.ID, o1);
                    }
                    if (!uod.TryGetValue(line.O2, out UniverseObject o2))
                    {
                        o2 = new UniverseObject(line.O2);
                        uod.Add(o2.ID, o2);
                    }
                    o2.OrbitParent = o1;
                }

                long cntr=0;
                foreach (UniverseObject o in uod.Values.Where(o=>o.OrbitParent != null))
                {
                    UniverseObject vo=o;
                    do
                    {
                        vo = vo.OrbitParent;
                        cntr++;
                    } while (vo.OrbitParent != null);
                }

                return cntr;
            }
        }


        public static long Day6b
        {
            get
            {
                Dictionary<string, UniverseObject> uod = new Dictionary<string, UniverseObject>();
                var lines = File.ReadAllLines("input6.txt")
                    .Select(l => l.Split(')'))
                    .Select(a => new { O1 = a[0], O2 = a[1] });

                foreach (var line in lines)
                {
                    if (!uod.TryGetValue(line.O1, out UniverseObject o1))
                    {
                        o1 = new UniverseObject(line.O1);
                        uod.Add(o1.ID, o1);
                    }
                    if (!uod.TryGetValue(line.O2, out UniverseObject o2))
                    {
                        o2 = new UniverseObject(line.O2);
                        uod.Add(o2.ID, o2);
                    }
                    o2.OrbitParent = o1;
                }

                List<List<UniverseObject>> lastpaths = new List<List<UniverseObject>> { new List<UniverseObject> { uod["YOU"] } };

                List<List<UniverseObject>> newpaths;

                do
                {
                    newpaths = new List<List<UniverseObject>>();
                    foreach (List<UniverseObject> path in lastpaths)
                    {
                        UniverseObject uo = path.Last();
                        if (uo.OrbitParent == uod["SAN"] || uo.OrbitChildren.Any(x => x.ID == "SAN"))
                            return path.Count-2;
                        if (uo.OrbitParent != null && !path.Contains(uo.OrbitParent))
                        {
                            List<UniverseObject> np = new List<UniverseObject>(path){uo.OrbitParent};
                            newpaths.Add(np);
                        }
                        foreach (UniverseObject child in uo.OrbitChildren.Where(x=>!path.Contains(x)))
                        {
                            List<UniverseObject> np = new List<UniverseObject>(path) { child };
                            newpaths.Add(np);
                        }
                    }
                    lastpaths = newpaths;
                } while (newpaths.Any());
                return -1;
            }
        }

        public static long Day7a
        {
            get
            {
                int maxio = 0;
                for (int i = 1234; i < 43210; i++)
                {
                    string ds = i.ToString("D5");
                    int io=0;
                    if (ds.Distinct().Count() == 5 && ds.Max(c=>(int)char.GetNumericValue(c)) < 5)
                    {
                        foreach (char t in ds)
                        {
                            io=Day7aComp(  (int)char.GetNumericValue(t), io);
                        }
                    }
                    if (io > maxio) maxio = io;
                }
                return maxio;
            }
        }

        public static long Day7b
        {
            get
            {
                int maxio = 0;
                for (int i = 56789; i < 98765; i++)
                {
                    string ds = i.ToString("D5");
                    if (ds.Distinct().Count() == 5 && ds.Min(c => (int)char.GetNumericValue(c)) == 5)
                    {
                        (int output, int? nip) outp = default;
                        int[] ios = ds.Select(c => (int) char.GetNumericValue(c)).ToArray();
                        int[][] states = Enumerable.Repeat(0,ds.Length).Select(x => (int[])day7Code.Value.Clone()).ToArray();
                        int?[] ips = Enumerable.Repeat<int?>(0, ds.Length).ToArray();
                        for (int j = 0; j < ds.Length; j++)
                        {
                            outp = Day7bComp(states[j], ips[j].Value, ios[j], j>0? ios[j-1] : 0);
                            ips[j] = outp.nip;
                            ios[j] = outp.output;
                        }

                        do
                        {
                            for (int j = 0; j < ds.Length; j++)
                            {
                                outp = Day7bComp(states[j], ips[j].Value, ios[j==0? ios.Length-1 : j-1]);
                                if (!outp.nip.HasValue)
                                    break;
                                ips[j] = outp.nip;
                                ios[j] = outp.output;
                            }
                        } while (outp.nip.HasValue);

                        if (ios[^1] > maxio)
                            maxio = ios[^1];
                    }
                }
                return maxio;
            }
        }

        public static long Day8a
        {
            get
            {
                string rawtext = File.ReadAllText("input8.txt");
                int chunkSize = 25 * 6;
                return Enumerable.Range(0, rawtext.Length / chunkSize)
                    .Select(i => rawtext.Substring(i * chunkSize, chunkSize))
                    .Select(ss=>new {ZC=ss.Count(c=>c=='0'), CheckSum= ss.Count(c => c == '1') * ss.Count(c => c == '2') })
                    .MinBy(x=>x.ZC)
                    .First().CheckSum;
            }
        }

        public static string Day8b
        {
            get
            {
                string rawtext = File.ReadAllText("input8.txt");
                const int width = 25;
                const int height = 6;
                int chunkSize = width * height;
                char[] imgChars = rawtext
                    .Select((c, idx) => new {C = c, Idx = idx})
                    .GroupBy(x => x.Idx % chunkSize)
                    .Select(g=>(g.FirstOrDefault(x=>x.C!='2')?.C) ?? '2').ToArray();
                int scale = 40;
                Bitmap img = new Bitmap((width+2)*scale, (height+2)*scale);
                Graphics imgg = Graphics.FromImage(img);
                imgg.FillRectangle(Brushes.Black, 0, 0, img.Width, img.Height);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Brush b = imgChars[(i*width)+j] switch
                        {
                            '1' => Brushes.White, //white
                            '0' => Brushes.Black, //black
                            _ => Brushes.Transparent
                        };
                        imgg.FillRectangle(b, (j+1) * scale, (i+1) * scale, scale, scale);
                    }
                }

                string filename = "aoc8b.png";
                img.Save(filename);
                ProcessStartInfo si = new ProcessStartInfo( filename){UseShellExecute = true};
                Process.Start(si);
                return "View the image";
            }
        }


        static readonly Lazy<int[]> day2Code = new Lazy<int[]>(
            () =>
            {
                string fullCode = File.ReadAllText("input2.txt");
                return fullCode.Split(',').Select(int.Parse).ToArray();
            });

        static readonly Lazy<int[]> day5Code = new Lazy<int[]>(
            () =>
            {
                string fullCode = File.ReadAllText("input5.txt");
                return fullCode.Split(',').Select(int.Parse).ToArray();
            });

        static readonly Lazy<int[]> day7Code = new Lazy<int[]>(
            () =>
            {
                string fullCode = File.ReadAllText("input7.txt");
                return fullCode.Split(',').Select(int.Parse).ToArray();
            });

        static long Day2(int noun, int verb)
        {
                int[] code = (int[])day2Code.Value.Clone();
                code[1] = noun;
                code[2] = verb;
                int i = 0;
                do
                {
                    switch (code[i])
                    {
                        case 99:
                            i = code.Length;
                            break;
                        case 1:
                            code[code[i + 3]] = code[code[i + 1]] + code[code[i + 2]];
                            break;
                        case 2:
                            code[code[i + 3]] = code[code[i + 1]] * code[code[i + 2]];
                            break;
                    }
                    i += 4;
                } while (i < code.Length);
                return code[0];
        }


        static long Day5(int input)
        {
            int[] code = (int[])day5Code.Value.Clone();
            int i = 0, dcode=0;
            string cString;

            int ind(int offset) => cString[cString.Length - 2 - offset] == '0' ? code[i + offset] : i + offset;

            do
            {
                cString = code[i].ToString("D5");
                switch (code[i] % 100)
                {
                    case 99:
                        i = code.Length;
                        break;
                    case 1:
                        code[code[i+3]] = code[ind( 1)] + code[ind( 2)];
                        i+= 4;
                        break;
                    case 2:
                        code[code[i + 3]] = code[ind( 1)] * code[ind( 2)];
                        i+= 4;
                        break;
                    case 3:
                        code[code[i + 1]] = input;
                        i += 2;
                        break;
                    case 4:
                        dcode = code[ind(1)];
                        i+= 2;
                        break;
                    case 5:
                        if (code[ind(1)] != 0){ i = code[ind(2)]; } else i += 3;
                        break;
                    case 6:
                        if (code[ind(1)] == 0) { i = code[ind(2)]; } else i += 3; 
                        break;
                    case 7:
                        code[code[i + 3]] = code[ind(1)] < code[ind(2)] ? 1 : 0;
                        i += 4;
                        break;
                    case 8:
                        code[code[i + 3]] = code[ind(1)] == code[ind(2)] ? 1 : 0;
                        i += 4;
                        break;
                }
            } while (i < code.Length);
            return dcode;
        }


        static int Day7aComp(params int[] input)
        {
            int[] code = (int[])day7Code.Value.Clone();
            int i = 0, dcode = 0;
            string cString;

            int ind(int offset) => cString[cString.Length - 2 - offset] == '0' ? code[i + offset] : i + offset;
            int iinput = 0;

            do
            {
                cString = code[i].ToString("D5");
                switch (code[i] % 100)
                {
                    case 99:
                        i = code.Length;
                        break;
                    case 1:
                        code[code[i + 3]] = code[ind(1)] + code[ind(2)];
                        i += 4;
                        break;
                    case 2:
                        code[code[i + 3]] = code[ind(1)] * code[ind(2)];
                        i += 4;
                        break;
                    case 3:
                        code[code[i + 1]] = input[iinput];
                        iinput++;
                        i += 2;
                        break;
                    case 4:
                        dcode = code[ind(1)];
                        i += 2;
                        break;
                    case 5:
                        if (code[ind(1)] != 0) { i = code[ind(2)]; } else i += 3;
                        break;
                    case 6:
                        if (code[ind(1)] == 0) { i = code[ind(2)]; } else i += 3;
                        break;
                    case 7:
                        code[code[i + 3]] = code[ind(1)] < code[ind(2)] ? 1 : 0;
                        i += 4;
                        break;
                    case 8:
                        code[code[i + 3]] = code[ind(1)] == code[ind(2)] ? 1 : 0;
                        i += 4;
                        break;
                }
            } while (i < code.Length);
            return dcode;
        }

        static (int, int?) Day7bComp(int[] code, int i, params int[] input)
        {
            //int[] code = (int[])day7Code.Value.Clone();
            string cString;

            int ind(int offset) => cString[cString.Length - 2 - offset] == '0' ? code[i + offset] : i + offset;
            int iinput = 0;

            do
            {
                cString = code[i].ToString("D5");
                switch (code[i] % 100)
                {
                    case 99:
                        i = code.Length;
                        break;
                    case 1:
                        code[code[i + 3]] = code[ind(1)] + code[ind(2)];
                        i += 4;
                        break;
                    case 2:
                        code[code[i + 3]] = code[ind(1)] * code[ind(2)];
                        i += 4;
                        break;
                    case 3:
                        code[code[i + 1]] = input[iinput];
                        iinput++;
                        i += 2;
                        break;
                    case 4:
                        var dcode = code[ind(1)];
                        i += 2;
                        return (dcode, i);
                    case 5:
                        if (code[ind(1)] != 0) { i = code[ind(2)]; } else i += 3;
                        break;
                    case 6:
                        if (code[ind(1)] == 0) { i = code[ind(2)]; } else i += 3;
                        break;
                    case 7:
                        code[code[i + 3]] = code[ind(1)] < code[ind(2)] ? 1 : 0;
                        i += 4;
                        break;
                    case 8:
                        code[code[i + 3]] = code[ind(1)] == code[ind(2)] ? 1 : 0;
                        i += 4;
                        break;
                }
            } while (i < code.Length);
            return (-1,null);
        }
    }
}
