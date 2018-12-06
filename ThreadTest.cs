using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspNetExampleApp.Database;

namespace BenchmarkApp
{
    class ThreadTest
    {
        QuerySet mQuerySet;
        QuerySetPool mQuerySetPool;
        int mThreadId;

        public ThreadTest(int threadId, QuerySet querySet, QuerySetPool querySetPool)
        {
            mQuerySet = querySet;
            mThreadId = threadId;
            mQuerySetPool = querySetPool;
        }

        public void Run()
        {
            Console.Write("#" + mThreadId + " : Start \t");

            Random rnd = new Random(mThreadId);
            int i = 0;

            while (true)
            {
                int index, queryNumber;
                DbTable tableObject = mQuerySet.GetTableObject(out index); // get table object of a random query

                // check when the query be executed
                if (mQuerySetPool.CanIExecuteQuery(index, out queryNumber) == false)
                {
                    continue;
                }

                // use the instance to call the method
                // prepare parameters
                int count = mQuerySetPool.GetMethodParameters(index).Length;
                object[] parameters = new object[count];
                for (int j = 0; j < count; j++)
                {
                    if (mQuerySetPool.GetMethodParameters(index)[j] == System.Type.GetType("System.Int32"))
                    {
                        parameters[j] = rnd.Next(QuerySetPool.INT_MAX);
                    }
                }
                mQuerySetPool.GetQueryMethod(index).Invoke(tableObject, parameters);

                // print only sometimes, for performance reason
                if (i++ % 10 == 0)
                {
                    Console.Write("#" + mThreadId + " (" + index + "): " + queryNumber + "\t");
                }

                if (mQuerySetPool.CanIFinish())
                {
                    break;
                }
            }
            Console.Write("#" + mThreadId + " : End \t");
        }
    }
}
