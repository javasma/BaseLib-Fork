namespace BaseLib.Core.Models
{
    public abstract class CoreResponseBase
    {
        public string? OperationId { get; set; }
        public bool Succeeded { get; set; }
        public CoreReasonCode ReasonCode { get; set; } = CoreReasonCode.Null;
        public string[] Messages { get; set; } = [];
    }
}
