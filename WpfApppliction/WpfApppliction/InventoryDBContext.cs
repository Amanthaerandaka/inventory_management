using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WpfApppliction.db;

namespace WpfApppliction
{
    public class InventoryDBContext
    {
        private readonly DBConnection dbcon;

        public InventoryDBContext()
        {
            dbcon = new DBConnection();
        }

        // Add Item
        public void AddItem(Item item)
        {
            using (SqlConnection conn = dbcon.GetConnection())
            {
                // INSERT query
                string query = "INSERT INTO [Items] ([ItemName], [UnitPrice], [CasePerUnit], [Supplier], [DateAdded]) " +
                               "VALUES (@name, @price, @casePerUnit, @supplier, @date); SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", item.ItemName);
                cmd.Parameters.AddWithValue("@price", item.UnitPrice);
                cmd.Parameters.AddWithValue("@casePerUnit", item.CasePerUnit);
                cmd.Parameters.AddWithValue("@supplier", item.Supplier);

                // @date is not null
                cmd.Parameters.AddWithValue("@date", item.Date ?? (object)DBNull.Value);

                conn.Open();
                item.ItemID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Get All Items
        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();

            using (SqlConnection conn = dbcon.GetConnection())
            {
                // SELECT query
                string query = "SELECT [ItemID], [ItemName], [UnitPrice], [CasePerUnit], [Supplier], [DateAdded] FROM [Items]";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new Item
                        {
                            ItemID = Convert.ToInt32(reader["ItemID"]),
                            ItemName = reader["ItemName"].ToString(),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            CasePerUnit = Convert.ToInt32(reader["CasePerUnit"]),
                            Supplier = reader["Supplier"].ToString(),
                             Date = reader["DateAdded"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DateAdded"])
                        });
                    }
                }
            }
            return items;
        }

        // Search Items
        public List<Item> SearchItems(string searchTerm)
        {
            List<Item> items = new List<Item>();

            using (SqlConnection conn = dbcon.GetConnection())
            {
                string query = "SELECT [ItemID], [ItemName], [UnitPrice], [CasePerUnit], [Supplier], [DateAdded] " +
                               "FROM [Items] WHERE [ItemName] LIKE @searchTerm";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new Item
                        {
                            ItemID = Convert.ToInt32(reader["ItemID"]),
                            ItemName = reader["ItemName"].ToString(),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            CasePerUnit = Convert.ToInt32(reader["CasePerUnit"]),
                            Supplier = reader["Supplier"].ToString(),
                            Date = reader["DateAdded"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DateAdded"])
                        });
                    }
                }
            }
            return items;
        }

        // Update Item
        public void UpdateItem(Item item)
        {
            using (SqlConnection conn = dbcon.GetConnection())
            {
                // UPDATE query
                string query = "UPDATE [Items] SET [ItemName]=@name, [UnitPrice]=@price, [CasePerUnit]=@casePerUnit, " +
                               "[Supplier]=@supplier, [DateAdded]=@date WHERE [ItemID]=@id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", item.ItemID);
                cmd.Parameters.AddWithValue("@name", item.ItemName);
                cmd.Parameters.AddWithValue("@price", item.UnitPrice);
                cmd.Parameters.AddWithValue("@casePerUnit", item.CasePerUnit);
                cmd.Parameters.AddWithValue("@supplier", item.Supplier);

                // @date is not null
                cmd.Parameters.AddWithValue("@date", item.Date ?? (object)DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Delete Item
        public void DeleteItem(int itemID)
        {
            using (SqlConnection conn = dbcon.GetConnection())
            {
                // DELETE query
                string query = "DELETE FROM [Items] WHERE [ItemID]=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", itemID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

