using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceBase
    {
        Task<CoreServiceResponseBase> RunAsync(CoreServiceRequestBase request, string? correlationId = null);
    }

    public interface ICoreServiceBase<TRequest, TResponse> : ICoreServiceBase
        where TRequest : CoreServiceRequestBase
        where TResponse : CoreServiceResponseBase, new()
    {
        Task<TResponse> RunAsync(TRequest request, string? correlationId = null);
    }
}