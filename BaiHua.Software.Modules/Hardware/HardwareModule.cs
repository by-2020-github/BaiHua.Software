using BaiHua.Software.Modules.Hardware.ViewModels;
using BaiHua.Software.Modules.Hardware.Views;
using Prism.Ioc;

namespace BaiHua.Software.Modules.Hardware;

public class HardwareModule : ModuleBase, IModuleLifecycle
{
    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<HardwareView, HardwareViewModel>(nameof(HardwareView));
    }

    public override void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void OnLoad()
    {
        System.Diagnostics.Debug.WriteLine("HardwareModule loaded");
    }

    public void OnUnload()
    {
        System.Diagnostics.Debug.WriteLine("HardwareModule unloaded");
    }

    public ModuleExport GetModuleInfo()
    {
        return new ModuleExport
        {
            Name = nameof(HardwareModule),
            DisplayName = "硬件",
            CategoryKey = ModuleNavigationCategories.Hardware,
            IconGlyph = "",
            Description = "接入 PLC、相机、运动控制和 IO 等硬件资源。",
            ViewType = typeof(HardwareView),
            Region = "ContentRegion",
            IsVisible = true,
            Order = 3
        };
    }
}
