using System.Threading.Tasks;
using Abc.RaffleOnline.Models;

namespace Abc.RaffleOnline.Raffles.OpenRaffle.InProc
{
    public class RaffleInProcWriter : IRaffleWriter
    {
        public Task<int> WriteAsync(Raffle raffle)
        {
            return Task.FromResult(1);
        }
    }
}   