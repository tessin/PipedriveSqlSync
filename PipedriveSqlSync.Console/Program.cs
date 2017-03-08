using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace PipedriveSqlSync.Console
{
    class Program
    {
        static void Main()
        {
            System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput(8192)));

            string pipedriveApiKey = null;

            string connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Database=PipedriveSqlSync;";

            string configPath = $"{Directory.GetCurrentDirectory()}\\User.config";

            if (File.Exists(configPath))
            {
                var xSettings = XDocument.Load(configPath).Root?.Element("appSettings");

                connectionString =
                    xSettings?
                        .Elements()
                        .FirstOrDefault(e => e.Attribute("key")?.Value == "ConnectionString")
                        ?.Attribute("value")
                        ?.Value;

                pipedriveApiKey =
                    xSettings?
                        .Elements()
                        .FirstOrDefault(e => e.Attribute("key")?.Value == "PipedriveApiKey")
                        ?.Attribute("value")
                        ?.Value;
            }

            if (string.IsNullOrEmpty(pipedriveApiKey))
            {
                System.Console.Write("Enter your Pipedrive API key: ");
                pipedriveApiKey = System.Console.ReadLine();
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                System.Console.WriteLine(
                    $"Enter your SQL connection string:");
                connectionString = System.Console.ReadLine();
            }

            System.Console.WriteLine("Starting...");
            System.Console.WriteLine();
            var engine = new PipedriveSqlSyncEngine(pipedriveApiKey, connectionString, new ConsoleLogger());

#if !DEBUG
            try
            {
#endif
                engine.SynchronizeAsync().Wait();
#if !DEBUG
            }
            catch (AggregateException ex)
            {
                System.Console.WriteLine($"ERROR: {ex.InnerException}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"ERROR: {ex}");
            }
#endif

            System.Console.WriteLine();
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }
}
