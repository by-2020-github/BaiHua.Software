using BaiHua.Software.Core.Application;

namespace BaiHua.Software.BLL.Application;

public sealed class WorkstationAppService : IWorkstationAppService
{
    private readonly IRuntimeStateService _runtimeStateService;

    public WorkstationAppService(IRuntimeStateService runtimeStateService)
    {
        _runtimeStateService = runtimeStateService;
    }

    public WorkstationData GetWorkstation()
    {
        return _runtimeStateService.GetWorkstationData();
    }

    public WorkstationData Start()
    {
        return _runtimeStateService.StartWorkstation();
    }

    public WorkstationData Stop()
    {
        return _runtimeStateService.StopWorkstation();
    }

    public WorkstationData Reset()
    {
        return _runtimeStateService.ResetWorkstation();
    }

    public WorkstationData ToggleMode()
    {
        return _runtimeStateService.ToggleWorkstationMode();
    }
}
