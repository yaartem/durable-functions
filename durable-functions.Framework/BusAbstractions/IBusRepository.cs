using System;
using System.Threading;
using System.Threading.Tasks;

namespace durable_functions.Framework.BusAbstractions
{
    public interface IBusRepository
    {
        ValueTask<ProcessAbstractions.Process> GetCurrentlyRunningProcesses(DateTime createdSince, CancellationToken cancellationToken);
        ValueTask<InboxEvent> ReceiveNextEventAsync(string processType, string processId, long afterVersion, CancellationToken cancellationToken);
        ValueTask WriteAsync(InboxEvent @event, CancellationToken cancellationToken);
        ValueTask<CommandMessage> ReceiveNextCommandAsync(string processId);
        ValueTask WriteAsync(CommandMessage @event, CancellationToken cancellationToken);
    }
}