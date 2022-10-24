using System;
using System.Threading.Tasks;
using Abc.RaffleOnline.Models;
using BaseLib.Core.Services;

namespace Abc.RaffleOnline.Raffles.OpenRaffle
{
    public class OpenRaffleService : RaffleServiceBase<OpenRaffleRequest, OpenRaffleResponse>
    {
        private readonly IRaffleWriter writer;

        public OpenRaffleService(IRaffleWriter writer, ICoreServiceJournal journal)
            : base(journal)
        {
            this.writer = writer;
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