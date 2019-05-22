using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dodo.Bus
{
    public static class AwaitExtensions
    {
        public static async Task<IEnumerable<T>> WhenAllSpecialized<T>(this IEnumerable<Task<T>> source)
        {
            var l = new List<T>();
            foreach (var i in source)
            {
                l.Add(await i);
            }
            return l;
        }

        public static async Task WhenAll(this IEnumerable<Task> source)
        {
            foreach (var i in source)
            {
                await i;
            }
        }

        public static Task<Task<TResult>> WhenAnySpecialized<TResult>(params Task<TResult>[] source)
        {
            Action<Task<TResult>, object> continuationFunction =
                (t, state) =>
                {
                    var c = (TaskCompletionSource<Task<TResult>>) state;
                    if (t.IsFaulted)
                    {
                        c.TrySetException(t.Exception);
                    }
                    else if (t.IsCanceled)
                    {
                        c.TrySetCanceled();
                    }
                    else
                    {
                        c.TrySetResult(t);
                    }
                };

            var completion = new TaskCompletionSource<Task<TResult>>();

            foreach (var i in source)
            {
                i.ContinueWith(continuationFunction, completion);
            }
            
            return completion.Task;
        }
        
        public static Task<Task> WhenAnySpecialized(params Task[] source)
        {
            return WhenAnySpecialized((IEnumerable<Task>)source);
            //await Task.WhenAny(source);
            //await Task.Yield();
        }

        public static Task<Task> WhenAnySpecialized(this IEnumerable<Task> source)
        {
            Action<Task, object> continuationFunction =
                (t, state) =>
                {
                    var c = (TaskCompletionSource<Task>) state;
                    if (t.IsFaulted)
                    {
                        c.TrySetException(t.Exception);
                    }
                    else if (t.IsCanceled)
                    {
                        c.TrySetCanceled();
                    }
                    else
                    {
                        c.TrySetResult(t);
                    }
                };

            var completion = new TaskCompletionSource<Task>();

            foreach (var i in source)
            {
                i.ContinueWith(continuationFunction, completion);
            }
            
            return completion.Task;
            /*
            Action<Task, object> continuationFunction =
                (t, state) =>
                {
                    var c = (TaskCompletionSource<bool>) state;
                    if (t.IsFaulted)
                    {
                        c.TrySetException(t.Exception);
                    }
                    else if (t.IsCanceled)
                    {
                        c.TrySetCanceled();
                    }
                    else
                    {
                        c.TrySetResult(true);
                    }
                };

            var completion = new TaskCompletionSource<bool>();

            foreach (var i in source)
            {
                i.ContinueWith(continuationFunction, completion);
            }
            
            return completion.Task;
            */
            //await Task.WhenAny(source);
            //await Task.Yield();
        }
    }
}