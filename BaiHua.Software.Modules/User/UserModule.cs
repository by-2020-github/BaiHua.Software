using BaiHua.Software.Modules.User.ViewModels;
using BaiHua.Software.Modules.User.Views;
using Prism.Ioc;

namespace BaiHua.Software.Modules.User;

public class UserModule : ModuleBase, IModuleLifecycle
{
    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<UserView, UserViewModel>(nameof(UserView));
    }

    public override void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void OnLoad()
    {
        System.Diagnostics.Debug.WriteLine("UserModule loaded");
    }

    public void OnUnload()
    {
        System.Diagnostics.Debug.WriteLine("UserModule unloaded");
    }

    public ModuleExport GetModuleInfo()
    {
        return new ModuleExport
        {
            Name = nameof(UserModule),
            DisplayName = "用户",
            CategoryKey = ModuleNavigationCategories.User,
            IconGlyph = "",
            Description = "提供登录、权限、角色和审计相关入口。",
            ViewType = typeof(UserView),
            Region = "ContentRegion",
            IsVisible = true,
            Order = 4
        };
    }
}
