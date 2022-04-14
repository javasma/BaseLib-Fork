using Abc.Financial.Services.Shared;
using BaseLib.Core.Services;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Abc.Financial
{
    public abstract class FinancialServiceBase<TRequest, TResponse> : CoreServiceBase<TRequest, TResponse>
        where TRequest : FinancialRequest
        where TResponse : FinancialResponse, new()
    {
        protected FinancialServiceBase(IValidator<TRequest> validator)
            : base(validator)
        {

        }
    }

    public class FinancialResponse : CoreServiceResponseBase
    {
    }

    public class FinancialRequest : CoreServiceRequestBase
    {
        public string TenantId { get; set; }
    }

    public enum FinancialResponseCode
    {
        NoInstrumentsFound = 1025
    }

    public class FinancialRequestValidator<TRequest> : AbstractValidator<TRequest>
        where TRequest : FinancialRequest
    {
        private readonly ITenantChecker tenanChecker;

        public FinancialRequestValidator(ITenantChecker tenanChecker)
        {
            RuleFor(r => r.TenantId).NotEmpty();
            RuleFor(r => r.TenantId).MustAsync((tenantId, ctx) => tenanChecker.CheckAsync(tenantId));
        }
    }
}

