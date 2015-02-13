using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            decimal a = 1238432819529019075093281m;
            decimal b = 297545698622333647657432m;
            //uint a = 983747234;
            //uint b = 324234237;
            decimal c = a - b;
            Console.WriteLine(c);
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);
            Console.Read();
        }
    }
}
