using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace member
{
    public class Connection
    {
        public static MySqlConnection GetDBConnection()
        {
            if (DBConnection == null)
            {
                DBConnection = new MySqlConnection(ConnectionString);

                if (DBConnection.State == System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        DBConnection.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to connect database");
                        return null;
                    }
                }
               
            }
            return DBConnection;
        }
        public static string ConnectionString //connect to server
        {
            get
            {
                return string.Format("SERVER ={0};USER ID={1};PASSWORD={2};" +
                        "DATABASE={3};","localhost","root","0923","io_2019");

            }
        }
        public static DataTable GetDataTable(String sql)
        {
            DataSet ds = new DataSet();
            new MySqlDataAdapter(new MySqlCommand(sql, Connection.GetDBConnection())).Fill(ds);

            return ds.Tables[0];
        }
        private static MySqlConnection DBConnection;
    }
}
