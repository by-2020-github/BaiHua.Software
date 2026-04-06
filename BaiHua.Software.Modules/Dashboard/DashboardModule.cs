namespace BaiHua.Software.Modules.Dashboard;

using BaiHua.Software.Modules.Dashboard.Views;
using BaiHua.Software.Modules.Dashboard.ViewModels;

/// <summary>
/// 仪表板模块 - 显示系统实时状态
/// </summary>
public class DashboardModule : ModuleBase, IModuleLifecycle
{
    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>(nameof(DashboardView));
    }

    public override void OnInitialized(IContainerProvider containerProvider)
    {
        // 模块初始化完成
    }

    public void OnLoad()
    {
        Debug.WriteLine("DashboardModule loaded");
    }

    public void OnUnload()
    {
        Debug.WriteLine("DashboardModule unloaded");
    }

    public ModuleExport GetModuleInfo()
    {
        return new ModuleExport
        {
            Name = nameof(DashboardModule),
            DisplayName = "仪表板",
            IconGlyph = "",
            Description = "查看系统总览、设备在线状态和生产统计。",
            ViewType = typeof(DashboardView),
            Region = "ContentRegion",
            IsVisible = true,
            Order = 0
        };
    }
}
