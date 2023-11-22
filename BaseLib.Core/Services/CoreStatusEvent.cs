using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BaseLib.Core.Services
{
    public class CoreStatusEvent : ICoreStatusEvent
    {
        public string? ServiceName { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public CoreServiceStatus Status { get; set; }
        public DateTimeOffset StartedOn { get; set; }
        public DateTimeOffset FinishedOn { get; set; }
        public string? OperationId { get; set; }
        public string? CorrelationId { get; set; }
        public object? Request { get; set; }
        public object? Response { get; set; }
    }
}

