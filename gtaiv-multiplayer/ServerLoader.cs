using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIVServer
{
    class ServerLoader
    {
        public static void Main(string[] args)
        {
            var server = new Server(9999);
            while (Console.ReadLine() != "c") ;
        }
    }
}
