using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceBase<TRequest, TResponse>
        where TRequest : ICoreServiceRequest
        where TResponse : ICoreServiceResponse, new()
    {
        Task<TResponse> RunAsync(TRequest request);
    }
}