namespace BaseLib.Core.Models
{
    public class CoreStatusEvent 
    {
        public string? ServiceName { get; set; }
        public CoreServiceStatus Status { get; set; }
        public DateTimeOffset StartedOn { get; set; }
        public DateTimeOffset FinishedOn { get; set; }
        public string? OperationId { get; set; }
        public string? CorrelationId { get; set; }
        public object? Request { get; set; }
        public object? Response { get; set; }
    }
}

