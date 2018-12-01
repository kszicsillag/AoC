using System;
using System.Collections.Generic;
using System.Linq;
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
        }
    }
}
