using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using BaseLib.Core.Services;

namespace Abc.RaffleOnline
{
    public class RaffleServiceJournal : ICoreServiceJournal
    {
        public Task BeginAsync(ICoreServiceState state)
        {
            Trace.WriteLine($"Operation {state.OperationId} started on {state.StartedOn}");
            Trace.WriteLine(JsonSerializer.Serialize(state.Request as object, new JsonSerializerOptions { WriteIndented = true }));

            return Task.CompletedTask;

        }

        public Task EndAsync(ICoreServiceState state)
        {
            Trace.WriteLine($"Operation {state.OperationId} finished on {state.FinishedOn}, Succeded: {state.Response.Succeeded}");
            Trace.WriteLine(JsonSerializer.Serialize(state.Response as object, new JsonSerializerOptions { WriteIndented = true }));
            return Task.CompletedTask;
        }
    }
}