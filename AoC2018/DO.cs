using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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



    internal class Day13DO
    {
        public const char map_h = '-', map_v = '|', map_corner1 = '/', map_corner2 = '\\', map_intersect='+';
        public const char cart_l = '<', cart_r = '>', cart_u = '^', cart_d = 'v';
        static readonly Dictionary<char,char> cornerMap1=new Dictionary<char, char>
            {{cart_r,cart_u},{cart_l,cart_d},{cart_d,cart_l},{cart_u, cart_r}};
        static readonly Dictionary<char, char> cornerMap2 = new Dictionary<char, char>
            {{cart_r,cart_d},{cart_l,cart_u},{cart_d,cart_r},{cart_u, cart_l}};

        static readonly Dictionary<char, char> intersectMap0_left = new Dictionary<char, char>
            {{cart_r,cart_u},{cart_l,cart_d},{cart_d,cart_r},{cart_u, cart_l}};
        static readonly Dictionary<char, char> intersectMap1_right = new Dictionary<char, char>
            {{cart_r,cart_d},{cart_l,cart_u},{cart_d,cart_l},{cart_u, cart_r}};

        private static readonly Dictionary<short, Dictionary<char, char>> intersectMap =
            new Dictionary<short, Dictionary<char, char>> {{0, intersectMap0_left}, {2, intersectMap1_right}};


        public static readonly char[] cart_tiles = {cart_l, cart_r, cart_u, cart_d};
        public static readonly char[] map_tiles = { map_h, map_v, map_corner1, map_corner2, map_intersect };

        private static char[,] Map;

        public int X { get; set; }

        public int Y { get; set; }

        public char D { get; set; }

        public bool Crashed { get; private set; } = false;

        private short intersectCnt=-1;

        private readonly (int x, int y, char d) initialData;

        private List<(int x, int y, char d)> debugSteps= new List<(int x, int y, char d)>();

        public char InitialTileUnder
        {
            get
            {
                switch (initialData.d)
                {
                    case cart_l:
                    case cart_r:
                        return map_h;
                    case cart_u:
                    case cart_d:
                        return map_v;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
       

        public Day13DO(int x, int y, char d, char[,] map)
        {
            X = x;
            Y = y;
            D = d;
            initialData = (x, y, d);
            Map = map;
        }

        public void Step()
        {            
            switch (D)
            {
                case cart_l:
                    X -= 1;
                    break;
                case cart_r:
                    X += 1;
                    break;
                case cart_u:
                    Y -= 1;
                    break;
                case cart_d:
                    Y += 1;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            if(!map_tiles.Contains(Map[X,Y]))
                throw new InvalidOperationException();
            if (Map[X,Y] == map_corner1) D=cornerMap1[D];
            else if (Map[X, Y] == map_corner2) D = cornerMap2[D];
            else if (Map[X, Y] == map_intersect)
            {
                intersectCnt = (short)((intersectCnt + 1) % 3);                      
                if(intersectCnt != 1)
                    D = intersectMap[intersectCnt][D];
            }
            debugSteps.Add((X,Y,D));
        }

        public bool IsCollide(Day13DO other)
        {
            bool retVal= (X == other.X && Y == other.Y) && (initialData.x != other.initialData.x || initialData.y != other.initialData.y);
            if (retVal)
            {
                Crashed= other.Crashed = true;
            }
            return retVal;
        }

        public override string ToString() => $"{X},{Y}:{D}";
    }
}
