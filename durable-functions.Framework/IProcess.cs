using System.Threading.Tasks;

namespace Dodo.Bus
{
    public interface IProcess
    {
        Task Perform();
    }
}