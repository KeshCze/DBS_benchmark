using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using AspNetExampleApp.Database;

namespace BenchmarkApp
{
    class BenchmarkApp
    {

        public BenchmarkApp()
        {
        }

        public void Run(int threadCount)
        {
            DatabasePool databasePool = new DatabasePool(threadCount);
            databasePool.Connect();
            QuerySetPool queryPool = new QuerySetPool(databasePool);

            if (!queryPool.ReadFromFile("query.txt"))
            {
                return;
            }

            // create instances of test classes
            ThreadTest[] threadTests = new ThreadTest[threadCount];
            for (int i = 0 ; i < threadCount ; i++)
            {
                threadTests[i] = new ThreadTest(i, queryPool.GetQuerySet(i), queryPool);
            }

            // create instances of Thread class
            Thread[] threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(threadTests[i].Run);
            }


            Stopwatch sw = new Stopwatch();
            sw.Start();

            // start all test threads
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Start();
            }

            // Spin for a while waiting for the started thread to become alive:
            for (int i = 0; i < threadCount; i++)
            {
                while (!threads[i].IsAlive) ;
            }
            
            // Wait until oThread finishes. Join also has overloads
            // that take a millisecond interval or a TimeSpan object.
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }

            string executionTimeTaken = string.Format("\nTime: {0}s\n", sw.Elapsed.TotalMilliseconds/1000);
            Console.WriteLine(executionTimeTaken);
            databasePool.Close();

            Console.WriteLine("Done.");
        }
    }
}
