using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibbles
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static bool operator ==(Point left, Point right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Point left, Point right)
        {
            return left.X != right.X || left.Y != right.Y;
        }
    }
}
