using DbUp;
using DbUp.Helpers;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace OctopusSamples.OctoHR.ConfigDBMigrator
{
    class Program
    {
        static int Main(string[] args)
        {
            var retryCount = 0;
            var connectionString = args.FirstOrDefault(x => x.StartsWith("--ConnectionString", StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            connectionString = connectionString.Substring(connectionString.IndexOf("=") + 1).Replace(@"""", string.Empty);

            // retry three times
            while (true)
            {
                try
                {
                    EnsureDatabase.For.SqlDatabase(connectionString);
                    break;
                }
                catch (SqlException)
                {
                    if (retryCount < 3)
                    {
                        Console.WriteLine("Connection error occured, waiting 3 seconds then trying again.");
                        Thread.Sleep(3000);
                        retryCount += 1;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var upgrader =
                DeployChanges.To
                   .SqlDatabase(connectionString)
                   .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                   .LogToConsole()
                   .Build();

            if (args.Any(a => a.StartsWith("--PreviewReportPath", StringComparison.InvariantCultureIgnoreCase)))
            {
                var report = args.FirstOrDefault(x => x.StartsWith("--PreviewReportPath", StringComparison.OrdinalIgnoreCase));
                report = report.Substring(report.IndexOf("=") + 1).Replace(@"""", string.Empty);

                var fullReportPath = Path.Combine(report, "UpgradeReport.html");

                Console.WriteLine($"Generating the report at {fullReportPath}");

                upgrader.GenerateUpgradeHtmlReport(fullReportPath);
            }
            else
            {
                var result = upgrader.PerformUpgrade();

                // Display the result
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
            return 0;
        }
    }
}
