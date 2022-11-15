using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public interface ICoreStatusEventSink
    {
        Task WriteAsync(ICoreStatusEvent statusEvent);
    }
}