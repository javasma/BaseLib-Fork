using BaseLib.Core.Services;
using System;
using System.Threading.Tasks;

namespace Abc.Financial
{
    public class FinancialServiceJournal : ICoreServiceJournal<FinancialRequest, FinancialResponse>
    {
        public Task BeginAsync(FinancialRequest request)
        {
            throw new NotImplementedException();
        }

        public Task EndAsync(FinancialResponse response)
        {
            throw new NotImplementedException();
        }
    }
}

