using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public class NullCoreServiceJournal : ICoreServiceJournal
    {
        public Task BeginAsync(ICoreServiceState state)
        {
            return Task.CompletedTask;
        }

        public Task EndAsync(ICoreServiceState state)
        {
            return Task.CompletedTask;
        }
    }
}

