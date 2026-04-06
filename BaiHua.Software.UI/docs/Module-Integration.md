# BaiHua.Software 模块协作指南

## 🤝 模块间通信规范

### 基本原则

1. **只通过接口通信**
   - 上层通过接口调用下层功能
   - 下层不了解上层的实现
   - 所有接口定义在 Core 层

2. **单向依赖流**
   ```
   UI → BLL → (HAL + Services) → Infrastructure → Common
   ```

3. **通过依赖注入**
   - 不直接 new 对象
   - 所有依赖通过构造函数注入
   - 配置统一在 DI 容器中

---

## 🔄 常见交互模式

### 模式 1：UI → ViewModel → BLL

**场景**：用户点击按钮触发业务流程

```csharp
// UI/Views/MainWindow.xaml
<Button Content="启动" Command="{Binding StartCommand}" />

// UI/ViewModels/MainViewModel.cs
public class MainViewModel
{
    private readonly IWorkstationBLL _bll;
    
    public RelayCommand StartCommand { get; }
    
    public MainViewModel(IWorkstationBLL bll)
    {
        _bll = bll;
        StartCommand = new RelayCommand(async () => await _bll.Start());
    }
}

// BLL/WorkstationBLL.cs
public class WorkstationBLL : IWorkstationBLL
{
    private readonly IHALDriver _hal;
    private readonly IAlarmService _alarm;
    
    public async Task Start()
    {
        try
        {
            await _hal.Initialize();
            await _hal.MoveRobot();
        }
        catch (Exception ex)
        {
            await _alarm.RaiseAlarm(ex.Message);
        }
    }
}
```

**数据流**：
```
Click Event → Command → ViewModel → BLL → HAL → Hardware
```

---

### 模式 2：BLL 调用多个服务

**场景**：复杂业务流程需要多个服务协作

```csharp
public class RecipeBLL : IRecipeBLL
{
    private readonly IRecipeService _recipe;
    private readonly IConfigService _config;
    private readonly IAlarmService _alarm;
    private readonly IRepository _repository;
    
    public async Task ExecuteRecipe(int recipeId)
    {
        // 1. 从服务获取配方
        var recipe = await _recipe.GetRecipe(recipeId);
        
        // 2. 获取系统配置
        var config = await _config.GetConfig("DefaultTimeout");
        
        // 3. 执行业务逻辑
        try
        {
            ProcessRecipe(recipe, config);
        }
        catch (Exception ex)
        {
            // 4. 告警处理
            await _alarm.RaiseAlarm($"Recipe failed: {ex.Message}");
        }
        
        // 5. 保存结果
        await _repository.SaveExecution(recipeId, DateTime.Now);
    }
}
```

**依赖图**：
```
RecipeBLL
├── IRecipeService
├── IConfigService
├── IAlarmService
└── IRepository
    └── Infrastructure (数据库)
```

---

### 模式 3：HAL 驱动链调用

**场景**：硬件操作需要多个驱动协作

```csharp
public class VisionInspectionBLL : IVisionInspectionBLL
{
    private readonly IHALDriver _hal;
    private readonly IVisionService _vision;
    
    public async Task<InspectionResult> Inspect()
    {
        // 1. 移动相机到检测位置
        await _hal.Motion.MoveCamera(new Point(100, 100));
        
        // 2. 采集图像
        var image = await _hal.Vision.CaptureImage();
        
        // 3. 执行检测算法
        var result = await _vision.Detect(image);
        
        // 4. 根据结果执行IO操作
        if (!result.IsOK)
        {
            await _hal.IO.TriggerAlarmLight();
        }
        
        return result;
    }
}
```

**硬件操作顺序**：
```
Motion (移动) → Vision (采集) → Service (分析) → IO (输出)
```

---

### 模式 4：异步事件处理

**场景**：后台任务完成后通知 UI 更新

