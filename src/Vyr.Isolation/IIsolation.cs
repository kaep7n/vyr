using System.Threading.Tasks;

namespace Vyr.Isolation
{
    public interface IIsolation
    {
        Task IsolateAsync(IsolationConfiguration isolationConfiguration);

        Task RunAsync();

        Task IdleAsync();

        Task FreeAsync();
    }
}
