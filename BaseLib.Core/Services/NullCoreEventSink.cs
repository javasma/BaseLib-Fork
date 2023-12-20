using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public class NullCoreEventSink : ICoreStatusEventSink
    {
        public Task WriteAsync(CoreStatusEvent statusEvent)
        {
             return Task.CompletedTask;
        }
    }
}

