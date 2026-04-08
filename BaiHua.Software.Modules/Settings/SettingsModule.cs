using Prism.Ioc;
using Prism.Modularity;
using BaiHua.Software.Modules.Settings.Views;
using BaiHua.Software.Modules.Settings.ViewModels;

namespace BaiHua.Software.Modules.Settings;

/// <summary>
/// 设置模块 - 系统配置和参数管理
/// </summary>
public class SettingsModule : ModuleBase, IModuleLifecycle
{
    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>(nameof(SettingsView));
    }

    public override void OnInitialized(IContainerProvider containerProvider)
    {
        // 模块初始化完成
    }

    public void OnLoad()
    {
        System.Diagnostics.Debug.WriteLine("SettingsModule loaded");
    }

    public void OnUnload()
    {
        System.Diagnostics.Debug.WriteLine("SettingsModule unloaded");
    }

    public ModuleExport GetModuleInfo()
    {
        return new ModuleExport
        {
            Name = nameof(SettingsModule),
            DisplayName = "设置",
            CategoryKey = ModuleNavigationCategories.Configuration,
            IconGlyph = "",
            Description = "维护系统参数、界面主题和运行选项。",
            ViewType = typeof(SettingsView),
            Region = "ContentRegion",
            IsVisible = true,
            Order = 2
        };
    }
}
