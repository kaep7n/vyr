using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Vyr.Core;

namespace Vyr.Isolation
{
    public interface IIsolation
    {
        Task IsolateAsync(AgentOptions options);

        Task FreeAsync();
    }
}
