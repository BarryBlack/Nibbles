using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nibbles
{
    class Program
    {
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Level lvl0 = new Level("Level1.lvl")
            {
                NumPlayers = 1,
                IncreaseSpeed = true,
                MaxScore = 100,
            };

            lvl0.Play();

            Console.ReadKey(true);
        }

        private static Point GenerateFoodPoint(Arena arena, Snake[] snakes)
        {
            Point p = new Point() { X = rnd.Next(Arena.WindowWidth), Y = rnd.Next(Arena.WindowHeight) };

            while (arena.HasBlockAt(p) ||
                   snakes.Any(sn => sn.BodyCollides(p)))
            {
                p = new Point() { X = rnd.Next(Arena.WindowWidth), Y = rnd.Next(Arena.WindowHeight) };
            }

            return p;
        }
    }
}
