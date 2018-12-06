using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

using AspNetExampleApp.Database;
using ORM.Databaze.dao_sqls;

namespace BenchmarkApp
{
    class QuerySetPool
    {
        // values for each thread
        DbTable[] mTableObjects;
        QuerySet[] mQuerySets;

        // values shared by all query records
        Collection<MethodInfo> mQueryMethods;
        Collection<Type[]> mMethodParameters;
        Collection<int> mQueryNumbers;
        Collection<int> mQueryMaximalNumbers;

        DatabasePool mDatabasePool;
        Random mRandom = new Random();
        int mFirstPosition = 0;
        string mLine;

        public static int INT_MAX = 1000;

        public QuerySetPool(DatabasePool databasePool)
        {
            mDatabasePool = databasePool;
            mTableObjects = new DbTable[databasePool.Count()];
            mQuerySets = new QuerySet[databasePool.Count()];
            for (int i = 0; i < databasePool.Count(); i++)
            {
                mQuerySets[i] = new QuerySet(databasePool.GetDatabase(i));
            }

            mQueryMethods = new Collection<MethodInfo>();
            mMethodParameters = new Collection<Type[]>();
            mQueryNumbers = new Collection<int>();
            mQueryMaximalNumbers = new Collection<int>();
        }

        /*
         * Read queries from the file.
         */
        public bool ReadFromFile(string fileName)
        {
            bool ret = true;
            try
            {
                TextReader textReader = new StreamReader(fileName);
                while (true)
                {
                    mLine = textReader.ReadLine();
                    Console.WriteLine(mLine);

                    if (mLine == null)
                    {
                        break;
                    }

                    if (mLine.StartsWith("//"))
                    {
                        continue;
                    }

                    try
                    {
                        QuerySet.numberOfCalls++;
                        // read the object name
                        string objectName = ReadNextValue();
                        SetAllTableObjects(objectName);

                        // read the method name
                        string methodName = ReadNextValue();

                        // read the number of parameters
                        int paramNumber = Convert.ToInt32(ReadNextValue());

                        Type[] types = new Type[paramNumber];
                        // read the types of parameters
                        for (int i = 0; i < paramNumber; i++)
                        {
                            string dataType = ReadNextValue();
                            SetMethodDataType(i, types, dataType);
                        }
                        SetQueryMethod(objectName, methodName, types);
                        mMethodParameters.Add(types);

                        // read the number of the query
                        int maximalNumber = Convert.ToInt32(ReadNextValue());
                        mQueryNumbers.Add(0);
                        mQueryMaximalNumbers.Add(maximalNumber);

                        AddQueryRecords();
                    }
                    catch (Exception e) 
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception)
            {
                ret = false;
            }
            return ret;
        }

        private void SetAllTableObjects(string objectName)
        {
            for (int i = 0 ; i < mTableObjects.Length ; i++)
            {
                mTableObjects[i] = GetTableObject(i, objectName);
            }
        }

        private DbTable GetTableObject(int index, string tableObject)
        {
            DbTable dbTable = null;

            if (tableObject.CompareTo("PaperVersion") == 0)
            {
                dbTable = mDatabasePool.GetDatabase(index).PaperVersionTable;
            }
            else if (tableObject.CompareTo("FileType") == 0)
            {
                dbTable = mDatabasePool.GetDatabase(index).FileTypeTable;
            }
            else if (tableObject.CompareTo("BankaTable") == 0)
            {
                dbTable = mDatabasePool.GetDatabase(index).bankaTable;
            }
            return dbTable;
        }

        private void SetQueryMethod(string tableObject, string methodName, Type[] types)
        {
            MethodInfo methodInfo = null;

            if (tableObject.CompareTo("PaperVersion") == 0)
            {
                methodInfo = typeof(PaperVersionTable).GetMethod(methodName, types);
            }
            else if (tableObject.CompareTo("FileType") == 0)
            {
                methodInfo = typeof(FileTypeTable).GetMethod(methodName, types);
            }
            else if (tableObject.CompareTo("BankaTable") == 0)
            {
                methodInfo = typeof(BankaTable).GetMethod(methodName, types);
            }

            mQueryMethods.Add(methodInfo);
        }

        private void SetMethodDataType(int i, Type[] types, string dataType)
        {
            if (dataType.CompareTo("int") == 0)
            {
                types[i] = System.Type.GetType("System.Int32");
            }
        }

        private void AddQueryRecords()
        {
            for (int i = 0; i < mTableObjects.Length; i++)
            {
                mQuerySets[i].AddTableObject(mTableObjects[i]);
            }
        }

        public QuerySet GetQuerySet(int i)
        {   
            return mQuerySets[i];
        }

        private string ReadNextValue()
        {
            int pos = mLine.IndexOf('\t', mFirstPosition);
            // read the last value
            if (pos == -1)
            {
                pos = mLine.Length;
            }
            string str = mLine.Substring(mFirstPosition, pos - mFirstPosition);

            if (pos == mLine.Length)
            {
                mFirstPosition = 0;
            }
            else
            {
                mFirstPosition = pos + 1;
            }

            return str;
        }

        public MethodInfo GetQueryMethod(int index)
        {
            return mQueryMethods[index];
        }

        public Type[] GetMethodParameters(int index)
        {
            return mMethodParameters[index];
        }

        public int GetQueryNumber(int index)
        {
            return mQueryNumbers[index];
        }

        /// <summary>
        ///  Check if the queries were be extecuted.
        /// </summary>
        /// <returns></returns>
        public bool CanIFinish()
        {
            bool endf = true;
            lock (mQueryNumbers)
            {
                for (int  i = 0 ; i < mQueryNumbers.Count ; i++)
                {
                    if (mQueryNumbers[i] < mQueryMaximalNumbers[i])
                    {
                        endf = false;
                        break;
                    }
                }
            }
            return endf;
        }

        /// <summary>
        ///  Check if it is possible to execute the current query.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool CanIExecuteQuery(int index, out int queryNumber)
        {
            bool flag = true;

            lock (mQueryNumbers)
            {
                if (mQueryNumbers[index] >= mQueryMaximalNumbers[index])
                {
                    flag = false;
                }
                else
                {
                    mQueryNumbers[index]++;
                }
                queryNumber = mQueryNumbers[index];
            }
            return flag;
        }
    }
}