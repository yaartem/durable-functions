using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dodo.Bus
{
    public class CoroutineRuntime : ICoroutineRuntime
    {
        readonly ITimeService timeService;
        public CoroutineRuntime(ITimeService timeService)
        {
            this.timeService = timeService;
        }

        public async Task<(T1, T2)> AtTheSameTime<T1, T2>(Task<T1> a, Task<T2> b)
        {
            throw new NotImplementedException();
        }
        
        public async Task<T> Retry<T>(Func<Task<T>> func, DateTime dueTime, TimeSpan singleRetryTimeout)
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
                //Console.WriteLine($"{DateTime.Now} Retrying");
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
