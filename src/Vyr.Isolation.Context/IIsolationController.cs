using System.Threading.Tasks;

namespace Vyr.Isolation.Context
{
    public interface IIsolationController
    {
        Task IdleAsync();
        Task RunAsync();
    }
}