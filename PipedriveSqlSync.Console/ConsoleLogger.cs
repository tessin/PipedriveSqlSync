using System;

namespace PipedriveSqlSync.Console
{
    class ConsoleLogger : ILogger
    {
        public void Debug(string text)
        {
            System.Console.WriteLine(text);
        }

        public void Info(string text)
        {
            System.Console.WriteLine(text);
        }

        public void Error(string text)
        {
            System.Console.WriteLine($"ERROR: {text}");
        }

        public void Error(Exception ex, string text = null)
        {
            System.Console.WriteLine($"ERROR: {text}, {ex}");
        }
    }
}