namespace BaiHua.Software.Core.Application;

public interface IRuntimeStateService
{
    DashboardData GetDashboardData();
    DashboardData RefreshDashboard();
    WorkstationData GetWorkstationData();
    WorkstationData StartWorkstation();
    WorkstationData StopWorkstation();
    WorkstationData ResetWorkstation();
    WorkstationData ToggleWorkstationMode();
}
