using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace AoC2018
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"1a:{Day1a}");
            Console.WriteLine($"1b:{Day1b}");
            Console.ReadLine();
        }
       

        public static int Day1a => File.ReadLines("input1.txt").Select(int.Parse).Sum();

        public static int? Day1b => File.ReadLines("input1.txt").Select(int.Parse)
            .Cycle()
            .Scan(new {sum = 0, added = true, sumList = new HashSet<int>()}, 
                (acc, item) =>
                {
                    int newsum = acc.sum + item;
                    bool added = acc.sumList.Add(newsum);
                    return new {sum = newsum, added, acc.sumList};
                }
            )            
            .First(x=> !x.added).sum;

    }
}
