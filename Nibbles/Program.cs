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
    }
}
