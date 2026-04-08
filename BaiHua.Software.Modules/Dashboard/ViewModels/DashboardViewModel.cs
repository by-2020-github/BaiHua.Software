using Prism.Commands;
using Prism.Mvvm;
using BaiHua.Software.Core.Application;
using System.Collections.ObjectModel;

namespace BaiHua.Software.Modules.Dashboard.ViewModels;

public class DashboardViewModel : BindableBase
{
    private readonly IDashboardAppService _dashboardAppService;
    private string _title = string.Empty;
    private string _summary = string.Empty;
    private int _onlineDeviceCount;
    private int _offlineDeviceCount;
    private int _todayOutput;
    private double _yieldRate;
    private DateTime _lastRefreshTime;
    private string _deviceSummary = string.Empty;
    private string _productionSummary = string.Empty;
    private string _systemHealthStatus = string.Empty;
    private string _refreshSummary = string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Summary
    {
        get => _summary;
        set => SetProperty(ref _summary, value);
    }

    public int OnlineDeviceCount
    {
        get => _onlineDeviceCount;
        set => SetProperty(ref _onlineDeviceCount, value);
    }

    public int OfflineDeviceCount
    {
        get => _offlineDeviceCount;
        set => SetProperty(ref _offlineDeviceCount, value);
    }

    public int TodayOutput
    {
        get => _todayOutput;
        set => SetProperty(ref _todayOutput, value);
    }

    public double YieldRate
    {
        get => _yieldRate;
        set => SetProperty(ref _yieldRate, value);
    }

    public DateTime LastRefreshTime
    {
        get => _lastRefreshTime;
        set => SetProperty(ref _lastRefreshTime, value);
    }

    public string DeviceSummary
    {
        get => _deviceSummary;
        set => SetProperty(ref _deviceSummary, value);
    }

    public string ProductionSummary
    {
        get => _productionSummary;
        set => SetProperty(ref _productionSummary, value);
    }

    public string SystemHealthStatus
    {
        get => _systemHealthStatus;
        set => SetProperty(ref _systemHealthStatus, value);
    }

    public string RefreshSummary
    {
        get => _refreshSummary;
        set => SetProperty(ref _refreshSummary, value);
    }

    public int TotalDeviceCount => OnlineDeviceCount + OfflineDeviceCount;

    public string OnlineRateText => TotalDeviceCount == 0
        ? "0%"
        : $"{OnlineDeviceCount * 100.0 / TotalDeviceCount:F1}%";

    public ObservableCollection<string> InsightItems { get; } = [];

    public DelegateCommand RefreshCommand { get; }

    public DashboardViewModel(IDashboardAppService dashboardAppService)
    {
        _dashboardAppService = dashboardAppService;
        RefreshCommand = new DelegateCommand(OnRefresh);
        Apply(_dashboardAppService.GetDashboard());
    }

    private void OnRefresh()
    {
        Apply(_dashboardAppService.RefreshDashboard());
    }

    private void Apply(DashboardData data)
    {
        Title = data.Title;
        Summary = data.Summary;
        OnlineDeviceCount = data.OnlineDeviceCount;
        OfflineDeviceCount = data.OfflineDeviceCount;
        TodayOutput = data.TodayOutput;
        YieldRate = data.YieldRate;
        LastRefreshTime = data.LastRefreshTime;
        UpdateDerivedState();
    }

    private void UpdateDerivedState()
    {
        DeviceSummary = $"设备总数 {TotalDeviceCount} 台，在线率 {OnlineRateText}";
        ProductionSummary = $"今日累计产量 {TodayOutput}，当前良率 {YieldRate:F1}%";
        SystemHealthStatus = OfflineDeviceCount == 0 ? "系统运行稳定" : $"存在 {OfflineDeviceCount} 台离线设备待处理";
        RefreshSummary = $"最近刷新时间：{LastRefreshTime:yyyy-MM-dd HH:mm:ss}";

        InsightItems.Clear();
        InsightItems.Add($"在线设备 {OnlineDeviceCount} 台，建议优先巡检离线设备。");
        InsightItems.Add($"当前良率 {YieldRate:F1}% ，可作为工艺参数优化参考。");
        InsightItems.Add($"今日产量 {TodayOutput} 件，适合用于班次和日报统计。");

        RaisePropertyChanged(nameof(TotalDeviceCount));
        RaisePropertyChanged(nameof(OnlineRateText));
    }
}
