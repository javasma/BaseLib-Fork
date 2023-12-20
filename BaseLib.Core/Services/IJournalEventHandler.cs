using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public interface IJournalEventHandler
    {
        Task<int> HandleAsync(CoreStatusEvent statusEvent);
    }
}