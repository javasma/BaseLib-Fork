
using System;
using BaseLib.Core.Models;

namespace BaseLib.Core.Services;

public interface ICoreLongRunningServiceManager
{   
    Task HandleParentSuspendedAsync(CoreStatusEvent coreEvent);
    Task HandleParentFinishedAsync(CoreStatusEvent coreEvent);
    Task HandleChildrenFinishedAsync(CoreStatusEvent[] coreEvent);

}
