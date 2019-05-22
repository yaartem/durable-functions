using System;
using System.Runtime.CompilerServices;

namespace Dodo.Bus
{
    public static class TimeExtensions
    {
        public static TaskAwaiter GetAwaiter(this (ITimeService timeService, TimeSpan span) t)
        {
            var (time, span) = t;
            return time.GetAwaitable(time.GetCurrent().Add(span)).GetAwaiter();
        }
        
        public static TaskAwaiter GetAwaiter(this (ITimeService timeService, int milliseconds) t)
        {
            var (time, milliseconds) = t;
            return time
                .GetAwaitable(
                    time
                        .GetCurrent()
                        .AddMilliseconds(milliseconds))
                .GetAwaiter();
        }
        
        public static TaskAwaiter GetAwaiter(this (ITimeService timeService, double seconds) t)
        {
            var (time, seconds) = t;
            return time
                .GetAwaitable(
                    time
                        .GetCurrent()
                        .AddSeconds(seconds))
                .GetAwaiter();
        }
    }
}