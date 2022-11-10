using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceFireOnly
        
    {
        Task FireAsync<TService>(ICoreServiceRequest request)
            where TService : ICoreServiceBase;
    }
}