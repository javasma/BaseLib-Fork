namespace BaseLib.Core.Models
{
    public abstract class CoreServiceResponseBase
    {
        public bool Succeeded { get; set; }
        public CoreReasonCode ReasonCode { get; set; } = CoreReasonCode.Null;
        public string[] Messages { get; set; } = Array.Empty<string>();
    }
}
