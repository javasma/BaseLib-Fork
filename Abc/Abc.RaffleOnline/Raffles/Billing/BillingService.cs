using System.Threading.Tasks;
using BaseLib.Core.Services;

namespace Abc.RaffleOnline.Raffles.Billing
{
    public class BillingService : RaffleServiceBase<BillingRequest, BillingResponse>, IBillingService
    {
        public BillingService(ICoreServiceJournal journal)
            : base(journal)
        {
            
        }

        protected async override Task<BillingResponse> RunAsync()
        {
            await Task.CompletedTask;
            return Succeed(RaffleReasonCode.BillingSucceeded);
        }
    }
}