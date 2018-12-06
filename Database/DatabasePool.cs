using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetExampleApp.Database
{
    class DatabasePool
    {
        Database[] mDatabases;

        public DatabasePool(int number)
        {
            mDatabases = new Database[number];
            for (int i = 0; i < number; i++)
            {
                mDatabases[i] = new Database();
            }
        }

        public void Close()
        {
            for (int i = 0; i < mDatabases.Length; i++)
            {
                mDatabases[i].Close();
            }
        }

        public void Connect()
        {
            for (int i = 0; i < mDatabases.Length; i++)
            {
                mDatabases[i].Connect();
            }
        }

        public int Count()
        {
            return mDatabases.Length;
        }

        public Database GetDatabase(int i)
        {
            return mDatabases[i];
        }
    }
}
