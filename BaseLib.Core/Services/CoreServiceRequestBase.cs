using System;

namespace BaseLib.Core.Services
{
    public abstract class CoreServiceRequestBase : ICoreServiceRequest
    {
        public virtual DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;
    }


}

