using System;

namespace MIVServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new Server(9999);
            while (Console.ReadLine() != "c") ;
        }
    }
}