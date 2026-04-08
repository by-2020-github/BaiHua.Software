using Prism.Ioc;
using Prism.Modularity;
using BaiHua.Software.Modules.Workstation.Views;
using BaiHua.Software.Modules.Workstation.ViewModels;

namespace BaiHua.Software.Modules.Workstation;

/// <summary>
/// 工站控制模块 - 工站状态监控和控制
/// </summary>
public class WorkstationModule : ModuleBase, IModuleLifecycle
{
    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<WorkstationView, WorkstationViewModel>(nameof(WorkstationView));
    }

    public override void OnInitialized(IContainerProvider containerProvider)
    {
        // 模块初始化完成
    }

    public void OnLoad()
    {
        System.Diagnostics.Debug.WriteLine("WorkstationModule loaded");
    }

    public void OnUnload()
    {
        System.Diagnostics.Debug.WriteLine("WorkstationModule unloaded");
    }

    public ModuleExport GetModuleInfo()
    {
        return new ModuleExport
        {
            Name = nameof(WorkstationModule),
            DisplayName = "工站控制",
            CategoryKey = ModuleNavigationCategories.Operation,
            IconGlyph = "",
            Description = "监控工站状态，执行启动、停止和复位操作。",
            ViewType = typeof(WorkstationView),
            Region = "ContentRegion",
            IsVisible = true,
            Order = 1
        };
    }
}
