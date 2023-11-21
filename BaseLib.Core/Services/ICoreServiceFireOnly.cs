using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceFireOnly
        
    {
        Task FireAsync<TService>(ICoreServiceRequest request, string? correlationId = null)
            where TService : ICoreServiceBase;
    }
}