using System;
using System.Threading.Tasks;
using Dodo.Bus;

namespace durable_functions.Framework
{
    public static class DeterministicExtensions
    {
        public static async Task<T> RetryLinear<T>(this ITimeService timeService, Func<Task<T>> func, DateTime dueTime, TimeSpan singleRetryTimeout)
        {
            if (singleRetryTimeout <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(singleRetryTimeout));
            }
            var baseTime = timeService.GetCurrent();
            var now = baseTime;
            while (dueTime >= now)
            {
                now = timeService.GetCurrent();
                var advance = now + singleRetryTimeout;
                var workTask = func();
                await await AwaitExtensions.WhenAnySpecialized(timeService.GetAwaitable(advance), workTask);
                if (workTask.IsCompletedSuccessfully)
                {
                    return workTask.Result;
                }
            }
            throw new TimeoutException();
        }
    }
}
