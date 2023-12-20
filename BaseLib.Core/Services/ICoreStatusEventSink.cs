using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public interface ICoreStatusEventSink
    {
        Task WriteAsync(CoreStatusEvent statusEvent);
    }
}