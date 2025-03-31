using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace WpfApppliction.db
{
    public class DBConnection
    {
        private readonly string connectionString = "Server=AMANTHA_LK\\SQLEXPRESS;Database=InventoryDB;Trusted_Connection=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
