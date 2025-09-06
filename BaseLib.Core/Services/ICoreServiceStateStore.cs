namespace BaseLib.Core.Services
{
    public interface ICoreServiceStateStore
    {
        Task WriteAsync(string operationId, IDictionary<string, object?> state);
        Task<IDictionary<string, object?>> ReadAsync(string operationId);
    }
}