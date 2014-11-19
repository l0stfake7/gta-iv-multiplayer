using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIVServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server(9999);
            while (Console.ReadLine() != "c") ;
        }
    }
}
