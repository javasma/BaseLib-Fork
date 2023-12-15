using BaseLib.Core.Services;

namespace BaseLib.Core.Models
{
    public interface IJournalEntry
    {
        string? ServiceName { get; set; }

        CoreServiceStatus Status { get; set; }

        DateTimeOffset StartedOn { get; set; }

        DateTimeOffset FinishedOn { get; set; }

        string? OperationId { get; set; }

        string? CorrelationId { get; set; }
        bool Succeeded { get; set; }
        int ReasonCode { get; set; }
        string? Reason { get; set; }
        string[] Messages { get; set; }
    }
    

    
   
    
}