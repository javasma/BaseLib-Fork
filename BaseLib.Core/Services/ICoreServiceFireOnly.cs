using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceFireOnly

    {
        Task FireAsync<TService>(CoreRequestBase request, string? correlationId = null, bool isLongRunningChild = false)
            where TService : ICoreServiceBase;
        Task FireAsync(string typeName, CoreRequestBase request, string? correlationId = null, bool isLongRunningChild = false);
        Task FireManyAsync<TService>(IEnumerable<CoreRequestBase> requests, string? correlationId = null, bool isLongRunningChild = false)
            where TService : ICoreServiceBase;
        Task ResumeAsync<TService>(string operationId, string? correlationId = null)
            where TService : ICoreLongRunningService;
        Task ResumeAsync(string typeName, string operationId, string? correlationId = null);

    }
}