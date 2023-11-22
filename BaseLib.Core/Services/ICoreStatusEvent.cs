using System;

namespace BaseLib.Core.Services
{
    public interface ICoreStatusEvent
    {
        string? ServiceName { get; set; }
        CoreServiceStatus Status { get; set; }

        DateTimeOffset StartedOn { get; set; }
        DateTimeOffset FinishedOn { get; set; }

        string? OperationId { get; set; }
        string? CorrelationId { get; set; }
        object? Request { get; set; }
        object? Response { get; set; }
    }
}

