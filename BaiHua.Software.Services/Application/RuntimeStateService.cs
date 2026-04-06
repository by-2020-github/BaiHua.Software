using BaiHua.Software.Core.Application;

namespace BaiHua.Software.Services.Application;

public sealed class RuntimeStateService : IRuntimeStateService
{
    private readonly object _syncRoot = new();

    private DashboardData _dashboard = new()
    {
        Title = "系统总览",
        Summary = "聚合当前设备状态、产量和良率数据。",
        OnlineDeviceCount = 12,
        OfflineDeviceCount = 1,
        TodayOutput = 1860,
        YieldRate = 98.6,
        LastRefreshTime = DateTime.Now
    };

    private WorkstationData _workstation = new()
    {
        Title = "工站控制",
        CurrentRecipe = "默认配方-A01",
        CurrentStatus = "待机",
        IsAutoMode = true,
        AxisPosition = 125.50,
        Temperature = 36.8,
        Pressure = 0.42,
        OperationLogs = new List<string>
        {
            $"[{DateTime.Now:HH:mm:ss}] 工站已初始化，等待操作。"
        }
    };

    public DashboardData GetDashboardData()
    {
        lock (_syncRoot)
        {
            return CloneDashboard(_dashboard);
        }
    }

    public DashboardData RefreshDashboard()
    {
        lock (_syncRoot)
        {
            _dashboard.LastRefreshTime = DateTime.Now;
            _dashboard.TodayOutput += 12;
            _dashboard.YieldRate = Math.Min(99.9, _dashboard.YieldRate + 0.1);
            _dashboard.Summary = $"已刷新，当前在线 {_dashboard.OnlineDeviceCount} 台，离线 {_dashboard.OfflineDeviceCount} 台。";
            return CloneDashboard(_dashboard);
        }
    }

    public WorkstationData GetWorkstationData()
    {
        lock (_syncRoot)
        {
            return CloneWorkstation(_workstation);
        }
    }

    public WorkstationData StartWorkstation()
    {
        lock (_syncRoot)
        {
            _workstation.CurrentStatus = "运行中";
            _workstation.AxisPosition += 8.5;
            _workstation.Temperature += 0.6;
            _workstation.Pressure += 0.03;
            AppendLog("执行启动命令。");
            _dashboard.TodayOutput += 24;
            _dashboard.LastRefreshTime = DateTime.Now;
            return CloneWorkstation(_workstation);
        }
    }

    public WorkstationData StopWorkstation()
    {
        lock (_syncRoot)
        {
            _workstation.CurrentStatus = "已停止";
            AppendLog("执行停止命令。");
            _dashboard.LastRefreshTime = DateTime.Now;
            return CloneWorkstation(_workstation);
        }
    }

    public WorkstationData ResetWorkstation()
    {
        lock (_syncRoot)
        {
            _workstation.CurrentStatus = "待机";
            _workstation.AxisPosition = 0;
            _workstation.Pressure = 0.40;
            _workstation.Temperature = 30.0;
            AppendLog("工站已复位。");
            _dashboard.LastRefreshTime = DateTime.Now;
            return CloneWorkstation(_workstation);
        }
    }

    public WorkstationData ToggleWorkstationMode()
    {
        lock (_syncRoot)
        {
            _workstation.IsAutoMode = !_workstation.IsAutoMode;
            AppendLog($"切换为{(_workstation.IsAutoMode ? "自动" : "手动")}模式。");
            _dashboard.LastRefreshTime = DateTime.Now;
            return CloneWorkstation(_workstation);
        }
    }

    private void AppendLog(string message)
    {
        var logs = _workstation.OperationLogs.ToList();
        logs.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");
        _workstation.OperationLogs = logs;
    }

    private static DashboardData CloneDashboard(DashboardData source)
    {
        return new DashboardData
        {
            Title = source.Title,
            Summary = source.Summary,
            OnlineDeviceCount = source.OnlineDeviceCount,
            OfflineDeviceCount = source.OfflineDeviceCount,
            TodayOutput = source.TodayOutput,
            YieldRate = source.YieldRate,
            LastRefreshTime = source.LastRefreshTime
        };
    }

    private static WorkstationData CloneWorkstation(WorkstationData source)
    {
        return new WorkstationData
        {
            Title = source.Title,
            CurrentRecipe = source.CurrentRecipe,
            CurrentStatus = source.CurrentStatus,
            IsAutoMode = source.IsAutoMode,
            AxisPosition = source.AxisPosition,
            Temperature = source.Temperature,
            Pressure = source.Pressure,
            OperationLogs = source.OperationLogs.ToList()
        };
    }
}
