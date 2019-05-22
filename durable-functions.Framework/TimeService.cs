using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dodo.Bus
{
    public class TimeService : ITimeService
    {
        DateTime now;
        private long requests = 0;
        Task awaitable;
        DateTime currentTime;
        TaskScheduler scheduler;
        SortedList<DateTime, TaskCompletionSource<bool>> awaitingList = new SortedList<DateTime, TaskCompletionSource<bool>>();

        public long GetRequestAmount() {
            return requests;
        }

        public Task GetAwaitable(DateTime dateTime)
        {
            requests ++;
            var key = dateTime;
            var ts = new TaskCompletionSource<bool>();
            lock (this)
            {
                if (awaitingList.TryGetValue(key, out var existing))
                {
                    return existing.Task;
                }
                awaitingList.Add(key, ts);
            }
            return ts.Task;
        }
        
        public void ProcessRealtimeTicks()
        {
            DateTime? found = null;
            TaskCompletionSource<bool> completion = null;
            foreach (var item in awaitingList)
            {
                if (item.Key <= now)
                {
                    completion = item.Value;
                    found = item.Key;
                }
                else
                {
                    break;
                }
            }
            if (found != null)
            {
                awaitingList.Remove(found.Value);
                completion.SetResult(true);
            }
        }

        public DateTime GetCurrent()
        {
            return now;
        }

        public void SetCurrentTime(DateTime now)
        {
            if (this.now > now) {
                throw new InvalidOperationException("can't go back in time");
            }

            this.now = now;
        }
    }
}
