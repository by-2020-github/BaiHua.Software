using Prism.Commands;
using Prism.Mvvm;
using BaiHua.Software.Core.Application;

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
    }
}
