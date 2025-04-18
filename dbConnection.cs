using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Npgsql;
using System.Data;

namespace WebApplication1
{
    public class dbConnection
    {

        public readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Database=db;Password=P@ssw0rd";

        public dbConnection() {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    conn.Close();
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
            }
        }

        public DataRow GetInfoForTable(string tableName, int id)
        {
            DataRow dataRow = null;
            string query = $"SELECT * FROM {tableName} WHERE id = @id";

            Console.WriteLine(query);
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();

                        using (var reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            dataRow = dt.Rows[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dataRow;
        }

        public void UpdateUsers(string name, string lastName, int id)
        {
            string query = "UPDATE users set name = @name, lastName = @lastName WHERE id = @id";

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("name", name);
                        cmd.Parameters.AddWithValue("lastName", lastName);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void RemoveUsers(int id)
        {
            string query = "DELETE FROM users WHERE id = @id";

            try
            {
                using (NpgsqlConnection conn =new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddUsers(string name, string lastName)
        {
            string query = "INSERT INTO users (name, lastName) VALUES (@name, @lastName)";

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@lastName", lastName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
