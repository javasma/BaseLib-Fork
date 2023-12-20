using System.Text.Json;
using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    /// <summary>
    /// Stores event blob and write a record entry in the journal
    /// </summary>
    public class JournalEventHandler : IJournalEventHandler
    {
        private readonly IJournalEntryWriter journalWriter;
        private readonly ICoreStatusEventStore eventStore;

        public JournalEventHandler(IJournalEntryWriter journalWriter, ICoreStatusEventStore eventStore)
        {
            this.journalWriter = journalWriter;
            this.eventStore = eventStore;
        }

        public async Task<int> HandleAsync(CoreStatusEvent statusEvent)
        {
            if (statusEvent.Status == CoreServiceStatus.Finished)
            {
                var journalEntry = MapJournalEntry(statusEvent);

                await journalWriter.WriteAsync(journalEntry);

                await eventStore.WriteAsync(statusEvent);

                return 1;
            }
            return 0;
        }

        private JournalEntry MapJournalEntry(CoreStatusEvent statusEvent)
        {
            //TODO, serialization of the event 
            var partialResponse = JsonSerializer.Deserialize<PartialResponse>(JsonSerializer.Serialize(statusEvent.Response)) ?? new PartialResponse();

            return new JournalEntry
            {
                ServiceName = statusEvent.ServiceName,
                Status = statusEvent.Status,
                StartedOn = statusEvent.StartedOn,
                FinishedOn = statusEvent.FinishedOn,
                OperationId = statusEvent.OperationId,
                CorrelationId = statusEvent.CorrelationId,
                Succeeded = partialResponse.Succeeded,
                ReasonCode = partialResponse.ReasonCode,
                Messages = partialResponse.Messages ?? Array.Empty<string>()
            };

        }

        private class PartialResponse
        {
            public bool Succeeded { get; set; }
            public string[]? Messages { get; set; }
            public CoreReasonCode ReasonCode { get; set; } = CoreReasonCode.Null;
            public string? TransactionId { get; set; }
        }
    }
}