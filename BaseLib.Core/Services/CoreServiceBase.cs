using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public abstract class CoreServiceBase<TRequest, TResponse> : ICoreServiceBase<TRequest, TResponse>
        where TRequest : ICoreServiceRequest
        where TResponse : ICoreServiceResponse, new()
    {
        private TRequest request;
        private TResponse response;

        protected TRequest Request { get { return this.request; } }

        protected TResponse Response { get { return this.response; } }

        public async Task<TResponse> RunAsync(TRequest args)
        {
            this.request = args;
            this.response = new TResponse();

            await RunAsync();

            //Revise Estado del servicio 

            this.response.Succeeded = true;

            return this.response;
        }

        protected abstract Task RunAsync();
    }
    public interface ICoreServiceRequest
    {
    }

}

