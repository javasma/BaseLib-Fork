using BaseLib.Core.Models;
using Newtonsoft.Json;

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

        public async Task<int> HandleAsync(ICoreStatusEvent statusEvent)
        {
            if (statusEvent.Status == CoreServiceStatus.Finished)
            {
                JournalEntry journalEntry = MapJournalEntry(statusEvent);

                await journalWriter.WriteAsync(journalEntry);

                await eventStore.WriteAsync(statusEvent);

                return 1;
            }
            return 0;
        }

        private JournalEntry MapJournalEntry(ICoreStatusEvent statusEvent)
        {
            //TODO, serialization of the event 
            var partialResponse = JsonConvert.DeserializeObject<PartialResponse>(JsonConvert.SerializeObject(statusEvent.Response)) ?? new PartialResponse();

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
                Reason = partialResponse.Reason,
                Messages = partialResponse.Messages ?? new string[0]
            };

        }

        private class PartialResponse
        {
            public bool Succeeded { get; set; }
            public string? Reason { get; set; }
            public string[]? Messages { get; set; }
            public int ReasonCode { get; set; }
            public string? TransactionId { get; set; }
        }
    }
}