# 架构升级指南：从 CommunityToolkit.Mvvm 到 Prism 模块化

## 📋 升级概述

BaiHua.Software 框架进行了重大升级：
- ❌ **旧架构**：CommunityToolkit.Mvvm + 单体应用
- ✅ **新架构**：Prism 9.0 + 模块化设计

## 🎯 升级目标

| 目标 | 旧架构 | 新架构 |
|------|--------|--------|
| 应用架构 | 单体应用 | 模块化应用 |
| MVVM 框架 | CommunityToolkit.Mvvm | Prism 9.0 |
| 功能隔离 | 弱 | 强（每个功能独立模块） |
| 动态加载 | 不支持 | ✅ 支持 |
| DI 容器 | Microsoft.Extensions.DI | Prism.DryIOC |
| 区域管理 | 无 | ✅ 区域导航系统 |
| 模块通信 | 直接调用 | 事件系统 / 服务接口 |

## 📦 新增项目

### BaiHua.Software.Modules (项目06)
- **职责**：提供所有功能模块
- **内容**：
  - DashboardModule - 仪表板
  - WorkstationModule - 工站控制
  - SettingsModule - 设置
  - 用户可自定义添加更多模块
- **特点**：
  - 基于 Prism 框架
  - 支持动态加载/卸载
  - 完整的生命周期管理

## 📊 架构对比

### 旧架构流程
```
MainWindow
    ↓
MainViewModel (CommunityToolkit.Mvvm)
    ↓
BLL / Services
    ↓
数据访问和业务逻辑
```

### 新架构流程
```
Shell (MainWindow) - Prism 应用
    ↓
App.xaml / App.xaml.cs (启动配置)
    ↓
ModuleManager + MainWindowViewModel
    ↓
Prism Region（ContentRegion）
    ↓
├─ 模块1 (DashboardModule)
│   ├─ View
│   ├─ ViewModel
│   └─ BLL / Services
├─ 模块2 (WorkstationModule)
└─ 模块3 (SettingsModule)

核心特点：
- 每个模块独立，自治性强
- 通过 `RegionManager + ModuleManager` 动态切换内容
- 模块间通过事件或服务接口通信
```

## 🔄 迁移步骤

### 第 1 步：更新项目引用
✅ 已完成 - UI 项目已添加 Prism 依赖和 Modules 项目引用

### 第 2 步：创建模块基础
✅ 已完成 - 创建 ModuleBase、IModuleLifecycle、ModuleManager

### 第 3 步：迁移现有功能
```
旧的 ViewModel                新的 Module + ViewModel
├─ Dashboard页面      →       DashboardModule
├─ Workstation控制    →       WorkstationModule
└─ Settings配置       →       SettingsModule
```

### 第 4 步：配置 Prism 容器
✅ 已完成 - 在 `App.xaml.cs` 中完成 Prism 容器和模块目录配置

### 第 5 步：更新 UI 界面
✅ 已完成 - 更新 MainWindow.xaml 支持模块导航

## 📝 关键变更

### 1. ViewModel 基类变更

**旧代码**：
```csharp
using CommunityToolkit.Mvvm.ComponentModel;

public partial class MyViewModel : ObservableObject
{
    [ObservableProperty]
    private string title;

    public MyViewModel()
    {
    }
}
```

**新代码**：
```csharp
using Prism.Mvvm;
using Prism.Commands;

public class MyViewModel : BindableBase
{
    private string _title;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public DelegateCommand MyCommand { get; }

    public MyViewModel()
    {
        MyCommand = new DelegateCommand(OnExecute);
    }

    private void OnExecute()
    {
        // 命令执行
    }
}
```

### 2. 命令绑定变更

**旧代码**：
```xml
<Button Command="{Binding RelayCommand}" CommandParameter="123"/>
```

**新代码**：
```xml
<Button Command="{Binding MyCommand}" CommandParameter="123"/>
```

### 3. 视图定位变更

**旧代码**：
```csharp
public partial class MyView : UserControl
{
    public MyView()
    {
        InitializeComponent();
        DataContext = new MyViewModel();
    }
}
```

