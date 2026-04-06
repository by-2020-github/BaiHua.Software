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

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string SystemName
    {
        get => _systemName;
        set => SetProperty(ref _systemName, value);
    }

    public string DeviceCode
    {
        get => _deviceCode;
        set => SetProperty(ref _deviceCode, value);
    }

    public ObservableCollection<string> Languages { get; } = ["中文", "English"];

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set => SetProperty(ref _selectedLanguage, value);
    }

    public ObservableCollection<string> Themes { get; } = ["浅色", "深色"];

    public string SelectedTheme
    {
        get => _selectedTheme;
        set => SetProperty(ref _selectedTheme, value);
    }

    public bool AutoStart
    {
        get => _autoStart;
        set => SetProperty(ref _autoStart, value);
    }

    public bool EnableOperationLog
    {
        get => _enableOperationLog;
        set => SetProperty(ref _enableOperationLog, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

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
    }
}
