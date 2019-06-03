using System;
using System.Collections.Generic;

namespace durable_functions.DataAccess
{
    public class ProcessDto
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        
        public ICollection<InboxEventDto> InboxEvents { get; set; }
        
        public ICollection<OutboxEventDto> OutboxEvents { get; set; }

        public ProcessDto()
        {
            InboxEvents = new List<InboxEventDto>();
            OutboxEvents = new List<OutboxEventDto>();
        }
    }

    public class InboxEventDto
    {
        public string ProcessType { get; set; }
        public string ProcessId { get; set; }
        public long Version { get; set; }
        public byte[] Headers { get; set; }
        public byte[] Payload { get; set; }
    }
    
    public class OutboxEventDto
    {
        public string ProcessType { get; set; }
        public string ProcessId { get; set; }
        public long Version { get; set; }
        public byte[] Headers { get; set; }
        public byte[] Payload { get; set; }
    }
}
