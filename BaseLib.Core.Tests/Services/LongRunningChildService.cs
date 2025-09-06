using BaseLib.Core.Models;
using BaseLib.Core.Services;

namespace BaseLib.Core.Tests.Services
{
    // Test implementation for child service
    internal class LongRunningChildService : CoreServiceBase<LongRunningChildRequest, LongRunningChildResponse>
    {

        public LongRunningChildService()
            : base()
        {
        }

        protected override async Task<LongRunningChildResponse> RunAsync()
        {
            await Task.Delay(100); // Simulate some async work
            return new LongRunningChildResponse { Succeeded = true };
        }
    }


    internal class LongRunningChildResponse : CoreResponseBase
    {
    }

    internal class LongRunningChildRequest : CoreRequestBase
    {
        public string? Payload { get; set; }
    }
}
