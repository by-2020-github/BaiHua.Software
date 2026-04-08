using System.Collections.Generic;
using System.Linq;

namespace BaiHua.Software.Modules;

public interface IModuleNavigationRegistry
{
    IReadOnlyList<ModuleNavigationCategory> GetCategories();

    ModuleNavigationCategory GetDefaultCategory();

    ModuleNavigationCategory GetCategoryOrDefault(string? categoryKey);
}

public sealed class ModuleNavigationRegistry : IModuleNavigationRegistry
{
    private static readonly IReadOnlyList<ModuleNavigationCategory> Categories =
    [
        new(ModuleNavigationCategories.Operation, "Op", "作业运行模块", "", 0),
        new(ModuleNavigationCategories.Configuration, "Config", "系统配置模块", "", 1),
        new(ModuleNavigationCategories.Hardware, "Hardware", "硬件接入模块", "", 2),
        new(ModuleNavigationCategories.Data, "Data", "数据分析模块", "", 3),
        new(ModuleNavigationCategories.User, "User", "用户权限模块", "", 4)
    ];

    public IReadOnlyList<ModuleNavigationCategory> GetCategories()
    {
        return Categories;
    }

    public ModuleNavigationCategory GetDefaultCategory()
    {
        return Categories.First(category => category.Key == ModuleNavigationCategories.Data);
    }

    public ModuleNavigationCategory GetCategoryOrDefault(string? categoryKey)
    {
        if (string.IsNullOrWhiteSpace(categoryKey))
        {
            return GetDefaultCategory();
        }

        return Categories.FirstOrDefault(category => string.Equals(category.Key, categoryKey, StringComparison.OrdinalIgnoreCase))
            ?? GetDefaultCategory();
    }
}

public sealed record ModuleNavigationCategory(
    string Key,
    string Title,
    string Description,
    string IconGlyph,
    int Order);
