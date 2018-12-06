using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchmarkApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** BenchmarkApp 0.1, Michal Kratky, March 2011 *****");
            int numberOfTestThreads = 1;
            new BenchmarkApp().Run(numberOfTestThreads);
        }
    }
}
