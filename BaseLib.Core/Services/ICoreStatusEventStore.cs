using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    /// <summary>
    /// Abstraction to write the events to a StatusEvent repository
    /// </summary>
    public interface ICoreStatusEventStore
    {
        Task<int> WriteAsync(CoreStatusEvent statusEvent);
        Task<CoreStatusEvent> ReadAsync(string correlationId);

    }
}