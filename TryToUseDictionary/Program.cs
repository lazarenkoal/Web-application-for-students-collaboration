using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryToUseDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> Q = new Dictionary<string, int>();
            Q.Add("lal", 40);
            Console.WriteLine(Q["lal"]);
            Q["lal"] = 50;
            Console.WriteLine(Q["lal"]);
            Console.ReadKey();
        }
    }
}
