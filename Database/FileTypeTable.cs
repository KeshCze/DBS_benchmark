using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Oracle.DataAccess.Client;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AspNetExampleApp.Database
{
    public class FileTypeTable : DbTable
    {
        public static String TABLE_NAME = "FileType";

        public String SQL_SELECT = "SELECT * FROM FileType";
        public String SQL_SELECT_ID = "SELECT * FROM FileType WHERE id=:id";
        public String SQL_INSERT = "INSERT INTO FileType (id, extension, mime) VALUES (:id, :extension, :mime)";

        public FileTypeTable(Database database) : base(database, TABLE_NAME)
        {
        }

        /**
         * Insert the record.
         **/
        public int Insert(FileType fileType)
        { 
            mDatabase.Connect();
            SqlCommand command = mDatabase.CreateCommand(SQL_INSERT);
            PrepareCommand(command, fileType);
            int ret = mDatabase.Insert(command);
            mDatabase.Close();
            return ret;
        }

        /**
         * Update the record.
         **/
        public int Update(FileType fileType)
        {
            // ...
            return 0;
        }

        /**
         * Prepare a command.
         **/
        private void PrepareCommand(SqlCommand command, FileType fileType)
        {
            command.Parameters.Add(new SqlParameter(":" + FileType.ATTR_id, SqlDbType.Decimal));
            command.Parameters[":" + FileType.ATTR_id].Value = fileType.Id;

            command.Parameters.Add(new SqlParameter(":" + FileType.ATTR_extension, SqlDbType.VarChar, fileType.Extension.Length));
            command.Parameters[":" + FileType.ATTR_extension].Value = fileType.Extension;

            command.Parameters.Add(new SqlParameter(":" + FileType.ATTR_mime, SqlDbType.VarChar, fileType.Mime.Length));
            command.Parameters[":" + FileType.ATTR_mime].Value = fileType.Mime;
        }

        /**
         * Select records.
         **/
        public Collection<FileType> Select()
        {
            mDatabase.Connect();
            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT);
            SqlDataReader reader = mDatabase.Select(command);

            Collection<FileType> fileTypes = Read(reader);
            reader.Close();
            mDatabase.Close();
            return fileTypes;
        }

        /**
         * Select the record.
         **/
        public FileType Select(int id)
        {
            mDatabase.Connect();
            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ID);

            command.Parameters.Add(new SqlParameter(":" + FileType.ATTR_id, SqlDbType.Decimal));
            command.Parameters[":" + FileType.ATTR_id].Value = id;
            SqlDataReader reader = mDatabase.Select(command);

            Collection<FileType> fileTypes = Read(reader);
            FileType fileType = null;
            if (fileTypes.Count == 1)
            {
                fileType = fileTypes[0];
            }
            reader.Close();
            mDatabase.Close();
            return fileType;
        }

        private Collection<FileType> Read(SqlDataReader reader)
        {
            Collection<FileType> fileTypes = new Collection<FileType>();

            while (reader.Read())
            {
                FileType fileType = new FileType();
                fileType.Id = (int)reader.GetDecimal(0);
                fileType.Extension = reader.GetString(1);
                fileType.Mime = reader.GetString(2);
                fileTypes.Add(fileType);
            }
            return fileTypes;
        }

        /**
         * Delete the record.
         */
        public int Delete(FileType fileType)
        {
            // ...
            return 0;
        }
    }
}

