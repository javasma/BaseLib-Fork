using System.Threading.Tasks;
using Abc.RaffleOnline.Models;
using Abc.RaffleOnline.Raffles.OpenRaffle;
using Microsoft.AspNetCore.Mvc;

namespace Abc.RaffleOnline.Serverless.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RaffleController : ControllerBase
    {
        private readonly OpenRaffleService openRaffleService;

        public RaffleController(OpenRaffleService openRaffleService)
        {
            this.openRaffleService = openRaffleService;
        }

        [HttpGet]
        public Task<Raffle[]> GetAsync()
        {
            var result = new Raffle[]{ new Raffle()};

            return Task.FromResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> OpenAsync([FromBody] OpenRaffleRequest request)
        {
            var result = await this.openRaffleService.RunAsync(request);

            return new CreatedResult("", result);
        }
    }

}