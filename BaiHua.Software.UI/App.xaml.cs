using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using AppModules = BaiHua.Software.Modules;
using BaiHua.Software.Core.Application;
using BaiHua.Software.Services.Application;
using BaiHua.Software.BLL.Application;
using BaiHua.Software.Modules.Dashboard;
using BaiHua.Software.Modules.Hardware;
using BaiHua.Software.Modules.Settings;
using BaiHua.Software.Modules.User;
using BaiHua.Software.Modules.Workstation;

namespace BaiHua.Software.UI;

/// <summary>
/// 应用程序主类
/// </summary>
public partial class App : PrismApplication
{
    protected override Window CreateShell()
    {
        return ContainerLocator.Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<AppModules.IModuleManager, AppModules.ModuleManager>();
        containerRegistry.RegisterSingleton<AppModules.IModuleNavigationRegistry, AppModules.ModuleNavigationRegistry>();
        containerRegistry.RegisterSingleton<IRuntimeStateService, RuntimeStateService>();
        containerRegistry.RegisterSingleton<ISettingsStateService, SettingsStateService>();
        containerRegistry.Register<IDashboardAppService, DashboardAppService>();
        containerRegistry.Register<IWorkstationAppService, WorkstationAppService>();
        containerRegistry.Register<ISettingsAppService, SettingsAppService>();
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<DashboardModule>();
        moduleCatalog.AddModule<WorkstationModule>();
        moduleCatalog.AddModule<SettingsModule>();
        moduleCatalog.AddModule<HardwareModule>();
        moduleCatalog.AddModule<UserModule>();
    }
}

