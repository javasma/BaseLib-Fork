using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public class NullCoreEventSink : ICoreStatusEventSink
    {
        public Task WriteAsync(ICoreStatusEvent statusEvent)
        {
             return Task.CompletedTask;
        }
    }
}

