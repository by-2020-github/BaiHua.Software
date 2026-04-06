using BaiHua.Software.Core.Application;

namespace BaiHua.Software.BLL.Application;

public sealed class DashboardAppService : IDashboardAppService
{
    private readonly IRuntimeStateService _runtimeStateService;

    public DashboardAppService(IRuntimeStateService runtimeStateService)
    {
        _runtimeStateService = runtimeStateService;
    }

    public DashboardData GetDashboard()
    {
        return _runtimeStateService.GetDashboardData();
    }

    public DashboardData RefreshDashboard()
    {
        return _runtimeStateService.RefreshDashboard();
    }
}
