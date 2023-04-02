using CaptaApplication.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CaptaApplication.Utilities
{
    public class SqlConnectionManager
    {
        private readonly string _connectionString;

        public SqlConnectionManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool InsertData(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetIdClient(string query)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                connection.Close();
                return id;
            }
            connection.Close();
            return 0;
        }
        public ClientModel GetClient(string query)
        {

            ClientModel client = new ClientModel();
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                client.Nome = reader.GetString(0);
                client.Cpf = reader.GetString(1);
                client.Sexo = reader.GetString(2);
                client.TipoCliente = reader.GetString(3);
                client.SituacaoCliente = reader.GetString(4);
            }
            connection.Close();
            return client;
        }
        public List<ClientModel> GetClients(string query)
        {
            List<ClientModel> listClients = new List<ClientModel>();
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ClientModel client = new ClientModel();
                client.Nome = reader.GetString(0);
                client.Cpf = reader.GetString(1);
                client.Sexo = reader.GetString(2);
                client.TipoCliente = reader.GetString(3);
                client.SituacaoCliente = reader.GetString(4);
                listClients.Add(client);
            }
            connection.Close();
            return listClients;
        }
        public void UpdateData(string query)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void DeleteData(string query)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }

}
