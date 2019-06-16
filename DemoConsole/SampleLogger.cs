using System;
using durable_functions.Framework;

namespace DemoConsole
{
    public class SampleLogger : ILogger
    {
        private bool _logsEnabled;

        public void Log(string message)
        {
            if (!_logsEnabled) return;
            Console.WriteLine(message);
        }
        public void LogColored(string message, ConsoleColor color)
        {
            if (!_logsEnabled) return;
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = prev;
        }

        public void SetLogginEnabled(bool enable)
        {
            _logsEnabled = enable;
        }
    }
}