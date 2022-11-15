using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using BaseLib.Core.Services;

namespace Abc.RaffleOnline
{
    public class RaffleJournalEventHandler : IJournalEventHandler
    {
        public Task HandleAsync(ICoreStatusEvent statusEvent)
        {
            Trace.WriteLine($"{statusEvent.ServiceName}::{statusEvent.OperationId}::{statusEvent.Status.ToString()}");
            
            if( statusEvent.Status == CoreServiceStatus.Started)
            {
                //write Request
                Trace.WriteLine(JsonSerializer.Serialize(statusEvent.Request as object, new JsonSerializerOptions { WriteIndented = true }));
            }
            else if( statusEvent.Status == CoreServiceStatus.Finished)
            {
                //write Response
                Trace.WriteLine(JsonSerializer.Serialize(statusEvent.Response as object, new JsonSerializerOptions { WriteIndented = true }));
            }

            return Task.CompletedTask;

        }

    }
}