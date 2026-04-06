namespace BaiHua.Software.Core.Application;

public sealed class SettingsData
{
    public string Title { get; set; } = "系统设置";
    public string SystemName { get; set; } = string.Empty;
    public string DeviceCode { get; set; } = string.Empty;
    public string SelectedLanguage { get; set; } = string.Empty;
    public string SelectedTheme { get; set; } = string.Empty;
    public bool AutoStart { get; set; }
    public bool EnableOperationLog { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
}
