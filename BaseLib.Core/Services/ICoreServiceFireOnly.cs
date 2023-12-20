using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceFireOnly
        
    {
        Task FireAsync<TService>(CoreServiceRequestBase request, string? correlationId = null)
            where TService : ICoreServiceBase;
    }
}