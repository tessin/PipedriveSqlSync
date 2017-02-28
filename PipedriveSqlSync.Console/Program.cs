using System;
using System.IO;

namespace PipedriveSqlSync.Console
{
    class Program
    {
        static void Main()
        {
            System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput(8192)));

            const string defaultConnString = @"Server=(LocalDB)\MSSQLLocalDB;Database=PipedriveSqlSync;";

            System.Console.Write("Enter your Pipedrive API key: ");
            var apiKey = System.Console.ReadLine();

            System.Console.WriteLine(
                $"\r\nEnter your SQL connection string (default: \"{defaultConnString}\"):");
            var connString = System.Console.ReadLine();
            if (string.IsNullOrWhiteSpace(connString))
                connString = defaultConnString;

            System.Console.WriteLine("\r\nStarting...");
            var engine = new PipedriveSqlSyncEngine(apiKey, connString, new ConsoleLogger());

            try
            {
                engine.SynchronizeAsync().Wait();
            }
            catch (AggregateException ex)
            {
                System.Console.WriteLine($"ERROR: {ex.InnerException}");
#if DEBUG
                throw;
#endif
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"ERROR: {ex}");
#if DEBUG
                throw;
#endif
            }

            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }
}
