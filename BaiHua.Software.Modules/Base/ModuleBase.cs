using Prism.Ioc;
using Prism.Modularity;

namespace BaiHua.Software.Modules;

public static class ModuleNavigationCategories
{
    public const string Operation = "Op";
    public const string Configuration = "Config";
    public const string Hardware = "Hardware";
    public const string Data = "Data";
    public const string User = "User";
}

/// <summary>
/// 模块基类 - 所有功能模块的基础
/// </summary>
public abstract class ModuleBase : IModule
{
    /// <summary>
    /// 模块初始化时注册类型
    /// </summary>
    public virtual void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // 由派生类实现具体的类型注册
    }

    /// <summary>
    /// 模块初始化完成
    /// </summary>
    public virtual void OnInitialized(IContainerProvider containerProvider)
    {
        // 由派生类实现初始化逻辑
    }
}

/// <summary>
/// 模块导出 - 包含视图和视图模型
/// </summary>
public class ModuleExport
{
    /// <summary>
    /// 模块名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 模块显示名称
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 导航分类键
    /// </summary>
    public string CategoryKey { get; set; } = ModuleNavigationCategories.Data;

    /// <summary>
    /// 导航图标（Segoe MDL2 Assets）
    /// </summary>
    public string IconGlyph { get; set; } = "\uE80F";

    /// <summary>
    /// 模块说明
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 主视图类型
    /// </summary>
    public Type? ViewType { get; set; }

    /// <summary>
    /// 模块对应的区域（用于动态加载）
    /// </summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// 模块顺序（用于排序展示）
    /// </summary>
    public int Order { get; set; } = 0;
}

/// <summary>
/// 模块生命周期接口 - 提供模块初始化、加载、卸载等事件
/// </summary>
public interface IModuleLifecycle
{
    /// <summary>
    /// 模块加载时调用
    /// </summary>
    void OnLoad();

    /// <summary>
    /// 模块卸载时调用
    /// </summary>
    void OnUnload();

    /// <summary>
    /// 获取模块信息
    /// </summary>
    ModuleExport GetModuleInfo();
}

/// <summary>
/// 模块管理器接口
/// </summary>
public interface IModuleManager
{
    /// <summary>
    /// 加载模块
    /// </summary>
    void LoadModule(string moduleName);

    /// <summary>
    /// 卸载模块
    /// </summary>
    void UnloadModule(string moduleName);

    /// <summary>
    /// 获取所有模块信息
    /// </summary>
    IEnumerable<ModuleExport> GetAllModules();

    /// <summary>
    /// 获取指定模块
    /// </summary>
    ModuleExport? GetModule(string moduleName);
}
