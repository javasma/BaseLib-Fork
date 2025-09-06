using BaseLib.Core.Models;
using BaseLib.Core.Services;

namespace BaseLib.Core.Tests.Services
{
    internal class LongRunningMasterService : CoreLongRunningServiceBase<LongRunningMasterRequest, LongRunningMasterResponse>, ICoreServiceBase
    {
        private int numberOfChildrenFired;

        public LongRunningMasterService(ICoreServiceFireOnly invoker, ICoreServiceStateStore stateStore)
            : base(invoker, stateStore)
        {
        }

        protected override async Task<LongRunningMasterResponse> RunAsync()
        {
            // In a real scenario, this would fire off multiple child tasks
            for (int i = 0; i < this.Request.NumberOfChildren; i++)
            {
                var childRequest = new LongRunningChildRequest { Payload = $"Child {i}" };
                await this.FireAsync<LongRunningChildService>(childRequest);

                numberOfChildrenFired++;
            }

            return new LongRunningMasterResponse { Succeeded = true, NumberOfChildrenFired = numberOfChildrenFired };
        }

        protected override async Task<LongRunningMasterResponse> ResumeAsync()
        {
            await Task.Delay(100); // Simulate some work
            return new LongRunningMasterResponse
            {
                Succeeded = true,
                Messages = ["Resumed"],
                NumberOfChildrenFired = numberOfChildrenFired
            };
        }
    }


    internal class LongRunningMasterResponse : CoreResponseBase
    {
        public int NumberOfChildrenFired { get;  set; }
    }

    internal class LongRunningMasterRequest : CoreRequestBase
    {
        public int NumberOfChildren { get; set; }
    }


}