```csharp
// Services 层定义事件
public interface IWorkstationService
{
    event EventHandler<StatusChangedEventArgs>? StatusChanged;
    Task Start();
}

// BLL 层实现事件触发
public class WorkstationBLL : IWorkstationBLL
{
    public event EventHandler<StatusChangedEventArgs>? StatusChanged;
    
    public async Task Start()
    {
        var oldStatus = Status;
        Status = WorkstationStatus.Running;
        StatusChanged?.Invoke(this, new StatusChangedEventArgs(oldStatus, Status));
        
        await ExecuteWork();
        
        Status = WorkstationStatus.Idle;
        StatusChanged?.Invoke(this, new StatusChangedEventArgs(oldStatus, Status));
    }
}

// ViewModel 订阅事件
public class MainViewModel
{
    private readonly IWorkstationBLL _bll;
    
    public MainViewModel(IWorkstationBLL bll)
    {
        _bll = bll;
        _bll.StatusChanged += OnStatusChanged;
    }
    
    private void OnStatusChanged(object? sender, StatusChangedEventArgs e)
    {
        UpdateUI(e.NewStatus);
    }
}
```

**事件流**：
```
BLL 状态改变 → 触发事件 → ViewModel 处理 → 更新绑定属性 → UI 自动刷新
```

---

## 🔌 接口设计指南

### ✅ 好的接口设计

```csharp
// ✅ 粒度合适
public interface IWorkstationService
{
    Task Start();
    Task Stop();
    WorkstationStatus GetStatus();
}

// ✅ 明确的职责
public interface IAlarmService
{
    Task RaiseAlarm(AlarmInfo alarm);
    Task ClearAlarm(int alarmId);
    Task<List<AlarmInfo>> GetAlarmHistory();
}

// ✅ 异步操作使用 Task
public interface IHardwareDriver
{
    Task Initialize();
    Task<byte[]> ReadData();
    Task WriteData(byte[] data);
}

// ✅ 返回值明确
public interface IConfigService
{
    Task<ConfigInfo> GetConfig(string key);
    Task SetConfig(string key, string value);
}
```

### ❌ 不好的接口设计

```csharp
// ❌ 职责混乱
public interface IGodService
{
    void DoEverything();
}

// ❌ 粒度太细
public interface IPoint
{
    int GetX();
    int GetY();
    void SetX(int x);
    void SetY(int y);
}

// ❌ 同步操作（阻塞 UI）
public interface IHardwareDriver
{
    byte[] ReadData();  // ❌ 应该是异步
}

// ❌ 返回值模糊
public interface IService
{
    object DoSomething();  // ❌ 应该是具体类型
}
```

---

## 🗂️ 依赖注入配置

### 完整的 DI 配置示例

```csharp
// Program.cs 或 App.xaml.cs
public static class ServiceConfiguration
{
    public static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();
        
        // 1. Infrastructure 层注册
        services.AddSingleton<IDbContext, FreeSqlDbContext>();
        services.AddScoped<IRepository, FreeSqlRepository>();
        services.AddSingleton<ILoggerFactory, SerilogLoggerFactory>();
        
        // 2. Core 接口不需要注册（只是接口定义）
        
        // 3. HAL 层注册
        services.AddSingleton<IMotionDriver, MotionDriver>();
        services.AddSingleton<IVisionDriver, VisionDriver>();
        services.AddSingleton<IIODriver, IODriver>();
        services.AddSingleton<IHALDriver, HardwareDriver>();
        
        // 4. Services 层注册
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IConfigService, ConfigService>();
        services.AddScoped<IAlarmService, AlarmService>();
        services.AddScoped<IAuthService, AuthService>();
        
        // 5. BLL 层注册
        services.AddScoped<IWorkstationBLL, WorkstationBLL>();
        services.AddScoped<IRecipeBLL, RecipeBLL>();
        services.AddScoped<IVisionBLL, VisionBLL>();
        
        // 6. ViewModel 注册
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        
        // 7. View 注册
        services.AddTransient<MainWindow>();
        services.AddTransient<DashboardView>();
        
        return services.BuildServiceProvider();
    }
}
```

### 生命周期选择

```csharp
// Singleton：全局单一实例（数据库连接、配置读取）
services.AddSingleton<IDbContext, FreeSqlDbContext>();

// Scoped：每个请求/操作一个实例（业务逻辑、工作单元）
services.AddScoped<IWorkstationBLL, WorkstationBLL>();

// Transient：每次都创建新实例（ViewModel、View）
services.AddTransient<MainViewModel>();
```

