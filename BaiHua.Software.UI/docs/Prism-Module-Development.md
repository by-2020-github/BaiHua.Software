# Prism 模块开发快速指南

## 📋 开发前准备

### 环境检查
- [ ] .NET 8 SDK 已安装
- [ ] Visual Studio 2022+ 已打开 BaiHua.Software 解决方案
- [ ] 已阅读 [架构文档](./Architecture.md)
- [ ] 已了解 Prism 基础概念

### 必读文档
1. [Architecture.md](./Architecture.md) - 项目架构
2. [Module-Integration.md](./Module-Integration.md) - 模块通信
3. 本文档 - 快速开发指南

---

## 🚀 10 分钟快速开始

### 第 1 步：创建模块文件夹

在 `BaiHua.Software.Modules` 项目中创建模块文件夹：

```
Modules/
├── YourModuleName/
│   ├── YourModuleNameModule.cs    # 模块类
│   ├── Views/
│   │   └── YourView.xaml          # 视图
│   ├── ViewModels/
│   │   └── YourViewModel.cs       # 视图模型
│   └── Services/
│       └── YourService.cs         # 业务服务
```

### 第 2 步：创建模块类

```csharp
using Prism.Ioc;
using Prism.Modularity;
using BaiHua.Software.Modules;

namespace BaiHua.Software.Modules.YourModuleName;

public class YourModuleNameModule : ModuleBase, IModuleLifecycle
{
    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // 注册视图和视图模型
        containerRegistry.RegisterForNavigation<YourView, YourViewModel>();
    }

    public override void OnInitialized(IContainerProvider containerProvider)
    {
        // 模块初始化完成
    }

    public void OnLoad()
    {
        Debug.WriteLine("YourModuleNameModule loaded");
    }

    public void OnUnload()
    {
        Debug.WriteLine("YourModuleNameModule unloaded");
    }

    public ModuleExport GetModuleInfo()
    {
        return new ModuleExport
        {
            Name = nameof(YourModuleNameModule),
            DisplayName = "你的模块名称",
            Region = "ContentRegion",
            IsVisible = true,
            Order = 4  // 显示顺序
        };
    }
}
```

### 第 3 步：创建视图

```xml
<!-- Views/YourView.xaml -->
<UserControl x:Class="BaiHua.Software.Modules.YourModuleName.Views.YourView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             Background="White">
    <Grid>
        <StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
            <TextBlock Text="{Binding Title}" FontSize="18" FontWeight="Bold"/>
            <Button Content="点击" Command="{Binding ClickCommand}" Margin="0,10,0,0"/>
        </StackPanel>
    </Grid>
</UserControl>
```

```csharp
// Views/YourView.xaml.cs
namespace BaiHua.Software.Modules.YourModuleName.Views;

public partial class YourView : UserControl
{
    public YourView()
    {
        InitializeComponent();
    }
}
```

### 第 4 步：创建视图模型

```csharp
using Prism.Commands;
using Prism.Mvvm;

namespace BaiHua.Software.Modules.YourModuleName.ViewModels;

public class YourViewModel : BindableBase
{
    private string _title = "欢迎使用我的模块";

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public DelegateCommand ClickCommand { get; }

    public YourViewModel()
    {
        ClickCommand = new DelegateCommand(OnClick);
    }

    private void OnClick()
    {
        Title = "已点击按钮！";
    }
}
```

### 第 5 步：注册模块

在 `UI/App.xaml.cs` 中：

```csharp
protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
{
    // 添加新模块
    moduleCatalog.AddModule<YourModuleNameModule>();
}
```

---

## 💡 常用开发模式

### 模式 1：简单数据展示

```csharp
public class SimpleViewModel : BindableBase
{
    private string _data;
    
    public string Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }

    public SimpleViewModel()
    {
        Data = "初始数据";
    }
}
```

```xml
<TextBlock Text="{Binding Data}"/>
```

### 模式 2：带参数的命令

