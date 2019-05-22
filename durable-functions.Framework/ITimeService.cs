using System;
using System.Threading.Tasks;

namespace Dodo.Bus
{
    public interface ITimeService
    {
        Task GetAwaitable(DateTime dueDateTime);

        DateTime GetCurrent();
    }
}
