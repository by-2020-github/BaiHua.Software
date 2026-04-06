using System.Collections.Generic;
using System.Linq;

namespace BaiHua.Software.Modules;

/// <summary>
/// 模块管理器 - 管理所有动态加载的模块
/// </summary>
public class ModuleManager : IModuleManager
{
    private readonly Dictionary<string, ModuleExport> _modules = new();
    private readonly Dictionary<string, IModuleLifecycle> _lifecycleModules = new();

    public ModuleManager()
    {
        DiscoverModules();
    }

    private void DiscoverModules()
    {
        var moduleTypes = typeof(ModuleManager).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && typeof(IModuleLifecycle).IsAssignableFrom(t));

        foreach (var moduleType in moduleTypes)
        {
            if (Activator.CreateInstance(moduleType) is not IModuleLifecycle module)
            {
                continue;
            }

            var moduleInfo = module.GetModuleInfo();
            _modules[moduleInfo.Name] = moduleInfo;
            _lifecycleModules[moduleInfo.Name] = module;
        }
    }

    /// <summary>
    /// 加载模块
    /// </summary>
    public void LoadModule(string moduleName)
    {
        if (string.IsNullOrEmpty(moduleName))
            throw new ArgumentNullException(nameof(moduleName));

        if (!_lifecycleModules.TryGetValue(moduleName, out var module))
            return;

        try
        {
            module.OnLoad();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load module '{moduleName}'", ex);
        }
    }

    /// <summary>
    /// 卸载模块
    /// </summary>
    public void UnloadModule(string moduleName)
    {
        if (string.IsNullOrEmpty(moduleName))
            throw new ArgumentNullException(nameof(moduleName));

        if (!_lifecycleModules.TryGetValue(moduleName, out var module))
            return;

        try
        {
            module.OnUnload();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to unload module '{moduleName}'", ex);
        }
    }

    /// <summary>
    /// 获取所有模块信息
    /// </summary>
    public IEnumerable<ModuleExport> GetAllModules()
    {
        return _modules.Values.OrderBy(m => m.Order);
    }

    /// <summary>
    /// 获取指定模块
    /// </summary>
    public ModuleExport? GetModule(string moduleName)
    {
        return _modules.TryGetValue(moduleName, out var module) ? module : null;
    }

    /// <summary>
    /// 注册模块
    /// </summary>
    public void RegisterModule(ModuleExport moduleInfo)
    {
        if (moduleInfo == null)
            throw new ArgumentNullException(nameof(moduleInfo));

        _modules[moduleInfo.Name] = moduleInfo;
    }
}
