using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceBase
    {
        Task<CoreResponseBase> RunAsync(CoreRequestBase request, string? correlationId = null, bool isLongRunningChild = false);
    }

    public interface ICoreServiceBase<TRequest, TResponse> : ICoreServiceBase
        where TRequest : CoreRequestBase
        where TResponse : CoreResponseBase, new()
    {
        Task<TResponse> RunAsync(TRequest request, string? correlationId = null, bool isLongRunningChild = false);
    }
}