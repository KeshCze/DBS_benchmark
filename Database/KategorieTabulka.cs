using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Oracle.DataAccess.Client;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace AspNetExampleApp.Database
{
        public class KategorieTabulka : DbTable
        {

            public static String TABLE_NAME = "kategorieDBS";

            public static String SQL_SELECT = "SELECT * FROM kategorieDBS k";

            public static String SQL_SELECT_ID = "SELECT * FROM kategorieDBS k WHERE k.pk_id_kategorie = @pk_id_kategorie";

            public static String SQL_INSERT = "INSERT INTO kategorieDBS (nazev) VALUES(@nazev)";

            public static String SQL_UPDATE = "UPDATE kategorieDBS SET nazev = @nazev WHERE pk_id_kategorie = @pk_id_kategorie";


        public KategorieTabulka(Database database) : base(database, TABLE_NAME)
        {
        }

        /// <summary>
        /// Insert
        /// </summary>
        public int Insert(Kategorie kategorie)
            {
                mDatabase.Connect();
            
                SqlCommand command = mDatabase.CreateCommand(SQL_INSERT);
                PrepareCommand(command, kategorie);
                int ret = mDatabase.ExecuteNonQuery(command);
                mDatabase.Close();
                return ret;
            }


            /// <summary>
            /// Update
            /// </summary>
            public int Update(Kategorie kategorie)
            {
               
                mDatabase.Connect();


                SqlCommand command = mDatabase.CreateCommand(SQL_UPDATE);
                PrepareCommand(command, kategorie);
                int ret = mDatabase.ExecuteNonQuery(command);
                mDatabase.Close();
                return ret;
            }

            /// <summary>
            /// Select records
            /// </summary>

            public Collection<Kategorie> Select()
            {
                
                mDatabase.Connect();

                SqlCommand command = mDatabase.CreateCommand(SQL_SELECT);
                SqlDataReader reader = mDatabase.Select(command);

                Collection<Kategorie> kategories = Read(reader);
                reader.Close();
                mDatabase.Close();
                return kategories;

            }


            /// <summary>
            /// Select the record.
            /// </summary>
            public Kategorie SelectOne(int idKategorie)
            {
                mDatabase.Connect();
                SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ID);

                command.Parameters.AddWithValue("@pk_id_kategorie", idKategorie);
                SqlDataReader reader = mDatabase.Select(command);

                Collection<Kategorie> kategories = Read(reader);
                Kategorie kategorie = null;
                if (kategories.Count == 1)
                {
                    kategorie = kategories[0];
                }
                reader.Close();
                mDatabase.Close();
                return kategorie;

            }



            public void Delete(int id)
            {
               
                    mDatabase.Connect();
        
                // 1.  create a command object identifying the stored procedure
                SqlCommand command = mDatabase.CreateCommand("Odstranit_kategorii");

                // 2. set the command object so it knows to execute a stored procedure
                command.CommandType = CommandType.StoredProcedure;


                // 3. create input parameters
                SqlParameter input = new SqlParameter();
                input.ParameterName = "@p_id";
                input.DbType = DbType.Int32;
                input.Value = id;
                input.Direction = ParameterDirection.Input;
                command.Parameters.Add(input);

                // 4. execute procedure
                int ret = mDatabase.ExecuteNonQuery(command);

            }



            /// <summary>
            /// Prepare a command.
            /// </summary>
            private void PrepareCommand(SqlCommand command, Kategorie kategorie)
            {
                command.Parameters.AddWithValue("@pk_id_kategorie", kategorie.Pk_id_kategorie);
                command.Parameters.AddWithValue("@nazev", kategorie.Nazev);

            }

            /// <summary>
            /// Read
            /// </summary>

            private Collection<Kategorie> Read(SqlDataReader reader)
            {
                Collection<Kategorie> kategories = new Collection<Kategorie>();

                while (reader.Read())
                {
                    Kategorie kategorie = new Kategorie();
                    int i = -1;
                    kategorie.Pk_id_kategorie = reader.GetInt32(++i);
                    kategorie.Nazev = reader.GetString(++i);

                    kategories.Add(kategorie);
                }
                return kategories;
            }
        }
    }



