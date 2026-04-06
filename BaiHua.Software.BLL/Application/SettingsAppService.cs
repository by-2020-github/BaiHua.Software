using BaiHua.Software.Core.Application;

namespace BaiHua.Software.BLL.Application;

public sealed class SettingsAppService : ISettingsAppService
{
    private readonly ISettingsStateService _settingsStateService;

    public SettingsAppService(ISettingsStateService settingsStateService)
    {
        _settingsStateService = settingsStateService;
    }

    public SettingsData GetSettings()
    {
        return _settingsStateService.GetSettings();
    }

    public SettingsData Save(SettingsData settings)
    {
        return _settingsStateService.Save(settings);
    }

    public SettingsData RestoreDefaults()
    {
        return _settingsStateService.RestoreDefaults();
    }
}