```csharp
public DelegateCommand<string> MyCommand { get; }

public MyViewModel()
{
    MyCommand = new DelegateCommand<string>(OnExecute);
}

private void OnExecute(string parameter)
{
    MessageBox.Show($"参数：{parameter}");
}
```

```xml
<Button Command="{Binding MyCommand}" CommandParameter="Hello"/>
```

### 模式 3：异步操作

```csharp
public DelegateCommand LoadDataCommand { get; }

public MyViewModel(IService service)
{
    _service = service;
    LoadDataCommand = new DelegateCommand(OnLoadData);
}

private async void OnLoadData()
{
    try
    {
        var data = await _service.GetDataAsync();
        Data = data;
    }
    catch (Exception ex)
    {
        MessageBox.Show($"错误：{ex.Message}");
    }
}
```

### 模式 4：模块间通信（事件）

```csharp
// 定义事件
public class MyEvent : PubSubEvent<string> { }

// 发送事件
public class Module1ViewModel : BindableBase
{
    private readonly IEventAggregator _eventAggregator;
    
    public DelegateCommand SendCommand { get; }

    public Module1ViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        SendCommand = new DelegateCommand(OnSend);
    }

    private void OnSend()
    {
        _eventAggregator.GetEvent<MyEvent>().Publish("Hello from Module1");
    }
}

// 接收事件
public class Module2ViewModel : BindableBase
{
    private readonly IEventAggregator _eventAggregator;
    private string _message;
    
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public Module2ViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        _eventAggregator.GetEvent<MyEvent>().Subscribe(OnEventReceived);
    }

    private void OnEventReceived(string message)
    {
        Message = message;
    }
}
```

### 模式 5：共享服务

```csharp
// 定义服务接口（在 Core 或 Modules 中）
public interface ISharedService
{
    void DoSomething();
}

// 实现服务（在模块中）
public class SharedService : ISharedService
{
    public void DoSomething()
    {
        Debug.WriteLine("Service executed");
    }
}

// 在模块中注册
public override void RegisterTypes(IContainerRegistry containerRegistry)
{
    containerRegistry.RegisterSingleton<ISharedService, SharedService>();
}

// 在 ViewModel 中使用
public class MyViewModel : BindableBase
{
    private readonly ISharedService _service;

    public MyViewModel(ISharedService service)
    {
        _service = service;
    }

    public void Execute()
    {
        _service.DoSomething();
    }
}
```

---

## 🎯 开发清单

### 创建模块前
- [ ] 明确模块的职责（单一职责原则）
- [ ] 设计模块的公共接口
- [ ] 规划与其他模块的通信方式
- [ ] 确定模块的显示区域

### 编码阶段
- [ ] 创建模块类（继承 ModuleBase）
- [ ] 创建视图（XAML）
- [ ] 创建视图模型（BindableBase）
- [ ] 创建业务服务（如需要）
- [ ] 实现注册和初始化
- [ ] 实现生命周期方法（OnLoad/OnUnload）

### 测试阶段
- [ ] 编译通过（无错误或警告）
- [ ] 模块正常加载
- [ ] UI 正常显示
- [ ] 命令正常执行
- [ ] 与其他模块通信正常
- [ ] 数据绑定正常工作
- [ ] 错误处理完善

### 完成阶段
- [ ] 添加注释和文档
- [ ] 更新模块 Readme
- [ ] 代码审查
- [ ] 提交代码

---

## 🔍 常见问题排查

### 模块无法加载

**问题**：模块类未被找到或加载失败

**排查步骤**：
1. 检查模块类是否继承 `ModuleBase`
2. 检查是否在 `ConfigureModuleCatalog` 中注册了模块
3. 查看调试输出，看是否有异常信息
4. 检查命名空间是否正确

### ViewModel 未被正确注入

**问题**：ViewModel 总是 null 或无法获得注入的依赖

**排查步骤**：
1. 检查视图是否添加了 `prism:ViewModelLocator.AutoWireViewModel="True"`
2. 检查 ViewModel 命名（View 对应 ViewModel，如 MyView → MyViewModel）
3. 检查是否在模块中注册了 `RegisterForNavigation<View, ViewModel>()`
4. 检查依赖是否已注册到容器

