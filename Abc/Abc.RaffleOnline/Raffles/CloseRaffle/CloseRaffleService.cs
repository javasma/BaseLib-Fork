using System.Threading.Tasks;
using Abc.RaffleOnline.Models;

namespace Abc.RaffleOnline.Raffles.CloseRaffle
{
    public class CloseRaffleService : RaffleServiceBase<CloseRaffleRequest, CloseRaffleResponse>
    {
        protected override async Task<CloseRaffleResponse> RunAsync()
        {
            await Task.FromResult(0);

            return new CloseRaffleResponse
            {
                Succeeded = true
            };
        }
    }

    public class CloseRaffleRequest : RaffleServiceRequest
    {
        public string RaffleId{get;set;}
    }

    public class CloseRaffleResponse : RaffleServiceResponse
    {
        public Raffle Raffle { get; set; }
    }

}