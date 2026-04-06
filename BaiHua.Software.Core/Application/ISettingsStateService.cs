namespace BaiHua.Software.Core.Application;

public interface ISettingsStateService
{
    SettingsData GetSettings();
    SettingsData Save(SettingsData settings);
    SettingsData RestoreDefaults();
}
