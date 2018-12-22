using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
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

    internal class Day10DO
    {
       
        public Day10Point Loc { get;  }

        public Point V { get; }

        public void Shift()
        {            
            Loc.Shift(V);
        }

        public Day10DO(Day10Point loc, Point v)
        {
            Loc = loc;
            V=v;
        }

       
    }

    internal class Day10Point
    {
        public int X { get; private set; }

        public int Y { get; private set; }

     
        public Day10Point(int x, int y)
        {
            X = x;
            Y = y;            
        }

        public void Shift(Point p)
        {
            Offset(p);
        }

        public void Offset(Point p)
        {
            X += p.X;
            Y += p.Y;
        }

    }


}
