using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safina
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Game game = new Game();
            game.StartGame();
            Console.ReadKey();
        }
        public static void WriteAnimated(string text, int delay = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
        }
    }
}
