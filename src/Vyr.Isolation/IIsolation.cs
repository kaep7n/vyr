using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Vyr.Isolation
{
    public interface IIsolation
    {
        Task IsolateAsync();

        Task FreeAsync();
    }
}
