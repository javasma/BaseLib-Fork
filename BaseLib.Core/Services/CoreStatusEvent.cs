using System;

namespace BaseLib.Core.Services
{
    public class CoreStatusEvent : ICoreStatusEvent
    {
        public string ServiceName { get; set; }
        public CoreServiceStatus Status { get; set; }
        public string OperationId { get; set; }
        public string CorrelationId { get; set; }
        public DateTimeOffset StartedOn { get; set; }
        public DateTimeOffset FinishedOn { get; set; }
        public ICoreServiceRequest Request { get; set; }
        public ICoreServiceResponse Response { get; set; }
    }
}

