using System;
using System.Threading;
using System.Threading.Tasks;

namespace durable_functions.Framework.BusAbstractions
{
    public interface IProcessReference
    {
        ValueTask Post(CommandMessage @event, CancellationToken cancellationToken);
    }
    
    public interface IProcessManager
    {
        ValueTask<IProcessReference> StartProcess(string processType, DateTime startTime, CancellationToken cancellationToken);
    }
}
