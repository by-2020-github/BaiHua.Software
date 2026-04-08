using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using BaiHua.Software.Core.Application;

namespace BaiHua.Software.Modules.Workstation.ViewModels;

public class WorkstationViewModel : BindableBase
{
    private readonly IWorkstationAppService _workstationAppService;
    private string _title = string.Empty;
    private string _currentRecipe = string.Empty;
    private string _currentStatus = string.Empty;
    private bool _isAutoMode;
    private double _axisPosition;
    private double _temperature;
    private double _pressure;
    private string _executionSummary = string.Empty;
    private string _latestLog = string.Empty;
    private string _controlHint = string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string CurrentRecipe
    {
        get => _currentRecipe;
        set => SetProperty(ref _currentRecipe, value);
    }

    public string CurrentStatus
    {
        get => _currentStatus;
        set => SetProperty(ref _currentStatus, value);
    }

    public bool IsAutoMode
    {
        get => _isAutoMode;
        set
        {
            if (SetProperty(ref _isAutoMode, value))
            {
                RaisePropertyChanged(nameof(OperationMode));
            }
        }
    }

    public string OperationMode => IsAutoMode ? "自动" : "手动";

    public string ExecutionSummary
    {
        get => _executionSummary;
        set => SetProperty(ref _executionSummary, value);
    }

    public string LatestLog
    {
        get => _latestLog;
        set => SetProperty(ref _latestLog, value);
    }

    public string ControlHint
    {
        get => _controlHint;
        set => SetProperty(ref _controlHint, value);
    }

    public double AxisPosition
    {
        get => _axisPosition;
        set => SetProperty(ref _axisPosition, value);
    }

    public double Temperature
    {
        get => _temperature;
        set => SetProperty(ref _temperature, value);
    }

    public double Pressure
    {
        get => _pressure;
        set => SetProperty(ref _pressure, value);
    }

    public ObservableCollection<string> OperationLogs { get; } = [];

    public DelegateCommand StartCommand { get; }
    public DelegateCommand StopCommand { get; }
    public DelegateCommand ResetCommand { get; }
    public DelegateCommand ToggleModeCommand { get; }

    public WorkstationViewModel(IWorkstationAppService workstationAppService)
    {
        _workstationAppService = workstationAppService;
        StartCommand = new DelegateCommand(OnStart, CanStart);
        StopCommand = new DelegateCommand(OnStop, CanStop);
        ResetCommand = new DelegateCommand(OnReset);
        ToggleModeCommand = new DelegateCommand(OnToggleMode);
        Apply(_workstationAppService.GetWorkstation());
    }

    private bool CanStart() => CurrentStatus != "运行中";

    private bool CanStop() => CurrentStatus == "运行中" || CurrentStatus == "待机";

    private void OnStart()
    {
        Apply(_workstationAppService.Start());
    }

    private void OnStop()
    {
        Apply(_workstationAppService.Stop());
    }

    private void OnReset()
    {
        Apply(_workstationAppService.Reset());
    }

    private void OnToggleMode()
    {
        Apply(_workstationAppService.ToggleMode());
    }

    private void Apply(WorkstationData data)
    {
        Title = data.Title;
        CurrentRecipe = data.CurrentRecipe;
        CurrentStatus = data.CurrentStatus;
        IsAutoMode = data.IsAutoMode;
        AxisPosition = data.AxisPosition;
        Temperature = data.Temperature;
        Pressure = data.Pressure;

        OperationLogs.Clear();
        foreach (var log in data.OperationLogs)
        {
            OperationLogs.Add(log);
        }

        ExecutionSummary = $"{CurrentStatus} · {OperationMode} · 轴位 {AxisPosition:F2} mm · 温度 {Temperature:F1} ℃ · 压力 {Pressure:F2} MPa";
        LatestLog = OperationLogs.FirstOrDefault() ?? "暂无操作日志。";
        ControlHint = CurrentStatus == "运行中"
            ? "设备处于运行中，可执行停止、模式切换或监控操作。"
            : "设备当前未运行，可启动或复位后重新进入生产流程。";

        StartCommand.RaiseCanExecuteChanged();
        StopCommand.RaiseCanExecuteChanged();
        RaisePropertyChanged(nameof(OperationMode));
    }
}
