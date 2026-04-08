using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using BaiHua.Software.Core.Application;

namespace BaiHua.Software.Modules.Settings.ViewModels;

public class SettingsViewModel : BindableBase
{
    private readonly ISettingsAppService _settingsAppService;
    private string _title = string.Empty;
    private string _systemName = string.Empty;
    private string _deviceCode = string.Empty;
    private string _selectedLanguage = string.Empty;
    private string _selectedTheme = string.Empty;
    private bool _autoStart;
    private bool _enableOperationLog;
    private string _statusMessage = string.Empty;
    private string _runtimeSummary = string.Empty;
    private string _interfaceSummary = string.Empty;
    private string _persistenceSummary = string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string SystemName
    {
        get => _systemName;
        set
        {
            if (SetProperty(ref _systemName, value))
            {
                UpdateDerivedState();
            }
        }
    }

    public string DeviceCode
    {
        get => _deviceCode;
        set
        {
            if (SetProperty(ref _deviceCode, value))
            {
                UpdateDerivedState();
            }
        }
    }

    public ObservableCollection<string> Languages { get; } = ["中文", "English"];

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (SetProperty(ref _selectedLanguage, value))
            {
                UpdateDerivedState();
            }
        }
    }

    public ObservableCollection<string> Themes { get; } = ["浅色", "深色"];

    public string SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            if (SetProperty(ref _selectedTheme, value))
            {
                UpdateDerivedState();
            }
        }
    }

    public bool AutoStart
    {
        get => _autoStart;
        set
        {
            if (SetProperty(ref _autoStart, value))
            {
                UpdateDerivedState();
            }
        }
    }

    public bool EnableOperationLog
    {
        get => _enableOperationLog;
        set
        {
            if (SetProperty(ref _enableOperationLog, value))
            {
                UpdateDerivedState();
            }
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            if (SetProperty(ref _statusMessage, value))
            {
                UpdateDerivedState();
            }
        }
    }

    public string RuntimeSummary
    {
        get => _runtimeSummary;
        set => SetProperty(ref _runtimeSummary, value);
    }

    public string InterfaceSummary
    {
        get => _interfaceSummary;
        set => SetProperty(ref _interfaceSummary, value);
    }

    public string PersistenceSummary
    {
        get => _persistenceSummary;
        set => SetProperty(ref _persistenceSummary, value);
    }

    public ObservableCollection<string> PreviewItems { get; } = [];

    public DelegateCommand SaveCommand { get; }
    public DelegateCommand RestoreDefaultsCommand { get; }

    public SettingsViewModel(ISettingsAppService settingsAppService)
    {
        _settingsAppService = settingsAppService;
        SaveCommand = new DelegateCommand(OnSave);
        RestoreDefaultsCommand = new DelegateCommand(OnRestoreDefaults);
        Apply(_settingsAppService.GetSettings());
    }

    private void OnSave()
    {
        Apply(_settingsAppService.Save(new SettingsData
        {
            Title = Title,
            SystemName = SystemName,
            DeviceCode = DeviceCode,
            SelectedLanguage = SelectedLanguage,
            SelectedTheme = SelectedTheme,
            AutoStart = AutoStart,
            EnableOperationLog = EnableOperationLog,
            StatusMessage = StatusMessage
        }));
    }

    private void OnRestoreDefaults()
    {
        Apply(_settingsAppService.RestoreDefaults());
    }

    private void Apply(SettingsData data)
    {
        Title = data.Title;
        SystemName = data.SystemName;
        DeviceCode = data.DeviceCode;
        SelectedLanguage = data.SelectedLanguage;
        SelectedTheme = data.SelectedTheme;
        AutoStart = data.AutoStart;
        EnableOperationLog = data.EnableOperationLog;
        StatusMessage = data.StatusMessage;
        UpdateDerivedState();
    }

    private void UpdateDerivedState()
    {
        RuntimeSummary = $"系统 {SystemName} / 设备 {DeviceCode}";
        InterfaceSummary = $"语言 {SelectedLanguage} · 主题 {SelectedTheme}";
        PersistenceSummary = $"自动启动：{(AutoStart ? "开启" : "关闭")} · 操作日志：{(EnableOperationLog ? "开启" : "关闭")}";

        PreviewItems.Clear();
        PreviewItems.Add($"系统名称：{SystemName}");
        PreviewItems.Add($"设备编号：{DeviceCode}");
        PreviewItems.Add($"界面设置：{SelectedLanguage} / {SelectedTheme}");
        PreviewItems.Add($"运行行为：{(AutoStart ? "开机自启" : "手动启动")}，{(EnableOperationLog ? "记录操作日志" : "关闭操作日志")}");
    }
}
