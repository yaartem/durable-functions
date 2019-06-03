using System.Threading.Tasks;
using durable_functions.Framework.BusAbstractions;

namespace durable_functions.Framework.ProcessAbstractions
{
    public interface ICommandApi
    {
        void SendCommand(CommandMessage message);
    }

    public interface IChannel
    {
        ValueTask<InboxEvent> Receive();
    }

    public interface IChannelFactory
    {
        IChannel Create();
    }
}
