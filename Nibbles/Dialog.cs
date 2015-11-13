using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nibbles
{
    public class Dialog
    {

        private const char Block = '█';

        public static void Show(string text)
        {
            int dialogWidth = text.Length + 10;
            var lines = text.Split('\n');
            int dialogHeight = lines.Length + 4;
            
            int linenum = 0;
            int y = 12 - dialogHeight;
            int x = 40 - dialogWidth / 2;

            for (int i = 0; i < dialogHeight; i++)
            {
                if (i == 0 || i == dialogHeight - 1)
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(new string(Block, dialogWidth));
                    y++;
                }
                else
                {
                    if (i < 2 || i >= 3)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(Block);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(new string(Block, dialogWidth - 2));
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(Block);
                        y++;
                    }
                    else
                    {
                        Console.SetCursorPosition(x, y);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(Block);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("    " + text + "    ");
                        Console.Write(Block);
                        y++;
                    }
                    linenum++;
                }
            }

            while (!Console.KeyAvailable)
            {
                Thread.Sleep(10);
            }
            Console.ReadKey(true);
        }
    }
}
