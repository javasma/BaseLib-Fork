using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaseLib.Core.Services
{
    public abstract class CoreServiceBase<TRequest, TResponse> : ICoreServiceBase<TRequest, TResponse>
         where TRequest : ICoreServiceRequest
         where TResponse : ICoreServiceResponse, new()
    {
        private TRequest request;
        private TResponse response;

        protected TRequest Request { get { return this.request; } }

        protected TResponse Response { get { return this.response; } set { this.response = value; } }

        protected IValidator<TRequest> Validator { get; }
        private ICoreServiceJournal<TRequest, TResponse> Journal { get; }

        public CoreServiceBase(IValidator<TRequest> validator = null, ICoreServiceJournal<TRequest, TResponse> journal = null)
        {
            Validator = validator;
            Journal = journal ?? new NullCoreServiceJournal();
        }

        public async Task<TResponse> RunAsync(TRequest request)
        {
            this.request = request;

            using (var context = new CoreServiceJournalContext(this))
            {
                try
                {
                    if (this.Validator != null)
                    {
                        var validationResult = await this.Validator.ValidateAsync(this.Request);
                        if (!validationResult.IsValid)
                        {
                            this.response = new TResponse
                            {
                                //toma el resultado y lo mapea al response
                                Succeeded = false,
                                ReasonCode = CoreServiceReasonCode.ValidationResultNotValid,
                                Messages = validationResult.Errors.Select(e => e.ErrorMessage).ToArray()
                            };
                            return this.response;
                        }
                    }

                    //No hay validación o la validación fue exitosa
                    this.response = await RunAsync();

                    if (this.response.ReasonCode == null)
                    {
                        this.response.ReasonCode = this.response.Succeeded ? CoreServiceReasonCode.Succeeded : CoreServiceReasonCode.Failed;
                    }
                }
                catch (Exception ex)
                {
                    this.response = new TResponse
                    {
                        Succeeded = false,
                        ReasonCode = CoreServiceReasonCode.ExceptionHappened,
                        Messages = new string[] { $"Excepcion of type {ex.GetType().Name} with message {ex.Message} Happened" }
                    };
                }

                return this.response;
            }
        }

        protected abstract Task<TResponse> RunAsync();

        public TResponse Fail(Enum reasonCode, params string[] messages)
        {
            return new TResponse
            {
                Succeeded = false,
                ReasonCode = reasonCode,
                Messages = messages
            };
        }
        private class CoreServiceJournalContext : IDisposable
        {
            private readonly CoreServiceBase<TRequest, TResponse> service;

            public CoreServiceJournalContext(CoreServiceBase<TRequest, TResponse> service)
            {
                this.service = service;
                service.Journal.BeginAsync(this.service.Request);
            }

            public void Dispose()
            {
                service.Journal.EndAsync(this.service.Response);
            }
        }
        private class NullCoreServiceJournal : ICoreServiceJournal<TRequest, TResponse>
        {
            public Task BeginAsync(TRequest request)
            {
                return Task.FromResult(0);
            }

            public Task EndAsync(TResponse response)
            {
                return Task.FromResult(0);
            }
        }

    }

}

