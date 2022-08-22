using BaseLib.Core.Services;

namespace Abc.Financial
{
    public class FinancialRequest : CoreServiceRequestBase
    {
        public string TenantId { get; set; }
    }
}

