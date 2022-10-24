using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceJournal
    {
        Task BeginAsync(ICoreServiceState state);
        Task EndAsync(ICoreServiceState state);
    }
}