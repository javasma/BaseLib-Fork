namespace BaseLib.Core.Models
{
    public class JournalEntry
    {
        public string? ServiceName { get; set; }

        public CoreServiceStatus Status { get; set; }

        public DateTimeOffset StartedOn { get; set; }

        public DateTimeOffset FinishedOn { get; set; }

        public string? OperationId { get; set; }

        public string? CorrelationId { get; set; }
        public bool Succeeded { get; set; }
        public CoreReasonCode ReasonCode { get; set; } = CoreReasonCode.Null;

        public string[] Messages { get; set; } = Array.Empty<string>();
        public bool IsLongRunning { get; set; }
        public bool IsLongRunningChild { get; set; }
    }





}