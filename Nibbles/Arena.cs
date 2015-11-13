using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibbles
{
    public class Arena
    {
        private ConsoleColor[,] _arena;
        public const int WindowWidth = 80;
        public const int WindowHeight = 25;
        private const char ArenaBlock = '█';
        private ConsoleColor _backgroundColor;

        public ConsoleColor BackgroundColor { get { return _backgroundColor; } }

        public Arena(ConsoleColor[,] arena, ConsoleColor backgroundColor)
        {
            if (arena.GetLength(0) != WindowWidth || arena.GetLength(1) != WindowHeight)
                throw new ArgumentException("The arena must be an array that is 80x50.");
            _backgroundColor = backgroundColor;

            _arena = arena;
        }

        public Arena(int[,] arena, ConsoleColor backgroundColor)
        {
            if (arena.GetLength(0) != WindowWidth || arena.GetLength(1) != WindowHeight)
                throw new ArgumentException("The arena must be an array that is 80x50.");

            _arena = new ConsoleColor[WindowWidth, WindowHeight];
            _backgroundColor = backgroundColor;

            for (int x = 0; x < WindowWidth; x++)
                for (int y = 0; y < WindowHeight; y++)
                    _arena[x, y] = (ConsoleColor)arena[x, y];
        }

        public static Arena FromFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            if (lines.Length != WindowHeight + 1)
                throw new ArgumentException("The arena file does not contain enough data for an arena.");

            int[,] arena = new int[WindowWidth, WindowHeight];
            int y = 0;

            string line0 = lines[0];
            var arenaLines = new string[lines.Length - 1];
            Array.Copy(lines, 1, arenaLines, 0, arenaLines.Length);

            foreach (var line in arenaLines)
            {
                var values = line.Split(',');
                if (values.Length != WindowWidth)
                    throw new ArgumentException("The arena file does not contain enough data for an arena.");

                for (int x = 0; x < WindowWidth; x++)
                    arena[x, y] = int.Parse(values[x]);
                y++;
            }

            return new Arena(arena, (ConsoleColor)int.Parse(line0));
        }

        public void DrawArena()
        {
            for (int y = 0; y < WindowHeight; y++)
            {
                Console.SetCursorPosition(0, y);
                for (int x = 0; x < WindowWidth; x++)
                {
                    Console.ForegroundColor = _arena[x, y];
                    Console.Write(ArenaBlock);
                }
            }
            Console.SetCursorPosition(0, 0);
        }

        public bool HasBlockAt(Point p)
        {
            return _arena[p.X, p.Y] != _backgroundColor;
        }
    }
}