### 命令无法执行

**问题**：按钮点击时命令没有执行

**排查步骤**：
1. 检查命令绑定是否正确 `Command="{Binding MyCommand}"`
2. 检查 ViewModel 中是否正确创建了 `DelegateCommand`
3. 检查 DataContext 是否正确设置
4. 在命令处理方法中添加调试输出

### 数据绑定失效

**问题**：修改 ViewModel 属性，UI 不更新

**排查步骤**：
1. 检查 ViewModel 是否继承 `BindableBase`
2. 检查属性是否使用 `SetProperty` 方法
3. 检查 XAML 绑定路径是否正确
4. 检查是否启用了 WPF 绑定诊断

---

## 📚 完整示例

### 完整的仪表板模块示例

**文件结构**：
```
Modules/ExampleDashboard/
├── ExampleDashboardModule.cs
├── Views/
│   └── ExampleDashboardView.xaml
├── ViewModels/
│   └── ExampleDashboardViewModel.cs
└── Services/
    └── DashboardService.cs
```

**ExampleDashboardModule.cs**：
```csharp
using Prism.Ioc;
using BaiHua.Software.Modules;
using BaiHua.Software.Modules.ExampleDashboard.Views;
using BaiHua.Software.Modules.ExampleDashboard.ViewModels;

namespace BaiHua.Software.Modules.ExampleDashboard;

public class ExampleDashboardModule : ModuleBase, IModuleLifecycle
{
    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<ExampleDashboardView, ExampleDashboardViewModel>();
    }

    public void OnLoad()
    {
        Debug.WriteLine("ExampleDashboardModule loaded");
    }

    public void OnUnload()
    {
        Debug.WriteLine("ExampleDashboardModule unloaded");
    }

    public ModuleExport GetModuleInfo()
    {
        return new ModuleExport
        {
            Name = nameof(ExampleDashboardModule),
            DisplayName = "示例仪表板",
            Region = "ContentRegion",
            IsVisible = true,
            Order = 10
        };
    }
}
```

**ExampleDashboardViewModel.cs**：
```csharp
using Prism.Commands;
using Prism.Mvvm;

namespace BaiHua.Software.Modules.ExampleDashboard.ViewModels;

public class ExampleDashboardViewModel : BindableBase
{
    private string _title = "示例仪表板";
    private int _counter = 0;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public int Counter
    {
        get => _counter;
        set => SetProperty(ref _counter, value);
    }

    public DelegateCommand IncrementCommand { get; }

    public ExampleDashboardViewModel()
    {
        IncrementCommand = new DelegateCommand(OnIncrement);
    }

    private void OnIncrement()
    {
        Counter++;
        Title = $"计数：{Counter}";
    }
}
```

**ExampleDashboardView.xaml**：
```xml
<UserControl x:Class="BaiHua.Software.Modules.ExampleDashboard.Views.ExampleDashboardView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             Background="White">
    <Grid>
        <StackPanel VerticalAlignment="Center" HorizontalAlignment="Center" Spacing="10">
            <TextBlock Text="{Binding Title}" FontSize="18" FontWeight="Bold" TextAlignment="Center"/>
            <TextBlock Text="{Binding Counter, StringFormat='计数：{0}'}" FontSize="16" TextAlignment="Center"/>
            <Button Content="增加" Command="{Binding IncrementCommand}" Padding="20,10" Width="100"/>
        </StackPanel>
    </Grid>
</UserControl>
```

---

## 🚀 下一步

1. **复制模板创建新模块**：使用上面的示例作为模板
2. **完成模块开发**：按照清单逐步完成
3. **测试模块**：确保模块正常加载和工作
4. **提交代码**：提交到版本控制
5. **更新文档**：补充模块相关的文档

---

**提示**：
- 遇到问题？查看 Prism 官方文档：https://prismlibrary.com/
- 需要帮助？参考 [Module-Integration.md](./Module-Integration.md)
- 有建议？更新此文档

