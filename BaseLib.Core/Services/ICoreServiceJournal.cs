using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceJournal<TRequest, TResponse>
        where TRequest : ICoreServiceRequest
        where TResponse : ICoreServiceResponse
    {
        Task BeginAsync(TRequest request);
        Task EndAsync(TResponse response);
    }
}