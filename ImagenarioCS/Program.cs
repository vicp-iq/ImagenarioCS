using System;

namespace ImagenarioCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SCXManager.load(@"Inputs/Raw.scx");
        }
    }
}
