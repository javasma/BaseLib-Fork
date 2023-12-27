using BaseLib.Core.Models;
using BaseLib.Core.Serialization;

namespace BaseLib.Core.Services
{
    public class CoreServiceRunner
    {
        private readonly Func<string, ICoreServiceBase> serviceFactory;

        public CoreServiceRunner(Func<string, ICoreServiceBase> serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public Task<CoreResponseBase> RunAsync(string typeName, CoreRequestBase request)
        {
            var service = this.serviceFactory.Invoke(typeName);
            return service.RunAsync(request);
        }

        public Task<CoreResponseBase> RunAsync(string body)
        {
            var payload = CoreSerializer.Deserialize<Payload>(body)
                ?? throw new NullReferenceException("Unable to deserialize payload");
            var typeName = payload.Service
                ?? throw new NullReferenceException("No Service Name on payload");
            var request = payload.Request
                ?? throw new NullReferenceException("No Request on payload");
            return RunAsync(typeName, request);
        }
    }

    internal class Payload
    {
        public string? Service { get; set; }
        public CoreRequestBase? Request { get; set; }
    }
}