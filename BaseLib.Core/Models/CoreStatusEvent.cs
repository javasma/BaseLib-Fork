namespace BaseLib.Core.Models
{
    public class CoreStatusEvent
    {
        public string? ModuleName { get; set; }
        public string? ServiceName { get; set; }
        public CoreServiceStatus Status { get; set; }
        public DateTimeOffset StartedOn { get; set; }
        public DateTimeOffset FinishedOn { get; set; }
        public string? OperationId { get; set; }
        public string? CorrelationId { get; set; }
        public CoreRequestBase? Request { get; set; }
        public CoreResponseBase? Response { get; set; }
    }
}

