using System.Threading.Tasks;

namespace Vyr.Hosting
{
    public interface IHost
    {
        Task UpAsync();

        Task DownAsync();
    }
}
