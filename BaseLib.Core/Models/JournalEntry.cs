using BaseLib.Core.Services;

namespace BaseLib.Core.Models
{
    public class JournalEntry : IJournalEntry
    {
        public string? ServiceName { get; set; }

        public CoreServiceStatus Status { get; set; }

        public DateTimeOffset StartedOn { get; set; }

        public DateTimeOffset FinishedOn { get; set; }

        public string? OperationId { get; set; }

        public string? CorrelationId { get; set; }
        public bool Succeeded { get; set; }
        public int ReasonCode { get; set; }
        public string? Reason { get; set; }
        public string[] Messages { get; set; } = Array.Empty<string>();
    }





}