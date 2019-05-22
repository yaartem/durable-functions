using System;
using System.Threading.Tasks;

namespace Dodo.Bus
{
    public interface ICoroutineRuntime
    {
        Task<T> Retry<T>(Func<Task<T>> func, DateTime dueTime, TimeSpan singleRetryTimeout);
    }
}
