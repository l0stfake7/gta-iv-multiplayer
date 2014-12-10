using System;

namespace MIVServer
{
    internal class ServerLoader
    {
        public static void Main(string[] args)
        {
            var server = new Server();
            while (Console.ReadLine() != "c") ;
        }
    }
}