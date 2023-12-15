namespace BaseLib.Core.Services
{
    /// <summary>
    /// Abstraction to write the events to a StatusEvent repository
    /// </summary>
    public interface ICoreStatusEventStore
    {
        Task<int> WriteAsync(ICoreStatusEvent statusEvent);
        Task<ICoreStatusEvent> ReadAsync(string correlationId);

    }
}