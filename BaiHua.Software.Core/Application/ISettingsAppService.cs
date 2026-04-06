namespace BaiHua.Software.Core.Application;

public interface ISettingsAppService
{
    SettingsData GetSettings();
    SettingsData Save(SettingsData settings);
    SettingsData RestoreDefaults();
}
