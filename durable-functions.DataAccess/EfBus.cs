using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using durable_functions.Framework.BusAbstractions;
using durable_functions.Framework.ProcessAbstractions;
using Microsoft.EntityFrameworkCore;

namespace durable_functions.DataAccess
{
    public class EfBusRepository : IBusRepository
    {
        public async ValueTask<Process> GetCurrentlyRunningProcesses(DateTime createdSince, CancellationToken cancellationToken)
        {
            using (var ctx = CreateContext())
            {
                var fields = await ctx.Processes
                    .Where(p => p.CreatedAtUtc >= createdSince)
                    .OrderBy(p => p.CreatedAtUtc)
                    .Select(p => new {p.Type, p.Id})
                    .SingleOrDefaultAsync(cancellationToken);

                return new Process
                {
                    Type = fields.Type,
                    Id = fields.Id
                };
            }
        }

        /// <summary>
        /// ReceiveNextAsync receives the next event after <paramref name="afterVersion"/> in ascending order.
        /// </summary>
        /// <param name="processType">Type of the process.</param>
        /// <param name="processId">The process for which the events are received.</param>
        /// <param name="afterVersion">Exclusive lower bound of the events to look for the next event.</param>
        /// <returns>Returns the next event after the version specified.</returns>
        public async ValueTask<InboxEvent> ReceiveNextEventAsync(string processType, string processId, long afterVersion,
            CancellationToken cancellationToken)
        {
            using (var ctx = CreateContext())
            {
                var fields = await ctx.Processes
                    .Where(p => p.Type == processType && p.Id == processId)
                    .SelectMany(p => p.InboxEvents
                        .Where(e => e.Version > afterVersion))
                    .OrderBy(e => e.Version)
                    .Select(e => new
                    {
                        e.ProcessType,
                        e.ProcessId,
                        e.Version,
                        e.Headers,
                        e.Payload
                    })
                    .FirstOrDefaultAsync(cancellationToken);
                return new InboxEvent
                {
                    ProcessType = fields.ProcessType,
                    ProcessId = fields.ProcessId,
                    Version = fields.Version,
                    Headers = fields.Headers.AsMemory(),
                    Payload = fields.Payload.AsMemory()
                };
            }
        }

        public async ValueTask WriteAsync(InboxEvent @event, CancellationToken cancellationToken)
        {
            var createdAt = DateTime.UtcNow;
            
            var process = new ProcessDto {
                Id = @event.ProcessId,
                CreatedAtUtc = createdAt,
                Type = @event.ProcessType,
            };

            var eventDto = new InboxEventDto
            {
                ProcessType = @event.ProcessType,
                ProcessId = @event.ProcessId,
                Payload = @event.Payload.ToArray(),
                Headers = @event.Headers.ToArray(),
                Version = @event.Version
            };
            
            using (var ctx = CreateContext())
            {
                ctx.Processes.Attach(process);
                process.InboxEvents.Add(eventDto);
                await ctx.SaveChangesAsync(cancellationToken);
            }
        }

        public ValueTask<CommandMessage> ReceiveNextCommandAsync(string processId)
        {
            throw new NotImplementedException();
        }

        public async ValueTask WriteAsync(CommandMessage @event, CancellationToken cancellationToken)
        {
            var createdAt = DateTime.UtcNow;
            
            var process = new ProcessDto {
                Id = @event.ProcessId,
                CreatedAtUtc = createdAt,
                Type = @event.ProcessType
            };

            var eventDto = new OutboxEventDto
            {
                ProcessType = @event.ProcessType,
                ProcessId = @event.ProcessId,
                Payload = @event.Payload.ToArray(),
                Headers = @event.Headers.ToArray(),
                Version = @event.Version
            };
            
            using (var ctx = CreateContext())
            {
                ctx.Processes.Attach(process);
                process.OutboxEvents.Add(eventDto);
                await ctx.SaveChangesAsync(cancellationToken);
            }
        }

        private DurableFunctionsContext CreateContext() => new DurableFunctionsContext();
    }
}

