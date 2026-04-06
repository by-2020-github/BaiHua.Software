# BaiHua.Software 快速参考指南

## 📍 项目文件树

```
BaiHua.Software/
├── 01 BaiHua.Software.Common/          [工具层] - 通用帮助类
│   ├── Readme.md
│   └── ...
│
├── 02 BaiHua.Software.HAL/             [硬件抽象层] - 设备驱动
│   ├── Vision/                         - 视觉模块 (OpenCvSharp4)
│   ├── Communication/                  - 通信模块 (FluentModbus)
│   ├── Motion/                         - 运动控制模块
│   ├── Readme.md
│   └── ...
│
├── 03 BaiHua.Software.Infrastructure/  [基础设施层] - 数据与日志
│   ├── Repositories/                   - 数据仓储 (FreeSql)
│   ├── Logging/                        - 日志服务 (Serilog)
│   ├── Readme.md
│   └── ...
│
├── 02 BaiHua.Software.Core/            [核心契约层] - 接口与模型
│   ├── Interfaces/                     - 业务接口
│   ├── Enums/                          - 全局枚举
│   ├── DTOs/                           - 数据传输对象
│   ├── Models/                         - 领域模型
│   ├── Readme.md
│   └── ...
│
├── 04 BaiHua.Software.Services/        [应用服务层] - 业务服务
│   ├── AlarmService/                   - 报警管理
│   ├── RecipeService/                  - 配方管理
│   ├── AuthService/                    - 权限管理
│   ├── ConfigService/                  - 配置管理
│   ├── Readme.md
│   └── ...
│
├── 05 BaiHua.Software.BLL/             [业务逻辑层] - 核心大脑
│   ├── Workstations/                   - 工站控制
│   ├── Process/                        - 工艺流程
│   ├── Vision/                         - 视觉处理
│   ├── Readme.md
│   └── ...
│
└── 100 BaiHua.Software.UI/             [用户界面层] - WPF应用
    ├── Views/                          - XAML 界面
    ├── ViewModels/                     - 视图模型
    ├── App.xaml                        - 应用配置
    ├── MainWindow.xaml
    ├── AssemblyInfo.cs
    ├── docs/                           📍 文档文件夹
    │   ├── Architecture.md             - 架构设计文档
    │   ├── QuickStart.md               - 快速参考指南（本文件）
    │   ├── Module-Details.md           - 模块详细说明
    │   ├── Development-Guide.md        - 开发指南
    │   └── Troubleshooting.md          - 故障排查
    ├── Readme.md
    └── ...
```

---

## 🚀 快速开始

### 1️⃣ 环境要求
- .NET 8 SDK
- Visual Studio 2022 或更新版本
- Windows 10/11（WPF 应用）

### 2️⃣ 编译与运行
```powershell
# 在项目根目录
dotnet build

# 运行 UI 应用
dotnet run --project "BaiHua.Software.UI\100 BaiHua.Software.UI.csproj"
```

### 3️⃣ 项目配置
在 `Program.cs` 中配置依赖注入：
```csharp
IServiceCollection services = new ServiceCollection();

// 注册 Infrastructure 层服务
services.AddScoped<IRepository, FreeSqlRepository>();
services.AddScoped<ILogger, SerilogLogger>();

// 注册 HAL 层驱动
services.AddScoped<IHALDriver, HardwareDriver>();
services.AddScoped<IVisionEngine, OpenCvVisionEngine>();

// 注册 Services 层服务
services.AddScoped<IAlarmService, AlarmService>();
services.AddScoped<IConfigService, ConfigService>();

// 注册 BLL 层
services.AddScoped<IWorkstationBLL, WorkstationBLL>();

var provider = services.BuildServiceProvider();
```

---

## 📋 每层的职责清单

### ✅ Common 层
- [ ] 字符串处理（验证、格式化）
- [ ] JSON 序列化
- [ ] 加密解密
- [ ] 文件 I/O
- [ ] 日期时间处理
- [ ] 集合扩展

### ✅ Core 层
- [ ] 定义所有接口
- [ ] 定义枚举类型
- [ ] 定义 DTO
- [ ] 定义实体模型
- [ ] ❌ 不添加任何外部包依赖

### ✅ Infrastructure 层
- [ ] 数据库连接管理
- [ ] 仓储实现（CRUD）
- [ ] 事务处理
- [ ] 日志记录
- [ ] 缓存管理
- [ ] 配置读取

### ✅ HAL 层
- [ ] 硬件初始化
- [ ] 硬件控制命令
- [ ] 硬件数据采集
- [ ] 异常处理
- [ ] 驱动版本管理

### ✅ Services 层
- [ ] 报警规则执行
- [ ] 配方参数管理
- [ ] 用户权限检查
- [ ] 系统配置管理
- [ ] 数据统计计算
- [ ] 日志查询

### ✅ BLL 层
- [ ] 工艺流程编排
- [ ] 状态机实现
- [ ] 硬件调用协调
- [ ] 业务规则实现
- [ ] 异常处理与恢复
- [ ] 性能优化

