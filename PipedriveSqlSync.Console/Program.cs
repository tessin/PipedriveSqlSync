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
            System.Console.WriteLine();
            var engine = new PipedriveSqlSyncEngine(apiKey, connString, new ConsoleLogger());

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
