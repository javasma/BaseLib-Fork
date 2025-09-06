using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public interface ICoreLongRunningService 
    {
        Task<CoreResponseBase> ResumeAsync(string operationId);
    }

    public interface ICoreLongRunningService<TResponse> : ICoreLongRunningService
        where TResponse : CoreResponseBase, new()
    {
        new Task<TResponse> ResumeAsync(string operationId);
    }
}