using BaseLib.Core.Services;
using FluentValidation;
using System.Threading;

namespace Abc.Financial
{
    public abstract class FinancialServiceBase<TRequest, TResponse> : CoreServiceBase<TRequest, TResponse>
        where TRequest : FinancialRequest
        where TResponse : FinancialResponse, new()
    {
        public FinancialServiceBase(FinancialRequestValidator<TRequest> validator = null, FinancialServiceJournal journal = null)
            : base(validator, journal as ICoreServiceJournal<TRequest, TResponse>)
        {

        }
    }
}

