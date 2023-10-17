using System;

namespace BaseLib.Core.Services
{
    public interface ICoreServiceState
    {
        DateTimeOffset StartedOn { get; }
        DateTimeOffset FinishedOn { get; }
        
        string? OperationId { get; }
        string? CorrelationId { get; }
        ICoreServiceRequest? Request { get; }
        ICoreServiceResponse? Response { get; }

    }

}