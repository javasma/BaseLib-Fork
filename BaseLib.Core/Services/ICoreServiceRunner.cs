using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceRunner
    {
        Task<CoreResponseBase> RunAsync(string typeName, CoreRequestBase request, string? correlationId = null, bool IsLongRunningChild = false);
        Task<CoreResponseBase> ResumeAsync(string typeName, string operationId);        
    }
}
