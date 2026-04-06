using BaiHua.Software.Core.Application;

namespace BaiHua.Software.Services.Application;

public sealed class SettingsStateService : ISettingsStateService
{
    private readonly object _syncRoot = new();

    private SettingsData _settings = CreateDefault();

    public SettingsData GetSettings()
    {
        lock (_syncRoot)
        {
            return Clone(_settings);
        }
    }

    public SettingsData Save(SettingsData settings)
    {
        lock (_syncRoot)
        {
            _settings = Clone(settings);
            _settings.StatusMessage = $"设置已保存：{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            return Clone(_settings);
        }
    }

    public SettingsData RestoreDefaults()
    {
        lock (_syncRoot)
        {
            _settings = CreateDefault();
            _settings.StatusMessage = "已恢复默认设置。";
            return Clone(_settings);
        }
    }

    private static SettingsData CreateDefault()
    {
        return new SettingsData
        {
            Title = "系统设置",
            SystemName = "BaiHua.Software",
            DeviceCode = "BH-CTRL-001",
            SelectedLanguage = "中文",
            SelectedTheme = "浅色",
            AutoStart = true,
            EnableOperationLog = true,
            StatusMessage = "尚未保存变更。"
        };
    }

    private static SettingsData Clone(SettingsData source)
    {
        return new SettingsData
        {
            Title = source.Title,
            SystemName = source.SystemName,
            DeviceCode = source.DeviceCode,
            SelectedLanguage = source.SelectedLanguage,
            SelectedTheme = source.SelectedTheme,
            AutoStart = source.AutoStart,
            EnableOperationLog = source.EnableOperationLog,
            StatusMessage = source.StatusMessage
        };
    }
}
