using Abc.Financial.Models;
using System.Threading.Tasks;

namespace Abc.Financial.Services.CreateQuote
{
    public interface IInstrumentReader
    {
        Task<Instrument> ReadAsync(object identifier);
    }
}