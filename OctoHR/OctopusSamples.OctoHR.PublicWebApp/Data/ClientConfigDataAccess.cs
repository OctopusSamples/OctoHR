using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace OctopusSamples.OctoHR.PublicWebApp.Data
{
    public interface IClientConfigDataAccess
    {
        ApplicationUser CheckClientUser(string clientCode, string username, string password);
        List<Employee> GetEmployees(string clientCode);
    }

    public class DatabaseOptions
    {
        public string ConnectionString { get; set; }
    }

    public class ClientConfigDataAccess : IClientConfigDataAccess
    {
        private readonly DatabaseOptions _databaseOptions;
        // Config DB Queries
        public const string ListClients = "[dbo].[List_Enabled_Clients]";

        // CustDB Queries
        public const string CheckUser = "[dbo].[CheckUser]";
        public const string ListEmployees = "[dbo].[List_Employees]";

        public ClientConfigDataAccess(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions.Value;
        }

        public ApplicationUser CheckClientUser(string clientCode, string username, string password)
        {
            ApplicationUser user = null;

            var clients = GetClients();
            var registeredClient = clients.FirstOrDefault(c => c.Slug == clientCode);

            if (registeredClient != null)
            {
                var clientConnection = new SqlConnectionStringBuilder(_databaseOptions.ConnectionString);
                clientConnection.InitialCatalog = registeredClient.ClientDatabase;

                using (var connection = new SqlConnection(clientConnection.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(CheckUser, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@Username", SqlDbType.NVarChar, 255).Value = username;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new ApplicationUser
                                {
                                    ClientCode = clientCode,
                                    Username = username,
                                    FullName = reader.GetString("Name"),
                                    IsAdministrator = reader.GetBoolean("IsAdmin")
                                };
                            }
                        }
                    }
                }
            }
            return user;
        }

        public List<Client> GetClients()
        {
            var clients = new List<Client>();

            using (var connection = new SqlConnection(_databaseOptions.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(ListClients, connection))
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

        public List<Employee> GetEmployees(string clientCode)
        {
            List<Employee> employees = null;
            var clients = GetClients();
            var registeredClient = clients.FirstOrDefault(c => c.Slug == clientCode);

            if (registeredClient != null)
            {
                var clientConnection = new SqlConnectionStringBuilder(_databaseOptions.ConnectionString);
                clientConnection.InitialCatalog = registeredClient.ClientDatabase;

                using (var connection = new SqlConnection(clientConnection.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(ListEmployees, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            employees = new List<Employee>();
                            while (reader.Read())
                            {
                                var employee = new Employee
                                {
                                    Name = reader.GetString(nameof(Employee.Name)),
                                    Position = reader.GetString(nameof(Employee.Position)),
                                    Salary = reader.GetDecimal(nameof(Employee.Salary)),
                                    Manager = reader.GetString(nameof(Employee.Manager)),
                                    CurrentStaff = reader.GetBoolean(nameof(Employee.CurrentStaff))
                                };
                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            return employees;
        }
    }
}
