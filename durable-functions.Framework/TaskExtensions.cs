using System;
using System.Threading.Tasks;

namespace Dodo.Bus
{
    public static class TaskExtensions
    {
        public static Func<Task<T>> TaskFunc<T>(this Func<Task<T>> func)
        {
            return func;
        }
        public static Task<T> Retry<T>(this ICoroutineRuntime runtine, Func<Task<T>> func, DateTime dueTime, TimeSpan singleRetryTimeout)
        {
            throw new NotImplementedException();
        }
    }
}
