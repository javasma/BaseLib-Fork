namespace BaseLib.Core.Services
{
    public interface IJournalEventHandler
    {
        Task<int> HandleAsync(ICoreStatusEvent statusEvent);
    }
}