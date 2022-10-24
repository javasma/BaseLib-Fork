using System.Threading.Tasks;
using Abc.RaffleOnline.Models;

namespace Abc.RaffleOnline.Raffles.OpenRaffle
{
    public interface IRaffleWriter
    {
        Task<int> WriteAsync(Raffle raffle);
    }
}