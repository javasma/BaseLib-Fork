using Abc.Financial.Services.Shared;
using FluentValidation;

namespace Abc.Financial
{
    public class FinancialRequestValidator<TRequest> : AbstractValidator<TRequest>
        where TRequest : FinancialRequest
    {
        private readonly ITenantChecker tenanChecker;

        public FinancialRequestValidator(ITenantChecker tenanChecker)
        {
            this.tenanChecker = tenanChecker;
            RuleFor(r => r.TenantId).NotEmpty();
            RuleFor(r => r.TenantId).MustAsync((tenantId, ctx) => this.tenanChecker.CheckAsync(tenantId));
        }
        
    }
}

