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
                    Console.WriteLine("Working on database for client: {0}", client.Name);
                    var clientConnection = new SqlConnectionStringBuilder(configConnectionString);
                    clientConnection.InitialCatalog = client.ClientDatabase;
                   
                    // Migrate Tenant DB
                    Console.WriteLine("Migrating database for client: {0}", client.Name);
                    var upgrader = DeployChanges.To
                   .SqlDatabase(clientConnection.ConnectionString)
                   .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                   .LogToConsole()
                   .Build();

                    if (args.Any(a => a.StartsWith("--PreviewReportPath", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        var report = args.FirstOrDefault(x => x.StartsWith("--PreviewReportPath", StringComparison.InvariantCultureIgnoreCase));
                        report = report.Substring(report.IndexOf("=") + 1).Replace(@"""", string.Empty);

                        var fullReportPath = Path.Combine(report, $"UpgradeReport-{client.Slug}.html");

                        Console.WriteLine($"Generating the report at {fullReportPath}");

                        upgrader.GenerateUpgradeHtmlReport(fullReportPath);
                    }
                    else
                    {
                        var result = upgrader.PerformUpgrade();

                        if (result.Successful)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Success!");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(result.Error);
                            Console.WriteLine("Failed!");
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
