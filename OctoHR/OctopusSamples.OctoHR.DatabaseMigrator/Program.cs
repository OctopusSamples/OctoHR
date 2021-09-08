using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace OctopusSamples.OctoHR.DatabaseMigrator
{
    class Program
    {
        private static ClientConfigDataAccess clientConfigDataAccess;

        static int Main(string[] args)
        {
            var retryCount = 0;
            var configConnectionString = args.FirstOrDefault(x => x.StartsWith("--ConfigConnectionString", StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrWhiteSpace(configConnectionString))
                throw new ArgumentNullException(nameof(configConnectionString));

            configConnectionString = configConnectionString.Substring(configConnectionString.IndexOf("=") + 1).Replace(@"""", string.Empty);
            clientConfigDataAccess = new ClientConfigDataAccess(configConnectionString);

            try
            {
                var clients = clientConfigDataAccess.GetEnabledClients();
                foreach (var client in clients)
                {
                    Console.WriteLine("Migrating database for client: {0}", client.Name);
                    var clientConnection = new SqlConnectionStringBuilder(configConnectionString);
                    clientConnection.InitialCatalog = client.ClientDatabase;
                    while (true)
                    {
                        try
                        {
                            EnsureDatabase.For.SqlDatabase(clientConnection.ConnectionString);
                            break;
                        }
                        catch (SqlException)
                        {
                            if (retryCount < 3)
                            {
                                Console.WriteLine("Connection error occured for client: {0}, waiting 3 seconds then trying again.", client.Name);
                                Thread.Sleep(3000);
                                retryCount += 1;
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred");
                Console.WriteLine("Details: {0}", ex.Message);
                return 1;
            }

            return 0;
        }
    }
}
