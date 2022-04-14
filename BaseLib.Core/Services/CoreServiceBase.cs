using FluentValidation;
using System;
using System.Linq;
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

        public CoreServiceBase(IValidator<TRequest> validator)
        {
            Validator = validator;
        }

        public async Task<TResponse> RunAsync(TRequest request)
        {
            try
            {
                this.request = request;

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

                return this.response;
            }
            catch (System.Exception ex)
            {
                return new TResponse
                {
                    Succeeded = false,
                    ReasonCode = CoreServiceReasonCode.ExceptionHappened,
                    Messages = new string[] { $"Excepcion of type {ex.GetType().Name} with message {ex.Message} Happened" }
                };
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
    }

}

