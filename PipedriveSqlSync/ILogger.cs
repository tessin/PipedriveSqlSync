using System;

namespace PipedriveSqlSync
{
    public interface ILogger
    {
        void Debug(string text);
        void Info(string text);
        void Error(string text);
        void Error(Exception ex, string text = null);
    }
}
