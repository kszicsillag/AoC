using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AoC2018
{
    static class Utils
    {
        /*https://codereview.stackexchange.com/a/92070*/
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> list, int index = 0)
        {

            var count = list.Count();
            index = index % count;

            while (true)
            {
                yield return list.ElementAt(index);
                index = (index + 1) % count;
            }
            // ReSharper disable once IteratorNeverReturns
        }


        public static IEnumerable<int> Digits(this int number)
        {
            do
            {
                yield return number % 10;
                number /= 10;
            } while (number > 0);
        }
      



    }
}