---

## 🔍 调试步骤

### 问题：UI 无法调用 BLL 功能

**排查步骤**：
```
1. 检查 DI 容器是否正确注册
   → IWorkstationBLL 是否注册？
   → WorkstationBLL 的依赖（IHALDriver 等）是否都注册？

2. 检查 ViewModel 构造函数
   → 是否有 IWorkstationBLL 参数？
   → 是否正确注入？

3. 检查 View 的 DataContext
   → MainWindow.DataContext = new MainViewModel(provider.GetRequiredService<IWorkstationBLL>());

4. 检查 Binding 表达式
   → Command="{Binding StartCommand}" 是否正确？
   → ViewModel 中 StartCommand 是否正确定义？
```

### 问题：硬件操作异常

**排查步骤**：
```
1. 确认硬件是否已初始化
   → await _hal.Initialize();

2. 查看异常日志
   → Infrastructure 是否正确记录日志？

3. 检查异常处理
   → BLL 中是否有 try-catch？
   → 是否调用了 IAlarmService？

4. 测试硬件驱动
   → 创建单元测试直接测试 HAL 层
   → Mock 上层依赖，只测试驱动本身
```

---

## 📊 模块交互图

### 简化版
```
UI Layer
  ↓
ViewModel (RelayCommand)
  ↓
BLL Layer (业务流程)
  ├─→ HAL Layer (硬件驱动)
  └─→ Services Layer (业务服务)
      ↓
Infrastructure Layer (数据和日志)
      ↓
Common Layer (通用工具)
```

### 详细版（以启动工作流为例）
```
┌─ MainWindow.xaml ─────────────────┐
│  [Start Button]                   │
│  Command="{Binding StartCommand}" │
└───────────────┬───────────────────┘
                │ Click Event
┌───────────────▼───────────────────┐
│ MainViewModel                     │
│ ├─ StartCommand                   │
│ └─ IWorkstationBLL _workstation   │
└───────────────┬───────────────────┘
                │ Execute
┌───────────────▼───────────────────┐
│ WorkstationBLL.Start()            │
│                                   │
│ ├─ IHALDriver.Initialize()        │
│ ├─ IConfigService.GetRecipe()     │
│ ├─ IHALDriver.MoveRobot()         │
│ ├─ IVisionDriver.Capture()        │
│ └─ IAlarmService.RaiseAlarm()     │
└─────────────────────────────────┬─┘
        ├──────────┬──────────┐
        │          │          │
    ┌───▼────┐ ┌──▼────┐ ┌───▼───┐
    │HAL Drv │ │Servs  │ │Infra  │
    │(HW)    │ │(Logic)│ │(Data) │
    └───┬────┘ └──┬────┘ └───┬───┘
        │         │          │
        └─────────┼──────────┘
                  │
          ┌───────▼────────┐
          │ Infrastructure │
          │ ├─ Database    │
          │ └─ Logger      │
          └────────────────┘
```

---

## 💡 最佳实践

### ✅ 依赖注入最佳实践
- ✅ 依赖通过构造函数注入
- ✅ 在 DI 容器统一配置
- ✅ 使用接口而非实现类型
- ✅ 避免 Service Locator 模式（直接调用 provider.GetService）

### ✅ 接口设计最佳实践
- ✅ 单一职责原则
- ✅ 接口粒度合适
- ✅ 使用异步操作（Task）
- ✅ 明确的返回类型
- ✅ 异常处理清晰

### ✅ 错误处理最佳实践
- ✅ BLL 层捕获异常并处理
- ✅ 转换为业务异常
- ✅ 通过 IAlarmService 记录告警
- ✅ 记录详细日志

### ❌ 常见错误
- ❌ 越层调用（UI 直接调用 HAL）
- ❌ 硬编码依赖（new 对象）
- ❌ 同步操作造成阻塞
- ❌ 异常吞掉不记录
- ❌ 模块间强耦合

---

**下一步**：
- 查看 [架构设计文档](./Architecture.md) 了解完整架构
- 查看 [快速参考指南](./QuickStart.md) 获取快速上手
- 根据需要查看具体模块文档

