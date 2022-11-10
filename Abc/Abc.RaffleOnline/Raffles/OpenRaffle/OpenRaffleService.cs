using System;
using System.Threading.Tasks;
using Abc.RaffleOnline.Models;
using BaseLib.Core.Services;
using Abc.RaffleOnline.Raffles.Billing;

namespace Abc.RaffleOnline.Raffles.OpenRaffle
{
    public class OpenRaffleService : RaffleServiceBase<OpenRaffleRequest, OpenRaffleResponse>, IOpenRaffleService
    {
        private readonly IRaffleWriter writer;
        private readonly ICoreServiceFireOnly serviceInvoker;

        public OpenRaffleService(
            IRaffleWriter writer, 
            ICoreServiceFireOnly serviceInvoker,
            ICoreServiceJournal journal)
            : base(journal)
        {
            this.writer = writer;
            this.serviceInvoker = serviceInvoker;
        }

        protected override async Task<OpenRaffleResponse> RunAsync()
        {
            var raffle = new Raffle
            {
                Id = Guid.NewGuid().ToString(),
                Name = this.Request.Name,
                Description = this.Request.Description,
                BeginsOn = this.Request.BeginsOn,
                EndsOn = this.Request.EndsOn
            };

            await this.writer.WriteAsync(raffle);

            //fire and forget billing
            await this.serviceInvoker.FireAsync<IBillingService>(new BillingRequest{
                Raffle = raffle
            });

            return new OpenRaffleResponse
            {
                Succeeded = true,
                Raffle = raffle
            };
        }
    }

       

    public class OpenRaffleRequest : RaffleServiceRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset BeginsOn { get; set; }
        public DateTimeOffset EndsOn { get; set; }
    }

    public class OpenRaffleResponse : RaffleServiceResponse
    {
        public Raffle Raffle { get; set; }
    }

}