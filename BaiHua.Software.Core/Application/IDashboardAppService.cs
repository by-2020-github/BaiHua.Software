namespace BaiHua.Software.Core.Application;

public interface IDashboardAppService
{
    DashboardData GetDashboard();
    DashboardData RefreshDashboard();
}
