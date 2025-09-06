using System.Reflection;
using BaseLib.Core.Models;
using FluentValidation;

namespace BaseLib.Core.Services
{
    public abstract partial class CoreLongRunningServiceBase<TRequest, TResponse> : CoreServiceBase<TRequest, TResponse>, ICoreLongRunningService<TResponse>
        where TRequest : CoreRequestBase
        where TResponse : CoreResponseBase, new()
    {
        private readonly ICoreServiceFireOnly fireOnly;
        private readonly ICoreServiceStateStore stateStore;
        private int childrenCount = 0;

        public CoreLongRunningServiceBase(ICoreServiceFireOnly invoker, ICoreServiceStateStore stateStore, IValidator<TRequest>? validator = null, ICoreStatusEventSink? eventSink = null)
            : base(validator, eventSink)
        {
            this.fireOnly = invoker;
            this.stateStore = stateStore;
        }

        /// <summary>
        /// El estado del servicio es suspended, el finalize guarda el estado del servicio.
        /// </summary>
        protected override async Task FinalizeAsync()
        {
            if (this.childrenCount > 0)
            {
                //el servicio es de larga duración y tiene tareas asincrónicas, se suspende.
                this.Status = CoreServiceStatus.Suspended;

                //aquí agregamos el reasoncode de suspended al codigo existente
                this.Response!.ReasonCode = ((CoreServiceReasonCode)this.Response.ReasonCode.Value) | CoreServiceReasonCode.Suspended;

                //hay tareas asincrónicas, debe guardar el estado serializado.
                var state = this.GetState();
                await this.stateStore.WriteAsync(this.OperationId!, state);
            }

            // Reporta el evento de suspendido al sink de eventos
            await this.OnWriteStatusEventAsync();
        }

        protected override CoreStatusEvent GetStatusEvent()
        {
            var statusEvent = base.GetStatusEvent();
            statusEvent.IsLongRunningService = this.childrenCount > 0;
            statusEvent.ChildrenCount = this.childrenCount;
            return statusEvent;
        }

        async Task<CoreResponseBase> ICoreLongRunningService.ResumeAsync(string operationId)
        {
            var response = await this.ResumeAsync(operationId);
            return response;
        }

        /// <summary>
        /// Reanuda el proceso de larga duración. Invocado por el worker cuando todos los procesos secundarios han finalizado.
        /// </summary>
        public virtual async Task<TResponse> ResumeAsync(string operationId)
        {
            if (string.IsNullOrEmpty(operationId))
                throw new ArgumentNullException(nameof(operationId));
            try
            {
                var state = await this.stateStore.ReadAsync(operationId);
                this.SetState(state);

                this.Response = await this.ResumeAsync();

                //always set the OperationId
                this.Response.OperationId = this.OperationId;

                // If the ReasonCode is still Undefined, set it based on Succeeded
                if (this.Response.ReasonCode == CoreServiceReasonCode.Undefined)
                {
                    this.Response.ReasonCode = this.Response.Succeeded ? CoreServiceReasonCode.Succeeded : CoreServiceReasonCode.Failed;
                }
            }
            catch (Exception ex)
            {
                this.Response = new TResponse
                {
                    Succeeded = false,
                    ReasonCode = CoreServiceReasonCode.ExceptionHappened,
                    Messages = [
                        $"Exception of type {ex.GetType().Name} on {this.GetType().Name} with message {ex.Message} Happened",
                        ex.StackTrace ?? "No StackTrace in exception"
                    ]
                };

            }
            finally
            {
                this.Status = CoreServiceStatus.Finished;

                this.FinishedOn = DateTimeOffset.UtcNow;

                await this.OnWriteStatusEventAsync();
            }

            return this.Response;

        }

        protected abstract Task<TResponse> ResumeAsync();

        public virtual Task FireAsync<TService>(CoreRequestBase request, string? correlationId = null)
            where TService : ICoreServiceBase
        {
            this.childrenCount++;
            return this.fireOnly.FireAsync<TService>(request, correlationId ?? this.OperationId, isLongRunningChild: true);
        }

        protected IDictionary<string, object?> GetState()
        {
            var dict = new Dictionary<string, object?>();
            var type = this.GetType();

            // Traverse type hierarchy to include inherited private fields
            while (type != null && type != typeof(object))
            {
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    // Exclude readonly fields and backing fields (fields with names like <PropertyName>k__BackingField)
                    if (field.IsInitOnly || field.Name.Contains("k__BackingField"))
                        continue;

                    dict[field.Name] = field.GetValue(this);
                }

                type = type.BaseType;
            }

            return dict;
        }

        protected void SetState(IDictionary<string, object?> state)
        {
            var type = this.GetType();

            // Traverse type hierarchy to include inherited private fields
            while (type != null && type != typeof(object))
            {
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    // Exclude readonly fields and backing fields
                    if (field.IsInitOnly || field.Name.Contains("k__BackingField"))
                        continue;

                    if (state.TryGetValue(field.Name, out var value))
                    {
                        field.SetValue(this, value);
                    }
                }

                type = type.BaseType;
            }
        }
    }
}