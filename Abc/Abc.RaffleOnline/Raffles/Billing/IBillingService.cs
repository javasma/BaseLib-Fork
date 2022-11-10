using Abc.RaffleOnline.Models;

namespace Abc.RaffleOnline.Raffles.Billing
{
    public interface IBillingService : IRaffleService<BillingRequest, BillingResponse>
    {
    }

    public class BillingRequest : RaffleServiceRequest
    {
        public Raffle Raffle { get;  set; }
    }

    public class BillingResponse : RaffleServiceResponse
    {
    }

}