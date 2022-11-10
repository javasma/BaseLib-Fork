using BaseLib.Core.Services;

namespace Abc.RaffleOnline
{
    public interface IRaffleService : ICoreServiceBase
    {

    }
    public interface IRaffleService<TRequest, TResponse> : ICoreServiceBase<TRequest, TResponse>, IRaffleService
        where TRequest: RaffleServiceRequest
        where TResponse: RaffleServiceResponse, new()
    {

    }
}