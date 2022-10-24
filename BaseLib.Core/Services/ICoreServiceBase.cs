using System;
using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceBase
    {
        Task<ICoreServiceResponse> RunAsync(ICoreServiceRequest request, string correlationId = null);
    }

    public interface ICoreServiceBase<TRequest, TResponse> : ICoreServiceBase
        where TRequest : ICoreServiceRequest
        where TResponse : ICoreServiceResponse, new()
    {
        Task<TResponse> RunAsync(TRequest request, string correlationId = null);
    }
}