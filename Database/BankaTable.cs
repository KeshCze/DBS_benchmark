using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using AspNetExampleApp.Database;

namespace ORM.Databaze.dao_sqls
{
    public class BankaTable : DbTable
    {
        private static string TABLE_NAME = "Banka";

        private static string SQL_INSERT = "insert into \"Banka\" values (@idBanky,@nazev,@DIC,@ICO,@tel,@email,@deleted)";
        private static string SQL_UPDATE = "UPDATE \"Banka\" SET idBanky=@idBanky,nazev=@nazev,DIC=@DIC,ICO=@ICO,tel=@tel,email=@email,deleted=@deleted WHERE idBanky = @idBanky";
        private static string SQL_SELECT = "SELECT top 50 * FROM \"Banka\"";

        public BankaTable(Database database) :base(database,TABLE_NAME)
        {

        }
        public void Insert()
        {
            Random rnd = new Random();
            Banka tmp = new Banka() { nazev = "nazev", tel = "12126161", email = "asfssdf@afas.cz", ICO = 1213123, DIC = "asdfasfa213", deleted = 0, idBanky = rnd.Next(600000, 99999999) };
            
            try
            {
                Insert_1_1(tmp);

            }
            catch (Exception)
            {

                
            }
        }
        

        public int Insert_1_1(Banka banka)
        {
            this.mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_INSERT);
            command.Parameters.AddWithValue("@idBanky", banka.idBanky);
            command.Parameters.AddWithValue("@nazev", banka.nazev);
            command.Parameters.AddWithValue("@DIC", banka.DIC);
            command.Parameters.AddWithValue("@ICO", banka.ICO);
            command.Parameters.AddWithValue("@tel", banka.tel);
            command.Parameters.AddWithValue("@email", banka.email);
            command.Parameters.AddWithValue("@deleted", banka.deleted == null ? DBNull.Value : (object)banka.deleted);

            int ret = mDatabase.ExecuteNonQuery(command);


            this.mDatabase.Close();
            return ret;
        }


        public static int Update_1_2(Banka banka,Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = (Database)pDb;
            }

            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            command.Parameters.AddWithValue("@idBanky", banka.idBanky);
            command.Parameters.AddWithValue("@nazev", banka.nazev);
            command.Parameters.AddWithValue("@DIC", banka.DIC);
            command.Parameters.AddWithValue("@ICO", banka.ICO);
            command.Parameters.AddWithValue("@tel", banka.tel);
            command.Parameters.AddWithValue("@email", banka.email);
            command.Parameters.AddWithValue("@deleted", banka.deleted == null ? DBNull.Value : (object)banka.deleted);

            int ret = db.ExecuteNonQuery(command);

            if (pDb==null)
            {
                db.Close();
            }

            return ret;
        }

        public void Select()
        {
            Select_1_3();
        }

        public List<Banka> Select_1_3()
        {
            this.mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT);
            SqlDataReader dataReader = mDatabase.Select(command);

            List<Banka> list = Read(dataReader);
            dataReader.Close();

            mDatabase.Close();

            return list;

        }

        public static int Delete_1_4(int idBank,Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = (Database)pDb;
            }

            SqlCommand command = db.CreateCommand("BankaDel");
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@idBanky";
            sqlParameter.DbType = DbType.Int32;
            sqlParameter.Value = idBank;
            sqlParameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(sqlParameter);

            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }
            return ret;
        }

        private static List<Banka> Read(SqlDataReader dataReader)
        {
            List<Banka> list = new List<Banka>();

            while(dataReader.Read())
            {
                int i = -1;
                Banka banka = new Banka();
                banka.idBanky = dataReader.GetInt32(++i);
                banka.nazev = dataReader.GetString(++i);
                banka.DIC = dataReader.GetString(++i);
                banka.ICO = dataReader.GetInt32(++i);
                banka.tel = dataReader.GetString(++i);
                banka.email = dataReader.GetString(++i);
                if (!dataReader.IsDBNull(++i))
                {
                    banka.deleted = dataReader.GetInt32(i);
                }

                list.Add(banka);
            }
            return list;
        }
    }


}
