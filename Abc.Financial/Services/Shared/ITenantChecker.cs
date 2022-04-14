using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abc.Financial.Services.Shared
{
    public interface ITenantChecker
    {
        Task<bool> CheckAsync(string tenantId);
    }
}
