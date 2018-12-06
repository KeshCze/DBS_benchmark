using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Oracle.DataAccess.Client;
// mk: using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using ORM.Databaze.dao_sqls;

namespace AspNetExampleApp.Database
{
    public class Database
    {
        // private static int TIMEOUT = 240;
        private SqlConnection mConnection;
        uint mConnectNumber = 0;
        bool mTransactionFlag = false;
        SqlTransaction mSqlTransaction;
        private String mLanguage = "en";
        private static String CONNECTION_STRING = //"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbsys.cs.vsb.cz)(PORT=1521)))(CONNECT_DATA=(SERVER=SHARED)(SERVICE_NAME=dbsys.cs.vsb.cz\\STUDENT)));User Id=OPL0015;Password=password=beYteEWxEn;";
        "server=dbsys.cs.vsb.cz\\STUDENT;database=bab0082;user=bab0082;password=t1j4bWGBPn";
        public PaperVersionTable PaperVersionTable;
        public FileTypeTable FileTypeTable;
        public KategorieTabulka KategorieTabulka;
        public BankaTable bankaTable;

        public Database() 
        {
            mConnection = new SqlConnection();
            PaperVersionTable = new PaperVersionTable(this);
            FileTypeTable = new FileTypeTable(this);
            KategorieTabulka = new KategorieTabulka(this);
            bankaTable = new BankaTable(this);
        }

        /**
         * Connect.
         **/
        public bool Connect(String conString)
        {
            if (!mTransactionFlag)
            {
                mConnectNumber++;
            }
            if (mConnection.State != System.Data.ConnectionState.Open)
            {
                mConnection.ConnectionString = conString;
                mConnection.Open();
            }
            return true;
        }

        /**
         * Connect.
         **/
        public bool Connect()
        {
            bool ret = true;

            if (mConnection.State != System.Data.ConnectionState.Open)
            {
                ret = Connect(CONNECTION_STRING);
                // ret = Connect(WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            }
            else if (!mTransactionFlag)
            {
                mConnectNumber++;
            }

            return ret;
        }

        /**
         * Close.
         **/
        public bool Close()
        {
            if (!mTransactionFlag)
            {
                if (mConnectNumber > 0)
                {
                    mConnectNumber--;
                }
            }

            if (mConnectNumber == 0)
            {
                mConnection.Close();
            }
            return true;
        }

        /**
         * Begin a transaction.
         **/
        public void BeginTransaction()
        {
            mSqlTransaction = mConnection.BeginTransaction(IsolationLevel.Serializable);
            mTransactionFlag = true;
        }

        /**
         * End a transaction.
         **/
        public void EndTransaction()
        {
            // command.Dispose()
            mSqlTransaction.Commit();
            mTransactionFlag = false;
            mConnection.Close();
            Close();
        }

        /**
         * If a transaction is failed call it.
         **/
        public void Rollback()
        {
            mSqlTransaction.Rollback();
        }

        /**
         * Update a record encapulated in the command.
         **/
        public int Update(SqlCommand command)
        {
            // ...
            return 0;
        }

        /**
         * Insert a record encapulated in the command.
         **/
        public int Insert(SqlCommand command)
        {
            int rowNumber = 0;
            try
            {
                command.Prepare();
                rowNumber = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Close();
            }
            return rowNumber;
        }

        /**
         * Create command.
         **/
        public SqlCommand CreateCommand(string strCommand)
        {
            SqlCommand command = new SqlCommand(strCommand, mConnection);

            if (mTransactionFlag)
            {
                command.Transaction = mSqlTransaction;
            }
            return command;
        }

        /**
         * Select encapulated in the command.
         **/
        public SqlDataReader Select(SqlCommand command)
        {
            command.Prepare();
            SqlDataReader sqlReader = command.ExecuteReader();
            return sqlReader;
        }

        /**
         * Delete encapulated in the command.
         **/
        public int Delete(SqlCommand command)
        {
            // ...
            return 0;
        }

        public String Language
        {
            get
            {
                return mLanguage;
            }
            set
            {
                mLanguage = value;
            }
        }

        public int ExecuteNonQuery(SqlCommand command)
        {
            int rowNumber = 0;
           
                rowNumber = command.ExecuteNonQuery();
            

            return rowNumber;
        }
    }
}

