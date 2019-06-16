
using System;

namespace durable_functions.Framework
{
    public interface ILogger
    {
        void Log(string message);
        void LogColored(string message, ConsoleColor color);
    }
}