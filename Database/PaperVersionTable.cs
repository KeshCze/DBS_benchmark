using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Oracle.DataAccess.Client;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AspNetExampleApp.Database
{
    public class PaperVersionTable : DbTable
    {
        public static String TABLE_NAME = "PaperVersion";
        public String SQL_SELECT = "SELECT id,uploadDate,idFileType from PaperVersion ORDER BY id";

        public PaperVersionTable(Database database) : base(database, TABLE_NAME)
        {
        }

        /**
         * Insert the record.
         **/
        public int Insert(PaperVersion paperVersion)
        {
            // ...
            int insertNo = 0;
            return insertNo;
        }

        /**
         * Update the record.
         **/
        public int Update(PaperVersion paperVersion)
        {
            // ...
            return 0;
        }

        /**
         * Select records.
         **/
        public Collection<PaperVersion> Select()
        {
            mDatabase.Connect();
            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT);
            SqlDataReader reader = mDatabase.Select(command);

            Collection<PaperVersion> paperVersions = Read(reader);
            reader.Close();
            mDatabase.Close();
            return paperVersions;
        }

        public Collection<PaperVersion> Read(SqlDataReader reader)
        {
            Collection<PaperVersion> paperVersions = new Collection<PaperVersion>();
            while (reader.Read())
            {
                PaperVersion paperVersion = new PaperVersion();
                paperVersion.Id = (int)reader.GetDecimal(0);
                paperVersion.UploadDate = reader.GetDateTime(1);

                if (!reader.IsDBNull(2))
                {
                    paperVersion.IdFileType = (int)reader.GetDecimal(2);
                    paperVersion.FileType = mDatabase.FileTypeTable.Select(paperVersion.IdFileType);
                }
                else
                {
                    paperVersion.IdFileType = -1;
                    paperVersion.FileType = null;
                }
                paperVersions.Add(paperVersion);
            }
            return paperVersions;
        }

        /**
         * Delete the record.
         */
        public int Delete(PaperVersion paperVersion)
        {
            // ...
            return 0;
        }
    }
}
