using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using AspNetExampleApp.Database;

namespace BenchmarkApp
{
    class QuerySet
    {
        internal static int numberOfCalls = 0;
        Collection<DbTable> mTableObjects;
        Database mDatabase;
        Random mRandom = new Random();

        public QuerySet(Database database)
        {
            mTableObjects = new Collection<DbTable>();
            mDatabase = database;
        }

        public void AddTableObject(DbTable tableObject)
        {
            mTableObjects.Add(tableObject);
        }
        // Změna
        public DbTable GetTableObject(out int index)
        {
            ///int num = mRandom.Next(mTableObjects.Count);
            int num = mRandom.Next(numberOfCalls);
            index = num;
            return mTableObjects[num];
        }
    }
}
