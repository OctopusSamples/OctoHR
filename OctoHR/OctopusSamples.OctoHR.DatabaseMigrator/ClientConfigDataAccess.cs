using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusSamples.OctoHR.DatabaseMigrator
{
    internal class ClientConfigDataAccess
    {
        private readonly string connectionString;
        public ClientConfigDataAccess(string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            this.connectionString = connectionString;
        }

        public List<Client> GetEnabledClients()
        {
            var clients = new List<Client>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(StoredProcedures.ListClients, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var client = new Client
                            {
                                Name = reader.GetString(nameof(Client.Name)),
                                Slug = reader.GetString(nameof(Client.Slug)),
                                Description = reader.GetString(nameof(Client.Description)),
                                ImageUrl = reader.GetString(nameof(Client.ImageUrl)),
                                ClientDatabase = reader.GetString(nameof(Client.ClientDatabase))
                            };

                            clients.Add(client);
                        }
                    }
                }
            }
            return clients;
        }
    }
}
