using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2018
{
    class Day7DO
    {
        public char Task { get; set; }

        public int State { get; set; }

        public int Goal { get; set; }

        public int Worked { get; set; }

        public override string ToString() => $"{nameof(Task)}:{Task},{nameof(State)}:{State},{nameof(Goal)}:{Goal},{nameof(Worked)}:{Worked},";

    }
}
