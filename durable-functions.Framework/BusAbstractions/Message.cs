using System;

namespace durable_functions.Framework.BusAbstractions
{
    /// <summary>
    /// ProcessCommandMessage to existing process inbox or starts a process.
    /// </summary>
    public struct CommandMessage
    {
        public string ProcessType { get; set; }
        public string ProcessId { get; set; }
        public long Version { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public ReadOnlyMemory<byte> Headers { get; set; }
        public ReadOnlyMemory<byte> Payload { get; set; }
    }

    /// <summary>
    /// InboxEvent to consume inside user process code.
    /// </summary>
    public struct InboxEvent
    {
        public string ProcessType { get; set; }
        public string ProcessId { get; set; }
        public long Version { get; set; }
        
        public ReadOnlyMemory<byte> Headers { get; set; }
        public ReadOnlyMemory<byte> Payload { get; set; }
    }
}
