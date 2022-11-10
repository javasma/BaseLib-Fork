using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BaseLib.Core.Services
{
    public class CoreServiceRunner
    {
        private readonly Func<string, ICoreServiceBase> serviceFactory;

        public CoreServiceRunner(Func<string, ICoreServiceBase> serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public Task<ICoreServiceResponse> RunAsync(string typeName, ICoreServiceRequest request)
        {
            var service = this.serviceFactory.Invoke(typeName);
            return service.RunAsync(request);
        }

        public Task<ICoreServiceResponse> RunAsync(string body)
        {
            var payload = JsonConvert.DeserializeObject<Payload>(body);
            return RunAsync(payload.Service, payload.Request as ICoreServiceRequest);
        }
    }

    internal class Payload
    {
        public string Service { get; set; }
        public object Request { get; set; }
    }
}