# UI (100 BaiHua.Software.UI)

## 概述

**BaiHua.Software.UI** 是基于 **WPF + Prism + 模块化设计** 的用户界面层。采用模块化架构，每个功能模块独立开发和维护。

## 技术栈

- 🎨 **WPF**：微软原生 UI 框架
- 🔷 **Prism 9.0**：业界成熟的 MVVM 和模块化框架
- 📦 **模块化设计**：支持动态加载和卸载功能模块
- 🔌 **DryIOC**：轻量级依赖注入容器

## 架构特点

### 模块化设计
- 每个功能独立为一个模块（Dashboard、Workstation、Settings等）
- 模块间通过接口和事件进行通信
- 支持动态加载和卸载

### MVVM 模式
- 使用 Prism 的 BindableBase 和 DelegateCommand
- 共享窗口通过构造函数注入 ViewModel，模块视图内部绑定各自 ViewModel
- 强类型的命令和属性更改通知

### 模块承载与导航
- 使用 `App.xaml.cs` 作为 Prism 入口和容器注册点
- 主窗口通过 Prism `Region` 承载当前模块视图
- 左侧模块列表支持筛选、首页、前进和后退导航

## 项目结构

```
UI/
├── Views/                      # 视图文件
│   ├── MainWindow.xaml        # 主窗口
│   └── ...
│
├── ViewModels/                 # 视图模型
│   ├── MainWindowViewModel.cs # 主窗口视图模型
│   └── ...
│
├── App.xaml                    # 应用配置
├── App.xaml.cs                # 应用代码
├── MainWindow.xaml             # 主窗口
├── MainWindow.xaml.cs          # 主窗口代码
│
└── docs/                       # 文档
    ├── Architecture.md         # 架构文档
    ├── QuickStart.md           # 快速开始
    └── ...
```

## 核心类

### MainWindowViewModel
主窗口视图模型，负责：
- 管理模块列表显示
- 处理模块选择命令
- 与模块管理器、`IRegionManager` 交互

### App
Prism 应用入口，负责：
- 初始化 DryIOC 容器
- 注册类型和服务
- 注册 `BLL/Services` 依赖
- 配置模块目录
- 创建和初始化 Shell

## 使用示例

### 创建视图和视图模型

```csharp
// 视图
public class MyView : UserControl { }

// 视图模型
public class MyViewModel : BindableBase
{
    public DelegateCommand MyCommand { get; }

    public MyViewModel()
    {
        MyCommand = new DelegateCommand(OnExecute);
    }

    private void OnExecute()
    {
        // 执行命令
    }
}
```

### 注册视图和导航

```csharp
// 在模块中注册
containerRegistry.RegisterForNavigation<MyView, MyViewModel>();

// 在主窗口中切换模块内容
SelectedModule = module;
```

### 使用命令

```xml
<!-- XAML 中绑定命令 -->
<Button Content="Click Me" Command="{Binding MyCommand}"/>
<Button Content="With Parameter" Command="{Binding MyCommand}" CommandParameter="param"/>
```

## 模块结构

所有功能模块位于 `BaiHua.Software.Modules` 项目中，包括：

- **DashboardModule** - 仪表板模块
- **WorkstationModule** - 工站控制模块
- **SettingsModule** - 设置模块

每个模块包含：
- 模块类（继承 ModuleBase）
- Views（视图文件）
- ViewModels（视图模型）

## 最佳实践

✅ **DO**
- ✅ 使用 Prism 的数据绑定机制
- ✅ 使用 DelegateCommand 处理用户命令
- ✅ 在 ViewModel 中处理业务逻辑
- ✅ 使用 Prism 的事件系统进行模块间通信
- ✅ 为视图创建对应的 ViewModel

❌ **DON'T**
- ❌ 在 View 中处理业务逻辑
- ❌ 直接创建和销毁模块
- ❌ 使用硬编码的导航路径
- ❌ 跳过视图模型直接修改 UI
- ❌ 循环模块依赖

## 扩展指南

### 新增 UI 页面

1. **创建视图**
   ```xml
   <!-- Views/MyView.xaml -->
   <UserControl x:Class="BaiHua.Software.UI.Views.MyView"...>
   </UserControl>
   ```

2. **创建视图模型**
   ```csharp
   // ViewModels/MyViewModel.cs
   public class MyViewModel : BindableBase { }
   ```

3. **注册到模块**
   ```csharp
   containerRegistry.RegisterForNavigation<MyView, MyViewModel>();
   ```

### 新增模块

参考 `BaiHua.Software.Modules` 项目中的 Readme.md

## 相关文档

- [架构设计文档](./docs/Architecture.md)
- [模块协作指南](./docs/Module-Integration.md)
- [开发指南](./docs/Development-Guide.md)
- [Prism 官方文档](https://prismlibrary.com/)

## 依赖关系

```
UI (Prism 应用)
    ↓
Modules (功能模块)
    ↓
BLL (业务逻辑)
    ↓
Services + HAL (服务和硬件)
    ↓
Infrastructure (基础设施)
    ↓
Common (通用工具)
```

---

**更新时间**：2024-01-XX  
**框架版本**：.NET 8 + Prism 9.0  
**状态**：✅ 使用 Prism 和模块化设计
