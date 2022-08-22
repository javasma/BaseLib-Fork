using System;
using System.Threading.Tasks;

namespace Abc.Financial.Services.SuspendOrder
{
    internal class SuspendOrderService : FinancialServiceBase<FinancialRequest, FinancialResponse>
    {
        public SuspendOrderService(FinancialRequestValidator<FinancialRequest> validator, FinancialServiceJournal journal)
            : base(validator, journal)
        {

        }

        protected override Task<FinancialResponse> RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
