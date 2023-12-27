using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public class CoreServiceState 
    {
        private readonly IDictionary<string, object> environment;

        public CoreServiceState(IDictionary<string, object>? environment = null)
        {
            this.environment = environment ?? new Dictionary<string, object>(StringComparer.Ordinal);
        }

        public DateTimeOffset StartedOn => this.Get<DateTimeOffset>("StartedOn");
       
        public DateTimeOffset FinishedOn => this.Get<DateTimeOffset>("FinishedOn");

        public string? OperationId => this.Get<string?>("OperationId");

        public string? CorrelationId => this.Get<string?>("CorrelationId");

        public CoreRequestBase? Request => this.Get<CoreRequestBase?>("Request");

        public CoreResponseBase? Response => this.Get<CoreResponseBase?>("Response");

        public virtual T? Get<T>(string key)
        {
            return environment.TryGetValue(key, out var value) ? (T)value : default;
        }
        
        public virtual void Set<T>(string key, T value)
        {
            if (value == null) { return; }
            this.environment[key] = value;
        }
    }

}