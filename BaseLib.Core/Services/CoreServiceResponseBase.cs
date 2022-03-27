using System;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceResponse
    {
        bool Succeeded { get; set; }
        string Reason { get; }
        System.Enum ReasonCode { get; set; }
        string[] Messages { get; set; }
    }

    public interface ICoreServiceResponse<TReasonCode> : ICoreServiceResponse
        where TReasonCode : System.Enum
    {
        new TReasonCode ReasonCode { get; set; }
    }

    public abstract class CoreServiceResponseBase<TReasonCode> : ICoreServiceResponse<TReasonCode>
        where TReasonCode : System.Enum
    {
        public bool Succeeded { get; set; }
        public TReasonCode ReasonCode { get; set; }
        public string Reason { get { return this.ReasonCode.GetDescription(); } }
        public string[] Messages { get; set; }
        Enum ICoreServiceResponse.ReasonCode { get { return this.ReasonCode; } set { this.ReasonCode = (TReasonCode)value; } }
    }
}
