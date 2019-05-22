using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dodo.Bus
{
    public class NoConcurrencyTaskScheduler : TaskScheduler
{
   [ThreadStatic]
   private static bool _currentThreadIsProcessingItems;

   // The maximum concurrency level allowed by this scheduler. 
   private readonly int _maxDegreeOfParallelism = 1;

   // Indicates whether the scheduler is currently processing work items. 
   private int _delegatesQueuedOrRunning = 0;

   private Queue<Task> actionQueue = new Queue<Task>();

   private long processedTasksCount = 0;

   public long GetProcessedTaskCount()
   {
       return processedTasksCount;
   }

   protected sealed override void QueueTask(Task task)
   {
       actionQueue.Enqueue(task);
   }   

   public void ProcessPendingActions() {
       var l = new List<Task>();
       while (actionQueue.TryDequeue(out var task)) {
           l.Add(task);
       }
       foreach (var task in l)
       {
           processedTasksCount++;
           base.TryExecuteTask(task);
       }
   }

   public bool HasPendingWork() {
       return actionQueue.TryPeek(out var _);
   }

   // Attempts to execute the specified task on the current thread. 
   protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
   {
       if (taskWasPreviouslyQueued) {
           return false;
       }

       processedTasksCount++;
       return base.TryExecuteTask(task);
   }

   // Attempt to remove a previously scheduled task from the scheduler. 
   protected sealed override bool TryDequeue(Task task)
   {
       return false;
   }

   // Gets the maximum concurrency level supported by this scheduler. 
   public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

   // Gets an enumerable of the tasks currently scheduled on this scheduler. 
   protected sealed override IEnumerable<Task> GetScheduledTasks()
   {
       return actionQueue.ToList();
   }
}
}