### ✅ UI 层
- [ ] XAML 页面设计
- [ ] ViewModel 数据绑定
- [ ] 用户交互处理
- [ ] 界面状态管理
- [ ] 异常提示

---

## 🔧 常见开发任务

### 任务 1：添加新的硬件驱动

**步骤**：
1. 在 Core 层定义接口
   ```csharp
   // Core/Interfaces/INewDeviceDriver.cs
   public interface INewDeviceDriver
   {
       void Initialize();
       void SendCommand(string cmd);
       string ReadData();
   }
   ```

2. 在 HAL 层实现驱动
   ```csharp
   // HAL/NewDevice/NewDeviceDriver.cs
   public class NewDeviceDriver : INewDeviceDriver
   {
       public void Initialize() { /* ... */ }
       public void SendCommand(string cmd) { /* ... */ }
       public string ReadData() { /* ... */ }
   }
   ```

3. 在 BLL 层调用驱动
   ```csharp
   public class WorkstationBLL
   {
       private readonly INewDeviceDriver _device;
       
       public WorkstationBLL(INewDeviceDriver device)
       {
           _device = device;
       }
       
       public void RunProcess()
       {
           _device.Initialize();
           _device.SendCommand("START");
           var result = _device.ReadData();
       }
   }
   ```

### 任务 2：添加新的业务服务

**步骤**：
1. 在 Core 层定义接口
   ```csharp
   public interface INewService
   {
       void DoSomething();
   }
   ```

2. 在 Services 层实现服务
   ```csharp
   public class NewService : INewService
   {
       public void DoSomething() { /* ... */ }
   }
   ```

3. 在 DI 容器中注册
   ```csharp
   services.AddScoped<INewService, NewService>();
   ```

4. 在 BLL 或 ViewModel 中使用
   ```csharp
   public class MyViewModel
   {
       private readonly INewService _service;
       
       public MyViewModel(INewService service)
       {
           _service = service;
       }
   }
   ```

### 任务 3：添加新的 UI 页面

**步骤**：
1. 创建 XAML 文件
   ```
   UI/Views/NewPage.xaml
   ```

2. 创建 ViewModel
   ```csharp
   // UI/ViewModels/NewPageViewModel.cs
   public class NewPageViewModel
   {
       private readonly IWorkstationBLL _bll;
       
       public RelayCommand StartCommand { get; }
       
       public NewPageViewModel(IWorkstationBLL bll)
       {
           _bll = bll;
           StartCommand = new RelayCommand(() => _bll.Start());
       }
   }
   ```

3. 绑定 DataContext
   ```csharp
   // UI/Views/NewPage.xaml.cs
   public partial class NewPage : UserControl
   {
       public NewPage(NewPageViewModel viewModel)
       {
           InitializeComponent();
           DataContext = viewModel;
       }
   }
   ```

---

## 📊 数据流参考

### 工站启动流程
```
UI Button Click
    ↓
ViewModel.StartCommand
    ↓
BLL.Start()
    ↓
├─ IConfigService.GetRecipe()
├─ IHALDriver.Initialize()
└─ IHALDriver.MoveRobot()
    ↓
HAL 执行硬件操作
    ↓
BLL 处理结果
    ↓
IAlarmService.RaiseAlarm()（如果异常）
    ↓
Infrastructure 保存日志和数据
    ↓
ViewModel 更新 UI
    ↓
UI 显示结果
```

---

## 🐛 调试技巧

### 1. 查看依赖注入是否正确
```csharp
// 在 App.cs 添加日志
var provider = services.BuildServiceProvider();
var workstation = provider.GetRequiredService<IWorkstationBLL>();
Console.WriteLine($"Resolved: {workstation?.GetType().Name}");
```

### 2. 追踪硬件通信
在 HAL 层添加日志：
```csharp
public class HardwareDriver : IHALDriver
{
    private readonly ILogger _logger;
    
    public void SendCommand(string cmd)
    {
        _logger.LogInformation($"Sending: {cmd}");
        // 发送命令...
        _logger.LogInformation("Command sent");
    }
}
```

### 3. 监控数据库操作
查看 FreeSql 日志输出（在 Infrastructure 配置中启用）

---

## 📝 编码规范

### 命名规范
```csharp
// Interface（I开头）
public interface IWorkstationService { }

// Class（PascalCase）
public class WorkstationService : IWorkstationService { }

// 私有字段（_开头）
private readonly IHALDriver _driver;

// 属性（PascalCase）
public string Status { get; set; }

// 方法（PascalCase）
public void StartProcess() { }

// 常量（UPPER_SNAKE_CASE）
private const int MAX_RETRIES = 3;
```

### 文件组织
```
Project/
├── Interfaces/          [接口定义]
├── Models/              [数据模型]
├── Services/            [业务实现]
├── Exceptions/          [异常类]
└── Constants/           [常量定义]
```

---

## 📞 支持与联系

如有问题，请：
1. 查看完整架构文档：`Architecture.md`
2. 查看模块详细说明：`Module-Details.md`
3. 查看故障排查指南：`Troubleshooting.md`

---

**最后更新**：2024-01-XX  
**.NET 版本**：8.0  
**框架稳定性**：✅ Production Ready
