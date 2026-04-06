# Modules (06 BaiHua.Software.Modules)

## 概述

**BaiHua.Software.Modules** 是基于 **Prism** 框架实现的模块化系统。它提供了可插拔的、独立的功能模块架构，支持动态加载和卸载。

## 主要特性

- ✅ **模块化设计**：每个功能独立为一个模块
- ✅ **Prism 框架**：基于业界成熟的 Prism 框架
- ✅ **动态加载**：支持模块的动态加载和卸载
- ✅ **解耦通信**：通过事件或服务接口进行模块间通信
- ✅ **生命周期管理**：完整的模块生命周期支持

## 文件结构

```
Modules/
├── Base/                           # 基础类和接口
│   └── ModuleBase.cs              # 模块基类和接口定义
│
├── Management/                     # 模块管理
│   └── ModuleManager.cs           # 模块管理器实现
│
├── Dashboard/                      # 仪表板模块
│   └── DashboardModule.cs         # 仪表板模块定义
│
├── Workstation/                    # 工站控制模块
│   └── WorkstationModule.cs       # 工站模块定义
│
├── Settings/                       # 设置模块
│   └── SettingsModule.cs          # 设置模块定义
│
└── Readme.md
```

## 核心类型

### 1. ModuleBase - 模块基类

所有功能模块继承此基类，实现 IModule 接口。

```csharp
public abstract class ModuleBase : IModule
{
    public virtual void RegisterTypes(IContainerRegistry containerRegistry);
    public virtual void OnInitialized(IContainerProvider containerProvider);
}
```

### 2. IModuleLifecycle - 模块生命周期接口

定义模块的加载、卸载等生命周期方法。

```csharp
public interface IModuleLifecycle
{
    void OnLoad();
    void OnUnload();
    ModuleExport GetModuleInfo();
}
```

### 3. ModuleExport - 模块信息

包含模块的元数据信息。

```csharp
public class ModuleExport
{
    public string Name { get; set; }              // 模块名称
    public string DisplayName { get; set; }       // 显示名称
    public Type? ViewType { get; set; }           // 主视图类型
    public string Region { get; set; }            // 所属区域
    public bool IsVisible { get; set; }           // 是否可见
    public int Order { get; set; }                // 显示顺序
}
```

### 4. IModuleManager - 模块管理器

管理模块的加载、卸载和查询。

```csharp
public interface IModuleManager
{
    void LoadModule(string moduleName);
    void UnloadModule(string moduleName);
    IEnumerable<ModuleExport> GetAllModules();
    ModuleExport? GetModule(string moduleName);
}
```

## 内置模块

### DashboardModule - 仪表板模块
展示系统实时状态、统计数据和关键指标。

### WorkstationModule - 工站控制模块
管理工站状态监控、手动控制和参数配置。

### SettingsModule - 设置模块
系统配置、用户管理和参数设置。

## 使用示例

### 1. 创建自定义模块

```csharp
public class CustomModule : ModuleBase, IModuleLifecycle
{
    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // 注册视图和视图模型
        containerRegistry.RegisterForNavigation<CustomView, CustomViewModel>();
    }

    public void OnLoad()
    {
        Debug.WriteLine("CustomModule loaded");
    }

    public void OnUnload()
    {
        Debug.WriteLine("CustomModule unloaded");
    }

    public ModuleExport GetModuleInfo()
    {
        return new ModuleExport
        {
            Name = nameof(CustomModule),
            DisplayName = "自定义模块",
            Region = "ContentRegion",
            IsVisible = true,
            Order = 3
        };
    }
}
```

### 2. 在主应用中注册模块

在 `App.xaml.cs` 中：

```csharp
protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
{
    moduleCatalog.AddModule<DashboardModule>();
    moduleCatalog.AddModule<WorkstationModule>();
    moduleCatalog.AddModule<SettingsModule>();
}
```

### 3. 动态加载模块

```csharp
public class MainViewModel
{
    private readonly IModuleManager _moduleManager;

    public MainViewModel(IModuleManager moduleManager)
    {
        _moduleManager = moduleManager;
    }

    public void LoadCustomModule()
    {
        _moduleManager.LoadModule(nameof(CustomModule));
    }
}
```

## 模块通信

模块之间通过以下方式进行通信：

### 1. 事件系统（推荐）
使用 Prism 的 EventAggregator

```csharp
public class MyEvent : PubSubEvent<string> { }

// 发布事件
_eventAggregator.GetEvent<MyEvent>().Publish("message");

// 订阅事件
_eventAggregator.GetEvent<MyEvent>().Subscribe(OnEventReceived);
```

### 2. 服务接口
通过共享服务接口进行通信

```csharp
public interface ISharedService
{
    void DoSomething();
}

// 在容器中注册
containerRegistry.RegisterSingleton<ISharedService, SharedService>();
```

## 最佳实践

✅ **DO**
- ✅ 每个模块职责单一
- ✅ 使用接口定义模块间协议
- ✅ 通过事件进行模块间通信
- ✅ 模块内部做好日志记录
- ✅ 提供模块配置接口

❌ **DON'T**
- ❌ 直接引用其他模块的实现
- ❌ 在模块中硬编码配置
- ❌ 模块间强耦合
- ❌ 不完整的生命周期实现
- ❌ 循环模块依赖

## 扩展指南

### 新增模块步骤

1. **创建模块文件夹**
   ```
   Modules/YourModule/
   ```

2. **创建模块类**
   ```csharp
   public class YourModule : ModuleBase, IModuleLifecycle
   {
       // 实现接口...
   }
   ```

3. **创建视图和视图模型**
   ```csharp
   public class YourView : UserControl { }
   public class YourViewModel { }
   ```

4. **注册到模块目录**
   ```csharp
   moduleCatalog.AddModule<YourModule>();
   ```

5. **更新 Readme 文档**

## 相关文档

- [架构设计文档](../UI/docs/Architecture.md)
- [模块协作指南](../UI/docs/Module-Integration.md)
- [开发指南](../UI/docs/Development-Guide.md)
- [Prism 官方文档](https://prismlibrary.com/)

---

**更新时间**：2024-01-XX  
**框架版本**：Prism 9.0  
**状态**：✅ 活跃维护
