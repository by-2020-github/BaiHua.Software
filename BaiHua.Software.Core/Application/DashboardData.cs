namespace BaiHua.Software.Core.Application;

public sealed class DashboardData
{
    public string Title { get; set; } = "系统总览";
    public string Summary { get; set; } = string.Empty;
    public int OnlineDeviceCount { get; set; }
    public int OfflineDeviceCount { get; set; }
    public int TodayOutput { get; set; }
    public double YieldRate { get; set; }
    public DateTime LastRefreshTime { get; set; }
}
