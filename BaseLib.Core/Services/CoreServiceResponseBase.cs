using System;

namespace BaseLib.Core.Services
{

    public abstract class CoreServiceResponseBase : ICoreServiceResponse
    {
        public bool Succeeded { get; set; }
        public Enum ReasonCode { get; set; }
        public string Reason { get { return this.ReasonCode?.GetDescription(); } }
        public string[] Messages { get; set; }

        
    }

    public static class CoreServiceResponseFactory
    {
        public static TResponse Succeed<TResponse>(Enum reasonCode = null, params string[] messages)
            where TResponse : ICoreServiceResponse, new()
        {
            return new TResponse
            {
                Succeeded = true,
                ReasonCode = reasonCode ?? CoreServiceReasonCode.Succeeded,
                Messages = messages
            };
        }

        public static TResponse Fail<TResponse>(Enum reasonCode = null, params string[] messages)
            where TResponse : ICoreServiceResponse, new()
        {
            return new TResponse
            {
                Succeeded = false,
                ReasonCode = reasonCode ?? CoreServiceReasonCode.Failed,
                Messages = messages
            };
        }
    }


}
