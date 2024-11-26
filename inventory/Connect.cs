using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory
{
    public static class Connect
    {
        // Cadena de conexión centralizada
        private static readonly string connectionString = "Server=DESKTOP-6M7CQ1U;Database=inventory_db;Trusted_Connection=True;";

        // Método para verificar la conexión
        public static void CheckConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {
                        Console.WriteLine("Connection successful!");
                    }
                    else
                    {
                        Console.WriteLine("Connection failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Método para ejecutar consultas SQL sin retorno de datos
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return -1;
            }
        }

        // Método para ejecutar una consulta que retorna un solo valor
        public static object ExecuteScalar(string query, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        // Método para ejecutar una consulta que retorna un DataTable
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    if (parameters != null)
                    {
                        da.SelectCommand.Parameters.AddRange(parameters);
                    }

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        // Crear un registro de manera más flexible
        public static int CreateRecord(string table, Dictionary<string, object> values)
        {
            string columns = string.Join(", ", values.Keys);
            string parameters = string.Join(", ", values.Keys.Select(k => "@" + k));
            string query = $"INSERT INTO {table} ({columns}) VALUES ({parameters})";

            SqlParameter[] sqlParameters = values.Select(v => new SqlParameter("@" + v.Key, v.Value)).ToArray();
            return ExecuteNonQuery(query, sqlParameters);
        }

        // Leer registros de una tabla con condiciones opcionales
        public static DataTable ReadRecords(string table, string conditions = "", SqlParameter[] parameters = null)
        {
            string query = $"SELECT * FROM {table}";
            if (!string.IsNullOrEmpty(conditions))
            {
                query += " WHERE " + conditions;
            }

            return ExecuteQuery(query, parameters);
        }

        // Actualizar un registro en una tabla
        public static int UpdateRecord(string table, Dictionary<string, object> values, string conditions, SqlParameter[] parameters)
        {

            string setClause = string.Join(", ", values.Select(kv => kv.Key + " = @" + kv.Key));
            string query = $"UPDATE {table} SET {setClause} WHERE {conditions}";

            SqlParameter[] sqlParameters = values.Select(v => new SqlParameter("@" + v.Key, v.Value)).ToArray();
            return ExecuteNonQuery(query, sqlParameters.Concat(parameters).ToArray());
        }

        // Eliminar un registro de una tabla
        public static int DeleteRecord(string table, string conditions, SqlParameter[] parameters)
        {
            string query = $"DELETE FROM {table} WHERE {conditions}";
            return ExecuteNonQuery(query, parameters);
        }
    }
}