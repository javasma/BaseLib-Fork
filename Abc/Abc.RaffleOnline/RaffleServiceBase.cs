using BaseLib.Core.Services;

namespace Abc.RaffleOnline
{
    public abstract class RaffleServiceBase<TRequest, TResponse> : CoreServiceBase<TRequest, TResponse>, IRaffleService<TRequest, TResponse> 
        where TRequest: RaffleServiceRequest
        where TResponse: RaffleServiceResponse, new()
    {
        public RaffleServiceBase(ICoreServiceJournal journal = null)
            : base(journal: journal)   
        {
            
        }
    }
}