using System.Threading.Tasks;
using BaseLib.Core.Services;

namespace Abc.RaffleOnline
{
    public interface IJournalEventHandler
    {
        Task HandleAsync(ICoreStatusEvent statusEvent);
    }
}