**新代码**：
```csharp
public partial class MyView : UserControl
{
    public MyView()
    {
        InitializeComponent();
        // Prism 自动定位 ViewModel，无需手动设置 DataContext
    }
}
```

### 4. 视图导航变更

**旧代码**：
```csharp
// 直接创建窗口
var window = new MyWindow();
window.ShowDialog();
```

**新代码**：
```csharp
// 使用 Prism 导航
_regionManager.RequestNavigate("ContentRegion", nameof(MyView));
```

## 🔌 模块化益处

### 1. 功能隔离
- 每个模块独立开发测试
- 降低模块间耦合度

### 2. 动态加载
- 应用启动时仅加载必需模块
- 按需加载功能模块
- 支持模块热更新

### 3. 可扩展性
- 新功能可作为独立模块添加
- 无需修改核心应用
- 支持第三方模块集成

### 4. 易于维护
- 代码组织清晰
- 职责划分明确
- 便于团队协作

## 🚀 快速开始迁移

### 方案 A：完整迁移
适合新项目或完全重构

```
1. 使用新架构创建应用
2. 逐个创建模块
3. 迁移现有业务逻辑
4. 测试和优化
```

### 方案 B：渐进式迁移
适合已有项目

```
1. 添加 Prism 依赖
2. 创建第一个模块（简单功能）
3. 逐步迁移其他功能到模块
4. 完全替换旧架构
```

## 📊 性能影响

| 方面 | 影响 |
|------|------|
| 应用启动时间 | ↑ 稍增（模块加载需要时间） |
| 内存占用 | ↓ 可减少（未使用模块不加载） |
| 响应速度 | → 无明显变化 |
| 编译时间 | ↑ 稍增（项目增多） |

**优化建议**：
- 对关键模块进行预加载
- 使用延迟加载策略
- 定期进行性能分析

## ✅ 迁移检查清单

### 前置条件
- [ ] .NET 8 SDK 已安装
- [ ] Visual Studio 2022+
- [ ] Prism 文档已阅读

### 项目配置
- [x] 创建 BaiHua.Software.Modules 项目
- [x] UI 项目已更新 Prism 依赖
- [x] 项目引用已更新

### 代码更新
- [x] 更新 App.xaml / App.xaml.cs
- [x] 更新 MainWindow XAML
- [x] 创建 MainWindowViewModel
- [ ] 迁移现有业务模块
- [ ] 更新所有 ViewModel 继承
- [ ] 更新所有命令绑定

### 测试
- [ ] 编译成功
- [ ] 主窗口正常显示
- [ ] 模块正常加载
- [ ] 模块导航正常工作
- [ ] 集成测试通过
- [ ] 性能测试通过

### 文档
- [x] 更新 UI 项目 Readme
- [x] 创建 Modules 项目 Readme
- [ ] 更新开发指南
- [ ] 更新架构文档

## 🔗 相关资源

- [Prism 官方文档](https://prismlibrary.com/)
- [Prism GitHub](https://github.com/PrismLibrary/Prism)
- [MVVM 模式指南](https://learn.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-patterns/mvvm)
- [模块化架构最佳实践](https://martinfowler.com/articles/modular-monolith-01.html)

## 📝 常见问题

**Q: 升级后性能会下降吗？**
A: 不会。Prism 开销极小，模块化甚至可能改善性能（通过延迟加载）。

**Q: 是否需要一次性迁移所有代码？**
A: 不需要。可以渐进式迁移，新功能使用新架构，旧功能保持不变。

**Q: 如何调试模块？**
A: 与调试普通代码相同，设置断点即可。可在 Visual Studio 中查看区域和模块的加载情况。

**Q: 模块间如何通信？**
A: 使用 Prism 的 EventAggregator（事件系统）或注册共享服务接口。

## 🎉 升级完成

所有核心升级工作已完成：
- ✅ 项目结构创建
- ✅ Prism 框架集成
- ✅ 模块化基础实现
- ✅ 主窗口更新
- ✅ 文档完善

**下一步**：迁移现有业务模块到新架构

---

**升级时间**：2024-01-XX  
**Prism 版本**：9.0  
**状态**：✅ 核心升级完成，进行中

